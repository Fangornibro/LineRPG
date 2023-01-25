using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnPanel : MonoBehaviour
{
    public RectTransform text;
    private Vector3 defaultPosition;
    private void Start()
    {
        defaultPosition = text.position;
    }
    public void textShowUP()
    {
        text.position = defaultPosition;
    }
    public void textHideUP()
    {
        text.position = Vector3.one * 4000;
    }
}
