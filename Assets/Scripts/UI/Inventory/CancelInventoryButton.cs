using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CancelInventoryButton : MonoBehaviour, IButton
{
    private Inventory inv;

    void Start()
    {
        inv = GameObject.Find("Inventory").GetComponent<Inventory>();
    }

    void IButton.OnPointerClick()
    {
        inv.isInventOpen = false;
    }
}

