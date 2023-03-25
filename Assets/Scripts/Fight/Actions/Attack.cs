using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public abstract class Attack : Action
{
    [HideInInspector] protected bool isMiss, isEvasion, isCrit, isThroughArmor = false;

    protected abstract void specialEffect(Character target, Character attacker);
    protected override void makeAnAction(Character target, Character attacker)
    {
        value -= attacker.minusDamage;
        if (!isMagic)
        {
            if (UnityEngine.Random.Range(0, 100) <= attacker.passiveMissChance + missChance + attacker.plusMissChance)
            {
                isMiss = true;
            }
            else
            {
                isMiss = false;
            }
            if (target.evasionChance <= UnityEngine.Random.Range(0, 100))
            {
                isEvasion = false;
            }
            else
            {
                isEvasion = true;
            }
        }
        else
        {
            isMiss = false;
        }
        specialEffect(target, attacker);
        value += attacker.plusDamage + attacker.oneTimePlusDamage + attacker.difficultDamageMultiplier;
        if (!isMiss && !isEvasion)
        {
            actionSound.Play();
            if (UnityEngine.Random.Range(0, 100) <= attacker.passiveCriticalChance + criticalChance && attacker.passiveCriticalDamage + criticalDamage >= 1)
            {
                value = Convert.ToInt32(Math.Round(value * (attacker.passiveCriticalDamage + criticalDamage), MidpointRounding.AwayFromZero));
                GameManager.critSound.Play();
                isCrit = true;
            }
            else
            {
                isCrit = false;
            }
            if (value < 0)
            {
                value = 0;
            }
        }
        else
        {
            GameManager.missSound.Play();
        }
        target.GetHit(value, isCrit, isMiss, isEvasion, isThroughArmor);
        attacker.oneTimePlusDamage = 0;

    }
}
