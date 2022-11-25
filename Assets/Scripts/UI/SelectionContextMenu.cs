using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SelectionContextMenu : MonoBehaviour
{
    private static GameObject selectionContextMenuHudGO;
    private Inventory inv;
    private void Start()
    {
        selectionContextMenuHudGO = transform.Find("SelectionContextMenuHud").gameObject;
        UnShow();
        inv = GameObject.Find("Inventory").GetComponent<Inventory>();
    }
    public static void Show()
    {
        selectionContextMenuHudGO.transform.position = Input.mousePosition;
    }
    public static void UnShow()
    {
        selectionContextMenuHudGO.transform.position = new Vector2(-3000, -3000);
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0) || !inv.isInventOpen)
        {
            UnShow();
        }
    }
}
