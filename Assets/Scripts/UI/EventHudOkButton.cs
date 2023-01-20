using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class EventHudOkButton : MonoBehaviour
{
    private FightManager fm;
    private EventHud eh;
    private TextMeshProUGUI buttonText;

    void Start()
    {
        fm = GameObject.Find("FightManager").GetComponent<FightManager>();
        //Event hud
        eh = GameObject.Find("EventHud").GetComponent<EventHud>();
        //Button text
        buttonText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    public void OnPointerClick()
    {
        if (eh.eventString != "Defeat")
        {
            bool isAny = false;
            if (buttonText.text == "OK")
            {
                foreach (var i in eh.curRewardItems)
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
                foreach (Icon i in eh.curRewardItems)
                {
                    if (i != null)
                    {
                        if (i.isTakeable)
                        {
                            i.GetComponent<ItemEventSystem>().takeAndUse();
                        }
                    }
                }
                eh.curRewardItems.Clear();
                buttonText.SetText("OK");
                eh.DeActivation();
                fm.BackToMap();
            }
            
            if (isAny)
            {
                eh.warningText.gameObject.SetActive(true);
                buttonText.SetText("Collect all");
            }
            else
            {
                eh.curRewardItems.Clear();
                eh.DeActivation();
                fm.BackToMap();
            }
        }
        else
        {
            //BackToMenu
        }
    }
}
