using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ContextMenu : MonoBehaviour
{
    private GameObject contextMenuHudGO;
    private Image rarityIcon;
    private TextMeshProUGUI nameGO, RarityGO, descriptionGO;
    [SerializeField]
    private Sprite commonIcon, rareIcon, epicIcon, legendaryIcon;
    private void Start()
    {
        contextMenuHudGO = transform.Find("ContextMenuHud").gameObject;
        nameGO = contextMenuHudGO.transform.Find("Name").GetComponent<TextMeshProUGUI>();
        RarityGO = contextMenuHudGO.transform.Find("Rarity").GetComponent<TextMeshProUGUI>();
        descriptionGO = contextMenuHudGO.transform.Find("Description").GetComponent<TextMeshProUGUI>();
        contextMenuHudGO.SetActive(false);

        rarityIcon = contextMenuHudGO.transform.Find("RarityIcon").gameObject.GetComponent<Image>();
    }
    public void Show(string Name, string Rarity, string Description, Vector3 Position)
    {
        contextMenuHudGO.SetActive(true);
        nameGO.SetText(Name);
        RarityGO.SetText(Rarity);
        if (Rarity == "Common")
        {
            RarityGO.color = new Color(0.4666667f, 0.4666667f, 0.4666667f, 1f);
            rarityIcon.sprite = commonIcon;
        }
        else if (Rarity == "Rare")
        {
            RarityGO.color = new Color(0.2392157f, 0.3803922f, 0.7333333f, 1f);
            rarityIcon.sprite = rareIcon;
        }
        else if (Rarity == "Epic")
        {
            RarityGO.color = new Color(0.5137255f, 0.2431373f, 0.5803922f, 1f);
            rarityIcon.sprite = epicIcon;
        }
        else if (Rarity == "Legendary")
        {
            RarityGO.color = new Color(0.7803922f, 0.4509804f, 0.1215686f, 1f);
            rarityIcon.sprite = legendaryIcon;
        }
        else if (Rarity == "Gold")
        {
            RarityGO.color = new Color(1f, 0.881f, 0f, 1f);
            rarityIcon.sprite = commonIcon;
        }
        descriptionGO.SetText(Description);
        if (Input.mousePosition.y < Screen.height/2)
        {
            contextMenuHudGO.transform.position = Position;
        }
        else
        {
            contextMenuHudGO.transform.position = Position - new Vector3(0, contextMenuHudGO.GetComponent<RectTransform>().sizeDelta.y/2);
        }
    }
    public void UnShow()
    {
        contextMenuHudGO.SetActive(false);
    }
}
