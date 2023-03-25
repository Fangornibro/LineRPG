using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flash : Attack
{
    protected override void specialEffect(Character target, Character attacker)
    {
        if (!isMiss)
        {
            target.plusMissChance += 25;
            target.EffectUpdate();
        }
    }
}
