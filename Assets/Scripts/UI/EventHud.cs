using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EventHud : MonoBehaviour
{
    private GameObject childHud, RunningAwayText, Reward;
    [HideInInspector]
    public TextMeshProUGUI DefeatText, WarningText;
    private TextMeshProUGUI EventText;
    [HideInInspector]
    public string EventString;
    [HideInInspector]
    public List<Icon> rewardItems = new List<Icon>();
    [HideInInspector]
    public List<Icon> curRewardItems = new List<Icon>();
    void Start()
    {
        childHud = transform.Find("EventHudChild").gameObject;
        RunningAwayText = childHud.transform.Find("RunningAwayText").gameObject;
        Reward = childHud.transform.Find("Reward").gameObject;
        DefeatText = childHud.transform.Find("DefeatText").GetComponent<TextMeshProUGUI>();
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
            DefeatText.gameObject.SetActive(false);
            WarningText.gameObject.SetActive(false);
            for (int i = 0; i < rewardItems.Count; i++)
            {
                Icon curItem = Instantiate(rewardItems[i], Vector3.zero, Quaternion.Euler(0, 0, 0), Reward.transform);
                curRewardItems.Add(curItem);
                curItem.GetComponent<RectTransform>().localPosition = Reward.transform.GetChild(i).GetComponent<RectTransform>().localPosition + new Vector3(-4.5f, 4.5f);
            }
            rewardItems.Clear();
        }
        else if (EventString == "Defeat")
        {
            Reward.SetActive(false);
            RunningAwayText.SetActive(false);
            DefeatText.gameObject.SetActive(true);
            WarningText.gameObject.SetActive(false);
        }
        else if (EventString == "Running away")
        {
            Reward.SetActive(false);
            RunningAwayText.SetActive(true);
            DefeatText.gameObject.SetActive(false);
            WarningText.gameObject.SetActive(false);
        }
    }

    public void DeActivation()
    {
        childHud.SetActive(false);
    }
}
