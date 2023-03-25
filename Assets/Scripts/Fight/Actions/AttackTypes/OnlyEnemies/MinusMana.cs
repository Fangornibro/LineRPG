using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinusMana : Attack
{
    protected override void specialEffect(Character target, Character attacker)
    {
        if (!isMiss)
        {
            try{
                Player player = (Player)target;
                player.minusManaEffect++;
                player.MinusMana(1);
                target.EffectUpdate();
            }
            catch { }
        }
    }
}
