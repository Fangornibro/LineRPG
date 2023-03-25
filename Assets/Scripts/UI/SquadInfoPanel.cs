using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class SquadInfoPanel : MonoBehaviour
{
    
    [SerializeField] private List<Sprite> icons;
    [SerializeField] private Transform difficults;
    [SerializeField] private Sprite difficultEmpty, difficultFull;

    [Space]
    [Space]
    [Header("Initialisations")]
    [SerializeField] private Map map;
    [SerializeField] private Image faceIcon;
    [SerializeField] private TextMeshProUGUI eventName, personName, description;
    public void RoomInfoReceiving(Room room, string EventName, string LocationName, Squad Squad)
    {
        eventName.text = "";
        personName.text = "";
        description.text = "";
        for (int i = 0; i < difficults.childCount; i++)
        {
            difficults.GetChild(i).gameObject.SetActive(false);
        }
        faceIcon.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 300);
        if (EventName == "QuestionIcon")
        {
            eventName.text = "Mystery";
        }
        else if (EventName == "ChestIcon")
        {
            eventName.text = "Chest"; 
        }
        else if (EventName == "BreadIcon")
        {
            eventName.text = "Event";
        }
        else if (EventName == "ElderIcon")
        {
            eventName.text = "Event";
        }
        else if (EventName == "FightIcon")
        {
            eventName.text = "Fight";
            for (int i = 0; i < difficults.childCount; i++)
            {
                difficults.GetChild(i).gameObject.SetActive(true);
                if (i < Squad.difficult)
                {
                    difficults.GetChild(i).GetComponent<Image>().sprite = difficultFull;
                }
                else
                {
                    difficults.GetChild(i).GetComponent<Image>().sprite = difficultEmpty;
                }
            }
        }
        personName.text = Squad.squadName;
        description.text = Squad.description;
        faceIcon.sprite = Squad.GetComponent<SpriteRenderer>().sprite;
        float iconWidth = faceIcon.GetComponent<RectTransform>().sizeDelta.x / Squad.GetComponent<SpriteRenderer>().sprite.rect.width;
        float iconHeight = faceIcon.GetComponent<RectTransform>().sizeDelta.y / Squad.GetComponent<SpriteRenderer>().sprite.rect.height;
        faceIcon.GetComponent<RectTransform>().sizeDelta = new Vector2(Squad.GetComponent<SpriteRenderer>().sprite.rect.width * Mathf.Min(iconWidth, iconHeight), Squad.GetComponent<SpriteRenderer>().sprite.rect.height * Mathf.Min(iconWidth, iconHeight));
        transform.Find("LocationName").GetComponent<TextMeshProUGUI>().text = LocationName;
        map.RoomInfoReceiving(room, LocationName, Squad);
        transform.Find("StartFightButton").GetChild(0).GetComponent<StartFightButton>().room = room;
    }
}
