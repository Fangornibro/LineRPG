using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    //Damage
    public int damage;
    [SerializeField] private float criticalChance, criticalDamage, missChance;
    //Sound
    public AudioSource attackSound;
    //Effect
    public Action action;
    //Attack icon
    public Sprite attackIcon;
    public bool isMagic, isAOE;
    public void SetValues(AudioSource abilitySound)
    {
        action.SetValues(damage, criticalChance, criticalDamage, missChance, abilitySound, isMagic, isAOE);
    }
   
}
