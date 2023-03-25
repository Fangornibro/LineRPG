using TMPro;
using UnityEngine;

public class EventHudOkButton : MonoBehaviour, IButton
{
    [Header("Initialisations")]
    [SerializeField] private FightManager fightManager;
    [SerializeField] private EventHUD eventHUD;
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private ScreenFading screenFading;
    [SerializeField] private Map map;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Squad NonTutorialSquad, TutorialSquad;
    void IButton.OnPointerClick()
    {
        if (eventHUD.Statement != EventHUD.statement.Defeat)
        {
            bool isAny = false;
            if (buttonText.text == "OK")
            {
                foreach (var i in eventHUD.curRewardItems)
                {
                    if (i != null)
                    {
                        if (i.isTakeable)
                        {
                            isAny = true;
                        }
                    }
                }
            }
            else
            {
                foreach (Item i in eventHUD.curRewardItems)
                {
                    if (i != null)
                    {
                        if (i.isTakeable)
                        {
                            i.GetComponent<ItemEventSystem>().takeAndUse();
                        }
                    }
                }
                buttonText.SetText("OK");
                Off();
            }
            
            if (isAny)
            {
                eventHUD.warningText.gameObject.SetActive(true);
                buttonText.SetText("Collect all");
            }
            else
            {
                Off();
            }
        }
        else
        {
            transform.parent.GetComponent<ButtonScript>().deactivation();
            SceneTransition.SwitchToScene("MainMenu");
        }
    }
    public void Off()
    {
        eventHUD.curRewardItems.Clear();
        gameManager.EventHUDInvisible();
        if (eventHUD.Statement == EventHUD.statement.FirstFight)
        {
            fightManager.SpawnSquad(NonTutorialSquad);
        }
        else if (eventHUD.Statement == EventHUD.statement.FirstFightWithGeneration)
        {
            fightManager.SpawnSquad(TutorialSquad);
        }
        else
        {
            screenFading.StartFadingCoroutine(35, 100);
        }
    }
}
