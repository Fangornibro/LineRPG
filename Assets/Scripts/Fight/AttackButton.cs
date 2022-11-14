using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackButton : MonoBehaviour
{
    public int damage;
    public Texture2D cursorTexture;
    private void OnMouseDown()
    {
        AbilityOnCursor abilityOnCursor = GameObject.Find("AbilityOnCursor").GetComponent<AbilityOnCursor>();
        abilityOnCursor.newCursor(damage, cursorTexture);
    }

}
