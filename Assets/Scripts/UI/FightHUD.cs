using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightHUD : MonoBehaviour
{
    private Vector3 defaultPosition;
    private RectTransform rect;
    private Animator turnPanelAnimator;
    void Start()
    {
        rect = GetComponent<RectTransform>();
        defaultPosition = rect.position;
        turnPanelAnimator = transform.Find("TurnPanel").GetComponent<Animator>();
        VisibilityChange(false);
    }

    public void VisibilityChange(bool isVisible)
    {
        if (isVisible)
        {
            rect.position = defaultPosition;
            turnPanelAnimator.SetBool("IsVisible", true);
        }
        else
        {
            rect.position = Vector3.one * 4000;
            turnPanelAnimator.SetBool("IsVisible", false);
        }
    }
}
