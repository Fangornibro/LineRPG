using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPSteal : Attack
{
    protected override void specialEffect(Character target, Character attacker)
    {
        if (!isMiss)
        {
            attacker.GetHeal(value / 4);
        }
    }
}
