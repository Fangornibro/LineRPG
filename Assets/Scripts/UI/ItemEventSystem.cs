using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemEventSystem : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private Inventory inventory;
    private FightManager fm;

    private void Start()
    {
        fm = GameObject.Find("LevelDialogue").GetComponent<FightManager>();
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (GetComponent<Icon>().isTakeable)
        {
            foreach (Cell cell in inventory.GetComponent<Inventory>().cells)
            {
                if (cell.icon == null)
                {
                    Icon icon = GetComponent<Icon>();

                    icon.cell = cell;
                    icon.transform.SetParent(icon.cell.transform.parent);
                    icon.GetComponent<RectTransform>().anchoredPosition = cell.GetComponent<RectTransform>().anchoredPosition + new Vector2(-4.5f, 4.5f);
                    cell.icon = icon;
                    icon.transform.localScale = Vector3.one;
                    icon.isTakeable = false;
                    break;
                }
            }
        }
        else
        {
            if (eventData.button == PointerEventData.InputButton.Left && GetComponent<Icon>().cell.GetComponent<CellType>().cellType == CellType.Type.Usable && !inventory.isInventOpen)
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
        ContextMenu.Show(transform.GetComponent<Icon>().Name, transform.GetComponent<Icon>().rarity, transform.GetComponent<Icon>().description, transform.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.GetComponent<Image>().color = new Color(0.6603774f, 0.6603774f, 0.6603774f, 1);
        ContextMenu.UnShow();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!fm.startTempChecking && inventory.isInventOpen)
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
        if (!fm.startTempChecking)
        {
            transform.SetParent(GameObject.Find("Canvas").transform);
            if (inventory.isInventOpen)
            {
                ContextMenu.UnShow();
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!fm.startTempChecking && inventory.isInventOpen)
        {
            foreach (Cell cellinv in inventory.cells)
            {
                if (cellinv.GetComponent<Collider2D>().Distance(GetComponent<Collider2D>()).distance < cellinv.GetComponent<RectTransform>().sizeDelta.x / 4)
                {
                    GetComponent<Icon>().ChangeItemCell(cellinv);
                    ContextMenu.Show(GetComponent<Icon>().Name, GetComponent<Icon>().rarity, GetComponent<Icon>().description, transform.position);
                }
                else
                {
                    transform.position = GetComponent<Icon>().cell.transform.position;
                    transform.SetParent(GetComponent<Icon>().cell.transform.parent);
                }
            }
        }
    }
}
