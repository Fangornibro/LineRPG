using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    [HideInInspector]
    public float x, y;
    public Item icon;
    public KeyCode key;

    [SerializeField] private AbilityOnCursor abilityOnCursor;
    [SerializeField] private Sprite cellSelected, cellDefault;
    private void Start()
    {
        x = GetComponent<RectTransform>().position.x;
        y = GetComponent<RectTransform>().position.y;
    }

    private void Update()
    {
        if (icon != null && cellSelected != null)
        {
            if (abilityOnCursor.action != null)
            {
                if (icon == abilityOnCursor.action)
                {
                    GetComponent<Image>().sprite = cellSelected;
                }
                else
                {
                    GetComponent<Image>().sprite = cellDefault;
                }
            }
            else
            {
                GetComponent<Image>().sprite = cellDefault;
            }
        }
    }
}
