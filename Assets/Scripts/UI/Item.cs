using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.EventSystems;

public class Item : MonoBehaviour, IPointerClickHandler
{
    public GameObject iconPrefab;
    private GameObject inventory;
    [HideInInspector]
    public GameObject icon;
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
                if (cell.GetComponent<CellType>().cellType == CellType.Type.Usable)
                {
                    icon.transform.SetParent(GameObject.Find("VisibleInventory").transform);
                }
                else
                {
                    icon.transform.SetParent(inventory.transform);
                }
                icon.GetComponent<RectTransform>().position = new Vector2(cell.x, cell.y);
                icon.GetComponent<Icon>().cell = cell;
                inventory.GetComponent<Inventory>().InventoryUI.Add(icon);
                cell.icon = icon.GetComponent<Icon>();
                Destroy(gameObject);
                break;
            }
        }
    }
}
