using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    //Damage
    public int damage;
    public float criticalChance, criticalDamage;
    //Sound
    public AudioSource attackSound;
    //Effect
    public enum Effect { none, defaultAttack, minusMana, throughArmor, armorUp, flock, runningAway, weakness, poison, segmentation }
    public Effect effect;
    //Attack icon
    public Sprite attackIcon;
   
}
