using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharcterSelectionRightArrow : MonoBehaviour, IButton
{
    [SerializeField] private CharacterSelection characterSelection;
    void IButton.OnPointerClick()
    {
        characterSelection.ChangeSelectionRight();
    }
}
