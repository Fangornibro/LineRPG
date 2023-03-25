using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Initialisations")]
    public Inventory inventory;
    public SquadInfoPanel squadInfoPanel;
    public FocusButton focusButton;
    public Map map;
    public ContextMenu contextMenu;
    public CameraScript cameraScript;
    public BottomInventory bottomInventory;
    public FightManager fightManager;
    public DialogueStructure dialogueStructure;
    public EventHUD eventHUD;
    public GameObject timePanel, fightHUD, menu, noAttackSkill;
    [Space]
    [Space]
    [Header("PublicVariablesForPrefabs")]
    public static Player player;
    public AudioSource buttonDownSound, buttonUpSound, coinSound;
    public static AudioSource critSound, missSound;
    public AbilityOnCursor abilityOnCursor;
    public static Transform temp;
    private void Awake()
    {
        critSound = GameObject.Find("critSound").GetComponent<AudioSource>();
        missSound = GameObject.Find("missSound").GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        temp = GameObject.Find("Temp").transform;
    }

    void Update()
    {
        if (!SceneTransition.inAnimation)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (noAttackSkill.active)
                {
                    NoAttackSkillVisible(false);
                }
                else
                {
                    if (menu.active)
                    {
                        MenuVisible(false);
                    }
                    else
                    {
                        if (!dialogueStructure.isActiveAndEnabled && !inventory.isActiveAndEnabled && !squadInfoPanel.isActiveAndEnabled)
                        {
                            MenuVisible(true);
                        }
                    }
                    SquadInfoInvisible();
                    InventoryVisible(false);
                }
            }
            if (Input.GetKeyDown(KeyCode.I) && !dialogueStructure.isActiveAndEnabled)
            {
                InventoryVisible(!inventory.isActiveAndEnabled);
            }
        }
    }

    public void MenuVisible(bool isVisible)
    {
        if (isVisible)
        {
            menu.SetActive(true);
            Pause.PauseGame();
        }
        else
        {
            menu.SetActive(false);
            Pause.ResumeGame();
        }
    }

    public void NoAttackSkillVisible(bool isVisible)
    {
        if (isVisible)
        {
            noAttackSkill.SetActive(true);
        }
        else
        {
            noAttackSkill.SetActive(false);
        }
    }

    public void DialogueStructureVisible(bool isVisible)
    {
        if (isVisible)
        {
            dialogueStructure.gameObject.SetActive(true);
            BottomInventoryVisible(false);
        }
        else
        {
            dialogueStructure.gameObject.SetActive(false);
            BottomInventoryVisible(true);
        }
    }

    public void TimePanelVisible(bool isVisible)
    {
        if (isVisible)
        {
            timePanel.SetActive(true);
            timePanel.GetComponent<Animator>().SetBool("IsVisible", true);
        }
        else
        {
            timePanel.GetComponent<Animator>().SetBool("IsVisible", false);
            timePanel.SetActive(false);
        }
    }

    public void FightHUDVisible(bool isVisible)
    {
        if (isVisible)
        {
            fightHUD.SetActive(true);
        }
        else
        {
            fightHUD.SetActive(false);
        }
    }

    public void JustMapVisible(bool isVisible)
    {
        if (isVisible)
        {
            map.gameObject.SetActive(true);
        }
        else
        {
            map.gameObject.SetActive(false);
        }
    }

    public void CurRoomVisible(bool isVisible)
    {
        if (isVisible)
        {
            cameraScript.transform.position = new Vector3(150, 0, -15);
        }
        else
        {
            BottomInventoryVisible(false);
            if (map.room != null)
            {
                cameraScript.transform.position = new Vector3(map.room.transform.position.x, map.room.transform.position.y, -15);
            }
            else
            {
                cameraScript.transform.position = new Vector3(0, 0, -15);
            }
            cameraScript.isKinematic = false;
        }
    }

    public void MapVisible(bool isVisible)
    {
        JustMapVisible(isVisible);
        CurRoomVisible(!isVisible);
    }
    public void InventoryVisible(bool isVisible)
    {
        if (isVisible && !menu.active)
        {
            inventory.gameObject.SetActive(true);
            BottomInventoryVisible(true);
        }
        else
        {
            ContextMenuInvisible();
            inventory.gameObject.SetActive(false);
            if (!fightManager.startTempChecking)
            {
                BottomInventoryVisible(false);
            }
        }
    }

    public void BottomInventoryVisible(bool isVisible)
    {
        if (isVisible)
        {
            bottomInventory.gameObject.SetActive(true);
        }
        else
        {
            ContextMenuInvisible();
            bottomInventory.gameObject.SetActive(false);
        }
    }

    public void EventHUDVisible(EventHUD.statement s)
    {
        FightHUDVisible(false);
        eventHUD.gameObject.SetActive(true);
        BottomInventoryVisible(false);
        eventHUD.Activation(s);
    }

    public void EventHUDInvisible()
    {
        eventHUD.gameObject.SetActive(false);
    }

    public void SquadInfoVisible(Room Room, string EventName, string LocationName, Squad Squad)
    {
        squadInfoPanel.gameObject.SetActive(true);
        squadInfoPanel.RoomInfoReceiving(Room, EventName, LocationName, Squad);
        FocusPanelVisible(false);
    }

    public void SquadInfoInvisible()
    {
        squadInfoPanel.gameObject.SetActive(false);
        if (cameraScript.transform.position != new Vector3(150, 0, -15))
        {
            FocusPanelVisible(true);
        }
    }

    public void ContextMenuVisible(string Name, Item.Rarity Rarity, string Description, Vector3 Position, List<Sprite> abilityTypeSprites)
    {
        contextMenu.gameObject.SetActive(true);
        contextMenu.Show(Name, Rarity, Description, Position, abilityTypeSprites);
    }

    public void ContextMenuInvisible()
    {
        contextMenu.gameObject.SetActive(false);
    }

    public void FocusPanelVisible(bool isVisible)
    {
        if (isVisible)
        {
            focusButton.transform.parent.parent.gameObject.SetActive(true);
        }
        else
        {
            focusButton.transform.parent.parent.gameObject.SetActive(false);
        }
    }
}
