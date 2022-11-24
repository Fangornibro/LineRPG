using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EventHud : MonoBehaviour
{
    private GameObject childHud, RunningAwayText, Reward;
    [HideInInspector]
    public TextMeshProUGUI LooseText, WarningText;
    private TextMeshProUGUI EventText;
    [HideInInspector]
    public string EventString;
    [HideInInspector]
    public List<Item> rewardItems = new List<Item>();
    [HideInInspector]
    public List<Item> curRewardItems = new List<Item>();
    void Start()
    {
        childHud = transform.Find("EventHudChild").gameObject;
        RunningAwayText = childHud.transform.Find("RunningAwayText").gameObject;
        Reward = childHud.transform.Find("Reward").gameObject;
        LooseText = childHud.transform.Find("LooseText").GetComponent<TextMeshProUGUI>();
        EventText = childHud.transform.Find("EventText").GetComponent<TextMeshProUGUI>();
        WarningText = Reward.transform.Find("WarningText").GetComponent<TextMeshProUGUI>();
        DeActivation();
    }

    public void Activation(string eventString)
    {
        EventString = eventString;
        childHud.SetActive(true);
        EventText.SetText(EventString);
        if (EventString == "Victory")
        {
            Reward.SetActive(true);
            RunningAwayText.SetActive(false);
            LooseText.gameObject.SetActive(false);
            WarningText.gameObject.SetActive(false);
            for (int i = 0; i < rewardItems.Count; i++)
            {
                Item curItem = Instantiate(rewardItems[i], Vector3.zero, Quaternion.Euler(0, 0, 0), Reward.transform);
                curRewardItems.Add(curItem);
                curItem.GetComponent<RectTransform>().anchoredPosition = Reward.transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition;
            }
            rewardItems.Clear();
        }
        else if (EventString == "Loose")
        {
            Reward.SetActive(false);
            RunningAwayText.SetActive(false);
            LooseText.gameObject.SetActive(true);
            WarningText.gameObject.SetActive(false);
        }
        else if (EventString == "Running away")
        {
            Reward.SetActive(false);
            RunningAwayText.SetActive(true);
            LooseText.gameObject.SetActive(false);
            WarningText.gameObject.SetActive(false);
        }
    }

    public void DeActivation()
    {
        childHud.SetActive(false);
    }
}
