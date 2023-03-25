using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : Attack
{
    protected override void specialEffect(Character target, Character attacker)
    {
        if (!isMiss && target.poison < 5)
        {
            target.poison++;
            target.EffectUpdate();
        }
    }
}
