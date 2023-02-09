using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;
public class SquadInfoPanel : MonoBehaviour
{
    [HideInInspector]
    public bool isOpened = false;
    private Vector2 defaultPos;
    private Image faceIcon;
    public List<Sprite> icons;
    [SerializeField]
    private Transform difficults;
    public Sprite difficultEmpty, difficultFull;

    private TextMeshProUGUI eventName, personName, description;
    void Start()
    {
        defaultPos = GetComponent<RectTransform>().anchoredPosition;
        faceIcon = transform.Find("SquadFaceIcon").GetComponent<Image>();
        eventName = transform.Find("EventName").GetComponent<TextMeshProUGUI>();
        personName = transform.Find("SquadName").GetComponent<TextMeshProUGUI>();
        description = transform.Find("Description").GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        if (isOpened && Input.GetKeyDown(KeyCode.Escape))
        {
            isOpened = false;
        }
        if (isOpened)
        {
            GetComponent<RectTransform>().anchoredPosition = defaultPos;
        }
        else
        {
            GetComponent<RectTransform>().anchoredPosition = new Vector2(-3000, -3000);
        }
    }

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
        if (LocationName != "None")
        {
            transform.Find("LocationName").GetComponent<TextMeshProUGUI>().text = LocationName.Replace("Icon", "");
        }
        else 
        {
            transform.Find("LocationName").GetComponent<TextMeshProUGUI>().text = "";
        }
        GameObject.Find("Map").GetComponent<Map>().RoomInfoReceiving(room, EventName, LocationName, Squad);
        transform.Find("StartFightButton").GetChild(0).GetComponent<StartFightButton>().roomPos = room.transform.position;
    }
}
