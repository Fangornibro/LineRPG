using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SelectCharacterButton : MonoBehaviour, IButton
{
    [SerializeField] private CharacterSelection characterSelection;
    public static PlayerSelection curPlayer;
    public static bool isTutuorial = true;
    void IButton.OnPointerClick()
    {
        if (!characterSelection.characters[2].isLocked)
        {
            transform.parent.GetComponent<ButtonScript>().deactivation();
            curPlayer = characterSelection.characters[2];
            SceneTransition.SwitchToScene("MainScene");
        }
    }

    public void Tutorial(bool toogleOn) {
        isTutuorial = !toogleOn;
    }
}
