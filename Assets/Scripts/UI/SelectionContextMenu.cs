using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SelectionContextMenu : MonoBehaviour
{
    private static GameObject selectionContextMenuHudGO;
    private void Start()
    {
        selectionContextMenuHudGO = transform.Find("SelectionContextMenuHud").gameObject;
        selectionContextMenuHudGO.SetActive(false);
    }
    public static void Show()
    {
        selectionContextMenuHudGO.SetActive(true);
        selectionContextMenuHudGO.transform.position = Input.mousePosition;
    }
    public static void UnShow()
    {
        selectionContextMenuHudGO.SetActive(false);
    }
}
