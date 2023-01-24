using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    [HideInInspector]
    public float x, y;
    public Icon icon;
    public KeyCode key;

    private AbilityOnCursor abilityOnCursor;
    [SerializeField]
    private Sprite cellSelected, cellDefault;
    private void Start()
    {
        abilityOnCursor = GameObject.Find("AbilityOnCursor").GetComponent<AbilityOnCursor>();
        x = GetComponent<RectTransform>().position.x;
        y = GetComponent<RectTransform>().position.y;
        icon = null;
    }

    private void Update()
    {
        if (icon != null && cellSelected != null)
        {
            if (abilityOnCursor.ability != null)
            {
                if (icon == abilityOnCursor.ability)
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
