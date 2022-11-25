using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class AbilityOnCursor : MonoBehaviour
{
    private float isCursorDeafult = 0.1f;
    public bool isOnCursor = false;
    public int curDamageOrArmour = 0, curCost = 0;
    private Player player;
    public string abilityType;
    public AudioSource abilitySound;
    public Icon.Effect effect;
    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }
    public void newCursor(int damageOrArmour, int cost, Texture2D cursorTexture, string abilityType, AudioSource abilitySound, Icon.Effect effect)
    {
        if (player.curMana >= cost)
        {
            this.abilityType = abilityType;
            this.abilitySound = abilitySound;
            this.effect = effect;
            isCursorDeafult = 0.1f;
            Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
            curDamageOrArmour = damageOrArmour + player.passiveDamage;
            curCost = cost;
            isOnCursor = true;
        } 
    }
    void Update()
    {
        if (isCursorDeafult >= 0)
        {
            isCursorDeafult -= Time.deltaTime;
        }
        if (Input.GetMouseButtonDown(0) && isCursorDeafult <= 0)
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            curDamageOrArmour = 0;
            curCost = 0;
            isOnCursor = false;
        }   
    }
}
