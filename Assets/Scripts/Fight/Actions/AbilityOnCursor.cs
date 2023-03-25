using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class AbilityOnCursor : MonoBehaviour
{
    private float isCursorDeafult = 0.1f;
    [HideInInspector] public bool isOnCursor = false, isMagic;
    [SerializeField] private Player player;
    [HideInInspector] public Action action;
    [HideInInspector] public Item.Type abilityType;
    [HideInInspector] public int curCost;





    private Image selectedCell;
    [SerializeField] private Sprite cellDefault, cellSelected;
    private Texture2D cursorTexture;
    private void Start()
    {
        cursorTexture = Resources.Load<Texture2D>("Cursors/AttackCursor");
    }
    public void newCursor(Item item)
    {
        if (player.curMana >= item.cost)
        {
            selectedCell = item.cell.GetComponent<Image>();
            action = item.action;
            action.SetValues(item.damage, item.criticalChance, item.criticalDamage, item.missChance, item.abilitySound, item.isMagic, item.isAOE);
            abilityType = item.type;
            curCost = item.cost;
            isMagic = item.isMagic;
            isCursorDeafult = 0.1f;
            Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
            isOnCursor = true;
        } 
    }
    void Update()
    {
        if (action != null)
        {
            if (isOnCursor)
            {
                selectedCell.sprite = cellSelected;
            }
            else
            {
                selectedCell.sprite = cellDefault;
            }
        }
        
        if (isCursorDeafult >= 0)
        {
            isCursorDeafult -= Time.deltaTime;
        }
        if (Input.GetMouseButtonDown(0) && isCursorDeafult <= 0)
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            action = null;
            isOnCursor = false;
        }   
    }
}
