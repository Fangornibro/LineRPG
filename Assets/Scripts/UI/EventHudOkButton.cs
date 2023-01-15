using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class EventHudOkButton : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    private FightManager fm;
    private EventHud eh;
    private TextMeshProUGUI buttonText;
    private RectTransform rt;
    [SerializeField]
    private Sprite down, up;
    void Start()
    {
        fm = GameObject.Find("LevelDialogue").GetComponent<FightManager>();
        //Event hud
        eh = GameObject.Find("EventHud").GetComponent<EventHud>();
        //Button text
        buttonText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        //Rect Transform
        rt = GetComponent<RectTransform>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eh.EventString != "Defeat")
        {
            bool isAny = false;
            if (buttonText.text == "OK")
            {
                foreach (var i in eh.curRewardItems)
                {
                    if (i.isTakeable)
                    {
                        isAny = true;
                    }
                }
            }
            else
            {
                foreach (Icon i in eh.curRewardItems)
                {
                    if (i.isTakeable)
                    {
                        i.GetComponent<ItemEventSystem>().OnPointerClick(null);
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
    public void OnPointerDown(PointerEventData eventData)
    {
        rt.sizeDelta = new Vector2(270, 117);
        GetComponent<UnityEngine.UI.Image>().sprite = down;
        transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 14, 0);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        rt.sizeDelta = new Vector2(270, 135);
        GetComponent<UnityEngine.UI.Image>().sprite = up;
        transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 23, 0);
    }
}
