using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EventHUD : MonoBehaviour
{
    [HideInInspector]
    public List<Item> rewardItems = new List<Item>();
    [HideInInspector]
    public List<Item> curRewardItems = new List<Item>();

    [Header("Initialisations")]
    [SerializeField] private GameObject reward;
    [Space]
    [Space]
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI eventText;
    public TextMeshProUGUI eventHudText, warningText;

    public enum statement { Victory, RunningAway, Leaving, Defeat, FirstFight, FirstFightWithGeneration };
    [HideInInspector] public statement Statement;

    public void Activation(statement Statement)
    {
        this.Statement = Statement;
        eventText.SetText(Statement.ToString());
        eventHudText.text = "";
        warningText.gameObject.SetActive(false);
        if (Statement == statement.Victory || Statement == statement.FirstFight || Statement == statement.FirstFightWithGeneration)
        {
            eventText.SetText("Victory");
            reward.SetActive(true);
            eventHudText.text = "Reward:";
            for (int i = 0; i < rewardItems.Count; i++)
            {
                Item curItem = Instantiate(rewardItems[i], Vector3.zero, Quaternion.Euler(0, 0, 0), reward.transform);
                if (curItem.GetComponent<CellType>().cellType == CellType.Type.CoinBag)
                {
                    curItem.damageOrArmourText.text = curItem.damage.ToString();
                }
                curRewardItems.Add(curItem);
                curItem.GetComponent<RectTransform>().localPosition = reward.transform.GetChild(i).GetComponent<RectTransform>().localPosition + new Vector3(-4.5f, 4.5f);
            }
            rewardItems.Clear();
        }
        else if (Statement == statement.RunningAway)
        {
            reward.SetActive(false);
            eventHudText.text = "You ran away, so you didn't get any reward.";
        }
        else if (Statement == statement.Leaving)
        {
            reward.SetActive(false);
            eventHudText.text = "You just left.";
        }
        else if (Statement == statement.Defeat)
        {
            reward.SetActive(false);
        }
    }
}
