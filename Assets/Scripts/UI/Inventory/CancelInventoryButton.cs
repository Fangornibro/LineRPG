using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CancelInventoryButton : MonoBehaviour
{
    private Inventory inv;

    void Start()
    {
        inv = GameObject.Find("Inventory").GetComponent<Inventory>();
    }

    public void OnPointerClick()
    {
        inv.isInventOpen = false;
    }
}

