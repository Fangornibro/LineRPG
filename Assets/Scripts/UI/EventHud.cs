using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EventHud : MonoBehaviour
{
    private GameObject childHud, reward;
    [HideInInspector]
    public TextMeshProUGUI eventHudText, warningText;
    private TextMeshProUGUI eventText;
    [HideInInspector]
    public string eventString;
    [HideInInspector]
    public List<Icon> rewardItems = new List<Icon>();
    [HideInInspector]
    public List<Icon> curRewardItems = new List<Icon>();
    void Start()
    {
        childHud = transform.Find("EventHudChild").gameObject;
        reward = childHud.transform.Find("Reward").gameObject;
        eventHudText = childHud.transform.Find("EventHudText").GetComponent<TextMeshProUGUI>();
        warningText = reward.transform.Find("WarningText").GetComponent<TextMeshProUGUI>();
        eventText = childHud.transform.Find("EventText").GetComponent<TextMeshProUGUI>();
        DeActivation();
    }

    public void Activation(string eventString)
    {
        this.eventString = eventString;
        childHud.SetActive(true);
        eventText.SetText(this.eventString);
        eventHudText.text = "";
        warningText.gameObject.SetActive(false);
        if (this.eventString == "Victory")
        {
            reward.SetActive(true);
            eventHudText.text = "Reward:";
            for (int i = 0; i < rewardItems.Count; i++)
            {
                Icon curItem = Instantiate(rewardItems[i], Vector3.zero, Quaternion.Euler(0, 0, 0), reward.transform);
                if (curItem.GetComponent<CellType>().cellType == CellType.Type.CoinBag)
                {
                    curItem.damageOrArmourText.text = curItem.damageOrArmour.ToString();
                }
                curRewardItems.Add(curItem);
                curItem.GetComponent<RectTransform>().localPosition = reward.transform.GetChild(i).GetComponent<RectTransform>().localPosition + new Vector3(-4.5f, 4.5f);
            }
            rewardItems.Clear();
        }
        else if (this.eventString == "Running away")
        {
            reward.SetActive(false);
            eventHudText.text = "You ran away, so you didn't get any reward.";
        }
        else if (this.eventString == "Leaving")
        {
            reward.SetActive(false);
            eventHudText.text = "You just left.";
        }
        else if (this.eventString == "Defeat")
        {
            reward.SetActive(false);
        }
    }

    public void DeActivation()
    {
        childHud.SetActive(false);
    }
}
