using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using Unity.VisualScripting;
using UnityEngine.UIElements;

public class EventHudOkButton : MonoBehaviour
{
    private FightManager fm;
    private EventHud eh;
    private TextMeshProUGUI buttonText;
    //Effect
    private bool startLeaving = false;
    private Volume volume;
    private DepthOfField dof;
    private float time = 0;
    void Start()
    {
        //Effect
        volume = GameObject.Find("Global Volume").GetComponent<Volume>();
        volume.profile.TryGet(out dof);
        //Fight manager
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
                time = 0;
                dof.focalLength.value = 35;
                startLeaving = true;
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
                time = 0;
                dof.focalLength.value = 35;
                startLeaving = true;
                
            }
        }
        else
        {
            //BackToMenu
        }
    }
    private void Update()
    {
        if (startLeaving)
        {
            time += Time.deltaTime;
            dof.focalLength.value += time / 2;
            if (dof.focalLength.value >= 100)
            {
                startLeaving = false;
                dof.focalLength.value = 1;
                fm.BackToMap();
            }
        }
    }
}
