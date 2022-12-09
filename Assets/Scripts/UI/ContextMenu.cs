using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ContextMenu : MonoBehaviour
{
    private static  GameObject contextMenuHudGO;
    private static  TextMeshProUGUI nameGO, RarityGO, descriptionGO;
    private void Start()
    {
        contextMenuHudGO = transform.Find("ContextMenuHud").gameObject;
        nameGO = contextMenuHudGO.transform.Find("Name").GetComponent<TextMeshProUGUI>();
        RarityGO = contextMenuHudGO.transform.Find("Rarity").GetComponent<TextMeshProUGUI>();
        descriptionGO = contextMenuHudGO.transform.Find("Description").GetComponent<TextMeshProUGUI>();
        contextMenuHudGO.SetActive(false);
    }
    public static void Show(string Name, string Rarity, string Description, Vector3 Position)
    {
        contextMenuHudGO.SetActive(true);
        nameGO.SetText(Name);
        RarityGO.SetText(Rarity);
        if (Rarity == "Common")
        {
            RarityGO.color = new Color(0.6981132f, 0.6981132f, 0.6981132f, 1f);
        }
        else if (Rarity == "Rare")
        {
            RarityGO.color = new Color(0.1960784f, 0.3921569f, 0.6627451f, 1f);
        }
        else if (Rarity == "Epic")
        {
            RarityGO.color = new Color(0.4470588f, 0.3686275f, 0.5607843f, 1f);
        }
        descriptionGO.SetText(Description);
        contextMenuHudGO.transform.position = Position;
    }
    public static void UnShow()
    {
        contextMenuHudGO.SetActive(false);
    }
}
