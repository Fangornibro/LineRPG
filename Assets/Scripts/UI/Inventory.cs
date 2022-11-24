using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public List<GameObject> InventoryUI = new List<GameObject>();
    public Transform menu;
    public List<Cell> cells;
    [HideInInspector]
    public bool isInventOpen = false;

    void Update()
    {
        for (int i = 0; i < InventoryUI.Count; i++)
        {
            if (InventoryUI[i].gameObject == null)
            {
                InventoryUI.RemoveAt(i);
            }
        } 
        //Inventory open/close
        if (Input.GetKeyDown(KeyCode.I) && !menu.GetComponent<Menu>().isMenuOpen && !DialogueStructure.isDialogueOpen || isInventOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            isInventOpen = !isInventOpen;
            ContextMenu.UnShow();
        }
        //Activation/deactivation of UI when inventory opened
        if (isInventOpen)
        {
            if (InventoryUI.Count != 0)
            {
                foreach (GameObject UI in InventoryUI)
                {
                    if (UI.GetComponent<Icon>() != null)
                    {
                        if (UI.GetComponent<Icon>().cell.GetComponent<CellType>().cellType != CellType.Type.Usable)
                        {
                            UI.SetActive(true);
                            GetComponent<Image>().raycastTarget = true;
                        }
                    }
                    else
                    {
                        UI.SetActive(true);
                        GetComponent<Image>().raycastTarget = true;
                    }
                }
            }
        }
        else
        {
            if (InventoryUI.Count != 0)
            {
                foreach (GameObject UI in InventoryUI)
                {
                    if (UI.GetComponent<Icon>() != null)
                    {
                        if (UI.GetComponent<Icon>().cell.GetComponent<CellType>().cellType != CellType.Type.Usable)
                        {
                            UI.SetActive(false);
                            GetComponent<Image>().raycastTarget = false;
                        }
                    }
                    else
                    {
                        UI.SetActive(false);
                        GetComponent<Image>().raycastTarget = false;
                    }
                }
            }
        }
    }
}