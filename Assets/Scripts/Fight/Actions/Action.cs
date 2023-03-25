using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action : MonoBehaviour
{
    protected int value;
    protected float criticalChance, criticalDamage, missChance;
    protected AudioSource actionSound;
    protected bool isMagic, isAOE;

    public void SetValues(int value, float criticalChance, float criticalDamage, float missChance, AudioSource actionSound, bool isMagic, bool isAOE)
    {
        this.value = value;
        this.criticalChance = criticalChance;   
        this.criticalDamage = criticalDamage;
        this.missChance = missChance;
        this.actionSound = actionSound;
        this.isMagic = isMagic;
        this.isAOE = isAOE;
    }

    public void MakeAnAction(Character target, Character attacker)
    {
        try
        {
            if ((Buff)this)
            {
                target = attacker;
            }
        }
        catch
        {
            
        }
        if (isAOE)
        {
            Transform temp = GameManager.temp;
            for (int j = 0; j < temp.childCount; j++)
            {
                Enemy curEnemyForAOE = temp.GetChild(j).GetComponent<Enemy>();
                makeAnAction(curEnemyForAOE, attacker);
            }
        }
        else
        {
            makeAnAction(target, attacker);
        }
    }
    protected abstract void makeAnAction(Character target, Character attacker);
}
