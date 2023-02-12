using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharcterSelectionLeftArrow : MonoBehaviour, IButton
{
    [SerializeField] private CharacterSelection characterSelection;
    void IButton.OnPointerClick()
    {
        characterSelection.ChangeSelectionLeft();
    }
}
