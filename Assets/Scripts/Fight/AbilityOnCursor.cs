using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityOnCursor : MonoBehaviour
{
    private float isCursorDeafult = 0.2f;
    public int curDamage;
    public void newCursor(int damage, Texture2D cursorTexture)
    {
        isCursorDeafult = 0.2f;
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
        curDamage = damage;
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
            curDamage = 0;
        }   
    }
}
