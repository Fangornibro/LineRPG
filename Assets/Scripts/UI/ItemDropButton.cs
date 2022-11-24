using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDropButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public static bool canPlayerShoot = true;
    public Transform Icon;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (Icon.GetComponent<Icon>().item.GetComponent<CellType>().cellType != CellType.Type.Quest)
            {
                Icon.GetComponent<Icon>().DropOneItem();
            }
            SelectionContextMenu.UnShow();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        canPlayerShoot = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        canPlayerShoot = true;
    }
}
