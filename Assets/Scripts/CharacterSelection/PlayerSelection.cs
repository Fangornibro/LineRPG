using System.Collections.Generic;
using UnityEngine;

public class PlayerSelection : MonoBehaviour
{
    [Header("For character selection")]
    public bool isLocked;
    [TextArea(2, 4)] public string description;


    [Space]
    [Space]
    [Header("Stats")]
    public int maxMana;
    public int maxHP;
    public float passiveCriticalChance, passiveCriticalDamage, passiveMissChance, evasionChance;


    [Space]
    [Space]
    public RuntimeAnimatorController animatorController;


    [Space]
    [Space]
    public List<ItemPool> startItems;
}
