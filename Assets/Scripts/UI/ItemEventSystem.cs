using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemEventSystem : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public GameObject iconPrefab;
    private GameObject inventory, DropButtonGO, UseButtonGO;
    private void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Inventory");
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (inventory.GetComponent<Inventory>().isInventOpen)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                SelectionContextMenu.Show();
                DropButtonGO = GameObject.Find("DropButton");
                DropButtonGO.GetComponent<ItemDropButton>().Icon = transform;
                UseButtonGO = GameObject.Find("UseButton");
                UseButtonGO.GetComponent<ItemUseButton>().Icon = transform;

            }
        }
        else
        {
            if (eventData.button == PointerEventData.InputButton.Left && GetComponent<Icon>().cell.GetComponent<CellType>().cellType == CellType.Type.Usable)
            {
                GetComponent<Icon>().Use();
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right || eventData.button == PointerEventData.InputButton.Left)
        {
            transform.GetComponent<Image>().color = new Color(0.3584906f, 0.3584906f, 0.3584906f, 1);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right || eventData.button == PointerEventData.InputButton.Left)
        {
            transform.GetComponent<Image>().color = new Color(0.6603774f, 0.6603774f, 0.6603774f, 1);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.GetComponent<Image>().color = Color.white;
        ContextMenu.Show(transform.GetComponent<Icon>().name, transform.GetComponent<Icon>().itemType, transform.GetComponent<Icon>().description, transform.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.GetComponent<Image>().color = new Color(0.6603774f, 0.6603774f, 0.6603774f, 1);
        ContextMenu.UnShow();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (inventory.GetComponent<Inventory>().isInventOpen)
        {
            ContextMenu.UnShow();
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                transform.position = eventData.pointerCurrentRaycast.screenPosition;
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        GetComponent<Icon>().transform.SetParent(GameObject.Find("Canvas").transform);
        if (inventory.GetComponent<Inventory>().isInventOpen)
        {
            ContextMenu.UnShow();
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (inventory.GetComponent<Inventory>().isInventOpen)
        {
            foreach (Cell cellinv in inventory.GetComponent<Inventory>().cells)
            {
                if (cellinv.GetComponent<Collider2D>().Distance(GetComponent<Collider2D>()).distance < cellinv.GetComponent<RectTransform>().sizeDelta.x / 4)
                {
                    transform.GetComponent<Icon>().ChangeItemCell(cellinv);
                    ContextMenu.Show(transform.GetComponent<Icon>().name, transform.GetComponent<Icon>().itemType, transform.GetComponent<Icon>().description, transform.position);
                }
                else
                {
                    transform.position = transform.GetComponent<Icon>().cell.transform.position;
                }
            }
        }
    }
}
