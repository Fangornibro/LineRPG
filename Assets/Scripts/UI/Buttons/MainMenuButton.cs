using UnityEngine;


public class MainMenuButton : MonoBehaviour, IButton
{
    void IButton.OnPointerClick()
    {
        transform.parent.GetComponent<ButtonScript>().deactivation();
        Pause.ResumeGame();
        SceneTransition.SwitchToScene("MainMenu");
    }
}
