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
    private Sprite commonIcon, rareIcon, epicIcon, legendaryIcon, defaultAbilityTypeIcon;
    public List<Image> abilityTypeIcons;
    private void Start()
    {
        contextMenuHudGO = transform.Find("ContextMenuHud").gameObject;
        nameGO = contextMenuHudGO.transform.Find("Name").GetComponent<TextMeshProUGUI>();
        RarityGO = contextMenuHudGO.transform.Find("Rarity").GetComponent<TextMeshProUGUI>();
        descriptionGO = contextMenuHudGO.transform.Find("Description").GetComponent<TextMeshProUGUI>();
        contextMenuHudGO.SetActive(false);

        rarityIcon = contextMenuHudGO.transform.Find("RarityIcon").gameObject.GetComponent<Image>();
    }
    public void Show(string Name, Icon.Rarity Rarity, string Description, Vector3 Position, List<Sprite> abilityTypeSprites)
    {
        contextMenuHudGO.SetActive(true);
        nameGO.SetText(Name);
        for(int i = 0; i < abilityTypeIcons.Count; i++)
        {
            if (i < abilityTypeSprites.Count && abilityTypeSprites[i] != null)
            {
                abilityTypeIcons[i].sprite = abilityTypeSprites[i];
            }
            else
            {
                abilityTypeIcons[i].sprite = defaultAbilityTypeIcon;
            }
        }
        if (Rarity == Icon.Rarity.common)
        {
            RarityGO.SetText("Common");
            RarityGO.color = new Color(0.4666667f, 0.4666667f, 0.4666667f, 1f);
            rarityIcon.sprite = commonIcon;
        }
        else if (Rarity == Icon.Rarity.rare)
        {
            RarityGO.SetText("Rare");
            RarityGO.color = new Color(0.2392157f, 0.3803922f, 0.7333333f, 1f);
            rarityIcon.sprite = rareIcon;
        }
        else if (Rarity == Icon.Rarity.epic)
        {
            RarityGO.SetText("Epic");
            RarityGO.color = new Color(0.5137255f, 0.2431373f, 0.5803922f, 1f);
            rarityIcon.sprite = epicIcon;
        }
        else if (Rarity == Icon.Rarity.legendary)
        {
            RarityGO.SetText("Legendary");
            RarityGO.color = new Color(0.7803922f, 0.4509804f, 0.1215686f, 1f);
            rarityIcon.sprite = legendaryIcon;
        }
        else if (Rarity == Icon.Rarity.gold)
        {
            RarityGO.SetText("Gold");
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
