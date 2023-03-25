using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CancelSquadInfoButton : MonoBehaviour, IButton
{
    [Header("Initialisations")]
    [SerializeField] private RoomSelector roomSelector;
    [SerializeField] private GameManager uiManager;
    void IButton.OnPointerClick()
    {
        roomSelector.ChangeSelection(null);
        uiManager.SquadInfoInvisible();
    }
}

