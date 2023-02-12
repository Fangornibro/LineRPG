using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [HideInInspector]
    public bool isInventOpen = false;


    public List<Cell> cells;


    [Space]
    [Space]
    [Header("Initialisations")]
    [SerializeField] private Transform menu;
    [SerializeField] private ContextMenu contextMenu;
    [SerializeField] private DialogueStructure dialogueStructure;
    void Update()
    {
        //Inventory open/close
        if (Input.GetKeyDown(KeyCode.I) && !menu.GetComponent<Menu>().isMenuOpen && !dialogueStructure.isDialogueOpen || isInventOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            if (isInventOpen)
            {
                foreach (Cell c in cells)
                {
                    if (c.icon != null)
                    {
                        c.icon.transform.SetParent(c.transform.parent);
                        c.icon.GetComponent<RectTransform>().anchoredPosition = c.GetComponent<RectTransform>().anchoredPosition;
                    }
                }
            }
            isInventOpen = !isInventOpen;
            contextMenu.UnShow();
        }
        //Activation/deactivation of UI when inventory opened
        if (isInventOpen)
        {
            GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 300);
        }
        else
        {
            GetComponent<RectTransform>().anchoredPosition = new Vector2(-3000, -3000);
        }
    }
}