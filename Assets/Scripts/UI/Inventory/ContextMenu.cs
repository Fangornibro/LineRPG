using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ContextMenu : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI RarityText;
    [SerializeField] private TextMeshProUGUI descriptionText;


    [Space]
    [Space]
    [Header("Icons")]
    [SerializeField] private Image rarityIcon;
    [SerializeField] private Sprite commonIcon, rareIcon, epicIcon, legendaryIcon, defaultAbilityTypeIcon;
    public List<Image> abilityTypeIcons;
    public void Show(string Name, Item.Rarity Rarity, string Description, Vector3 Position, List<Sprite> abilityTypeSprites)
    {
        nameText.SetText(Name);
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
        if (Rarity == Item.Rarity.common)
        {
            RarityText.SetText("Common");
            RarityText.color = new Color(0.4666667f, 0.4666667f, 0.4666667f, 1f);
            rarityIcon.sprite = commonIcon;
        }
        else if (Rarity == Item.Rarity.rare)
        {
            RarityText.SetText("Rare");
            RarityText.color = new Color(0.2392157f, 0.3803922f, 0.7333333f, 1f);
            rarityIcon.sprite = rareIcon;
        }
        else if (Rarity == Item.Rarity.epic)
        {
            RarityText.SetText("Epic");
            RarityText.color = new Color(0.5137255f, 0.2431373f, 0.5803922f, 1f);
            rarityIcon.sprite = epicIcon;
        }
        else if (Rarity == Item.Rarity.legendary)
        {
            RarityText.SetText("Legendary");
            RarityText.color = new Color(0.7803922f, 0.4509804f, 0.1215686f, 1f);
            rarityIcon.sprite = legendaryIcon;
        }
        else if (Rarity == Item.Rarity.gold)
        {
            RarityText.SetText("Gold");
            RarityText.color = new Color(1f, 0.881f, 0f, 1f);
            rarityIcon.sprite = commonIcon;
        }
        descriptionText.SetText(Description);
        if (Input.mousePosition.y < Screen.height/2)
        {
            transform.position = Position;
        }
        else
        {
            transform.position = Position - new Vector3(0, GetComponent<RectTransform>().sizeDelta.y/2);
        }
    }
}
