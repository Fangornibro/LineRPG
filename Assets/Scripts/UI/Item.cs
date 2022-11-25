using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.EventSystems;

public class Item : MonoBehaviour, IPointerClickHandler
{
    public Icon iconPrefab;
    private GameObject inventory;
    [HideInInspector]
    public Icon icon;
    public string Name;
    private void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Inventory");
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        foreach (Cell cell in inventory.GetComponent<Inventory>().cells)
        {
            if (cell.icon == null && (cell.GetComponent<CellType>().cellType == GetComponent<CellType>().cellType || cell.GetComponent<CellType>().cellType == CellType.Type.Everything))
            {
                icon = Instantiate(iconPrefab);
                icon.cell = cell;
                icon.transform.SetParent(icon.cell.transform.parent);
                icon.GetComponent<RectTransform>().anchoredPosition = cell.GetComponent<RectTransform>().anchoredPosition;
                cell.icon = icon;
                Destroy(gameObject);
                break;
            }
        }
    }
}
