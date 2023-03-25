using UnityEngine;


public class ExitButton : MonoBehaviour, IButton
{

    void IButton.OnPointerClick()
    {
        Application.Quit();
    }
}
