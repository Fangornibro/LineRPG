using System.Collections.Generic;
using UnityEngine;
public class VisibleInventory : MonoBehaviour
{
    public List<Cell> cells;


    [Space]
    [Space]
    [Header("Initialisations")]
    [SerializeField] private FightManager fightManager;
    [SerializeField] private Inventory inv;
    [SerializeField] private Player player;
    [SerializeField] private DialogueStructure dialogueStructure;
    void Update()
    {
        foreach (Cell c in cells)
        {
            if (Input.GetKeyDown(c.key) && c.icon != null)
            {
                c.icon.Use();
            }
        }

        if (dialogueStructure.isDialogueOpen || (!fightManager.startTempChecking && !inv.isInventOpen))
        {
            GetComponent<RectTransform>().anchoredPosition = new UnityEngine.Vector2(-3000, -3000);
        }
        else
        {
            GetComponent<RectTransform>().anchoredPosition = UnityEngine.Vector2.zero;
        }
    }

    public void checkPassive()
    {
        player.passiveMana = 0;
        player.passiveDamage = 0;
        player.maxPassiveDamage = 0;
        player.passiveArmor = 0;
        foreach (Cell c in cells)
        {
            if (c.icon != null)
            {
                if (c.icon.type == Icon.Type.passive)
                {
                    c.icon.UpdatePassiveItem();
                }
            }
        }
    }
}
