using System.Collections.Generic;
using UnityEngine;
public class BottomInventory : MonoBehaviour
{
    [HideInInspector] public List<Cell> cells;


    [Space]
    [Space]
    [Header("Initialisations")]
    [SerializeField] private Player player;
    void Update()
    {
        foreach (Cell c in cells)
        {
            if (Input.GetKeyDown(c.key) && c.icon != null)
            {
                c.icon.Use();
            }
        }
    }

    public void checkPassive()
    {
        player.passiveMana = 0;
        player.plusDamage = 0;
        player.maxPassiveDamage = 0;
        player.passiveArmor = 0;
        foreach (Cell c in cells)
        {
            if (c.icon != null)
            {
                if (c.icon.type == Item.Type.passive)
                {
                    c.icon.UpdatePassiveItem();
                }
            }
        }
    }
}
