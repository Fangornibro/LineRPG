using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlusDamage : Buff
{
    protected override void specialEffect(Character target, Character attacker)
    {
        target.plusDamage += value;
        target.EffectUpdate();
    }
}
