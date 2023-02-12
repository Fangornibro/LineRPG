using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCharacterButton : MonoBehaviour, IButton
{
    [SerializeField] private CharacterSelection characterSelection;
    public static PlayerSelection curPlayer;
    void IButton.OnPointerClick()
    {
        if (!characterSelection.characters[2].isLocked)
        {
            curPlayer = characterSelection.characters[2];
            SceneTransition.SwitchToScene("MainScene");
        }
    }
}
