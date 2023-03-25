using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThroughArmor : Attack
{
    protected override void specialEffect(Character target, Character attacker)
    {
        isThroughArmor = true;
    }
}
