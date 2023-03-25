using UnityEngine;


public class BackToTheGameButton : MonoBehaviour, IButton
{
    [SerializeField] private GameManager gameManager;
    void IButton.OnPointerClick()
    {
        gameManager.MenuVisible(false);
    }
}
