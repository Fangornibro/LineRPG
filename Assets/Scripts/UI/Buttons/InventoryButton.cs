using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryButton : MonoBehaviour, IButton
{
    [SerializeField] private GameManager uiManager;
    void IButton.OnPointerClick()
    {
        uiManager.InventoryVisible(true);
    }
}

