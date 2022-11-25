using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDropButton : MonoBehaviour, IPointerClickHandler
{
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
}
