using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class AbilityOnCursor : MonoBehaviour
{
    private float isCursorDeafult = 0.1f;
    [HideInInspector]
    public bool isOnCursor = false;
    [HideInInspector]
    public int curDamageOrArmour = 0, curCost = 0;
    [HideInInspector]
    public float curCriticalChance = 0, curCriticalDamage = 0;
    private Player player;
    [HideInInspector]
    public Icon.Type abilityType;
    [HideInInspector]
    public AudioSource abilitySound;
    [HideInInspector]
    public Icon.Effect effect;
    [HideInInspector]
    public Icon ability;
    [SerializeField]
    private Sprite cellDefault, cellSelected;

    private Texture2D cursorTexture;
    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        cursorTexture = Resources.Load<Texture2D>("Cursors/AttackCursor");
    }
    public void newCursor(Icon ability)
    {
        if (player.curMana >= ability.cost)
        {
            this.ability = ability;
            abilityType = ability.type;
            abilitySound = ability.abilitySound;
            effect = ability.effect;
            isCursorDeafult = 0.1f;
            Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
            if (abilityType == Icon.Type.attack || abilityType == Icon.Type.magicAttack)
            {
                curDamageOrArmour = ability.damageOrArmour + player.passiveDamage;
            }
            else
            {
                curDamageOrArmour = ability.damageOrArmour;
            }
            curCriticalChance = ability.criticalChance;
            curCriticalDamage = ability.criticalDamage;
            curCost = ability.cost;
            isOnCursor = true;
        } 
    }
    void Update()
    {
        if (ability != null)
        {
            if (isOnCursor)
            {
                ability.cell.GetComponent<Image>().sprite = cellSelected;
            }
            else
            {
                ability.cell.GetComponent<Image>().sprite = cellDefault;
            }
        }
        
        if (isCursorDeafult >= 0)
        {
            isCursorDeafult -= Time.deltaTime;
        }
        if (Input.GetMouseButtonDown(0) && isCursorDeafult <= 0)
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            curDamageOrArmour = 0;
            curCost = 0;
            ability = null;
            isOnCursor = false;
        }   
    }
}
