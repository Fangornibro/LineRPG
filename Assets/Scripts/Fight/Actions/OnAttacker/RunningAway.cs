using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningAway : Buff
{
    protected override void specialEffect(Character target, Character attacker)
    {
        try
        {
            Enemy enemy = (Enemy)attacker;
            enemy.runningAway = true;
        }
        catch
        {

        }
    }
}
