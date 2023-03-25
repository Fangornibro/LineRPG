using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuButton : MonoBehaviour, IButton
{
    [SerializeField] private GameManager uiManager;
    void IButton.OnPointerClick()
    {
        uiManager.MenuVisible(true);
    }
}

