using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ContextMenu : MonoBehaviour
{
    private static  GameObject contextMenuHudGO;
    private static  TextMeshProUGUI nameGO, itemTypeGO, descriptionGO;
    private void Start()
    {
        contextMenuHudGO = transform.Find("ContextMenuHud").gameObject;
        nameGO = contextMenuHudGO.transform.Find("Name").GetComponent<TextMeshProUGUI>();
        itemTypeGO = contextMenuHudGO.transform.Find("ItemType").GetComponent<TextMeshProUGUI>();
        descriptionGO = contextMenuHudGO.transform.Find("Description").GetComponent<TextMeshProUGUI>();
        contextMenuHudGO.SetActive(false);
    }
    public static void Show(string Name, string ItemType, string Description, Vector3 Position)
    {
        contextMenuHudGO.SetActive(true);
        nameGO.SetText(Name);
        itemTypeGO.SetText(ItemType);
        descriptionGO.SetText(Description);
        contextMenuHudGO.transform.position = Position;
    }
    public static void UnShow()
    {
        contextMenuHudGO.SetActive(false);
    }
}
