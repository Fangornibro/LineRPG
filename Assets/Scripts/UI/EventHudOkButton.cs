using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class EventHudOkButton : MonoBehaviour, IPointerClickHandler
{
    private FightManager fm;
    private EventHud eh;
    private TextMeshProUGUI buttonText;
    void Start()
    {
        fm = GameObject.Find("LevelDialogue").GetComponent<FightManager>();
        //Event hud
        eh = GameObject.Find("EventHud").GetComponent<EventHud>();
        //Button text
        buttonText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eh.EventString != "Loose")
        {
            bool isAny = false;
            if (buttonText.text == "OK")
            {
                foreach (var i in eh.curRewardItems)
                {
                    if (i != null)
                    {
                        isAny = true;
                    }
                }
            }
            else
            {
                foreach (Item i in eh.curRewardItems)
                {
                    if (i != null)
                    {
                        GameObject.Destroy(i.gameObject);
                    }
                }
                eh.curRewardItems.Clear();
                buttonText.SetText("OK");
                eh.DeActivation();
                fm.BackToMap();
            }
            
            if (isAny)
            {
                eh.WarningText.gameObject.SetActive(true);
                buttonText.SetText("Remove reward");
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
