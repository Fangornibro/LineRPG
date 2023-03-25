using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disarm : Attack
{
    protected override void specialEffect(Character target, Character attacker)
    {
        if (!isMiss)
        {
            try
            {
                Enemy enemy = (Enemy)target;
                try
                {
                    if ((DefaultAttack)enemy.nextAttack.action)
                    {
                        int rand = Random.Range(0, 2);
                        if (rand == 0)
                        {
                            enemy.nextAttack = enemy.noneAttack;
                            target.transform.Find("AttackIcon").GetComponent<SpriteRenderer>().sprite = enemy.noneAttack.attackIcon;
                            target.EffectUpdate();
                        }
                    }
                }
                catch { }
            }
            catch
            {
                Player player = (Player)target;
                player.isDisarm = true;
            }
        }
    }
}
