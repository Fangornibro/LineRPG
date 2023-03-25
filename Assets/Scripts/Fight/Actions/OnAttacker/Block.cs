using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : Buff
{
    protected override void specialEffect(Character target, Character attacker)
    {
        target.AddArmor(value);
    }
}
