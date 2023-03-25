using UnityEngine;


public class NewGameButton : MonoBehaviour, IButton
{

    void IButton.OnPointerClick()
    {
        transform.parent.GetComponent<ButtonScript>().deactivation();
        SceneTransition.SwitchToScene("CharacterSelection");
    }
}
