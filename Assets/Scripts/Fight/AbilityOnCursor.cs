using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
    public string abilityType;
    [HideInInspector]
    public AudioSource abilitySound;
    [HideInInspector]
    public Icon.Effect effect;
    [HideInInspector]
    public Icon ability;
    [SerializeField]
    private Sprite cellDefault, cellSelected;
    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }
    public void newCursor(Icon ability)
    {
        if (player.curMana >= ability.cost)
        {
            this.ability = ability;
            abilityType = ability.AttackBlockOrPassive;
            abilitySound = ability.abilitySound;
            effect = ability.effect;
            isCursorDeafult = 0.1f;
            Cursor.SetCursor(ability.cursorTexture, Vector2.zero, CursorMode.Auto);
            if (abilityType == "attack")
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
