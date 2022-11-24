using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    //Name
    public string Name;
    //Damage
    public int damage;
    //Sound
    public string attackSound;
    //Player
    private Player player;
    //Effect
    public enum Effect { none, minusMana, throughArmor, armorUp, flock }
    public Effect effect;
    //Attack icon
    public Sprite attackIcon;
   
}
