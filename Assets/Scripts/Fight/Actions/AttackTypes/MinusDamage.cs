using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinusDamage : Attack
{
    protected override void specialEffect(Character target, Character attacker)
    {
        if (!isMiss)
        {
            target.minusDamage++;
            target.EffectUpdate();
        }
    }
}
