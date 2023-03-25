using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public abstract class Buff : Action
{
    [HideInInspector] protected bool isMiss, isCrit, isThroughArmor = false;

    protected abstract void specialEffect(Character target, Character attacker);
    protected override void makeAnAction(Character target, Character attacker)
    {
        actionSound.Play();
        specialEffect(target, attacker);
    }
}
