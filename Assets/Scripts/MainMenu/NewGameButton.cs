using UnityEngine;


public class NewGameButton : MonoBehaviour, IButton
{
    void IButton.OnPointerClick()
    {
        SceneTransition.SwitchToScene("CharacterSelection");
    }
}
