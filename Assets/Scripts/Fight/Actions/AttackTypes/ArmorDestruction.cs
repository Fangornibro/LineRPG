using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorDestruction : Attack
{
    protected override void specialEffect(Character target, Character attacker)
    {
        if (target.armor > 0)
        {
            value *= 2;
        }
    }
}
