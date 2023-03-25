using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public int HP, maxHP, armor;
    [HideInInspector] public int poison, minusDamage, plusDamage, oneTimePlusDamage, difficultDamageMultiplier;
    public float passiveCriticalDamage, passiveCriticalChance, passiveMissChance, evasionChance;
    [HideInInspector] public float plusMissChance;
    public abstract void GetHit(int damage, bool isCrit, bool isMiss, bool isEvasion, bool isThroughArmor);
    public abstract void BarsUpdate();
    public void GetHeal(int health)
    {
        Create(health, true, false, false, false);
        HP += health;
        if (HP >= maxHP)
        {
            HP = maxHP;
        }
        BarsUpdate();
    }
    public void AddArmor(int armor)
    {
        this.armor += armor;
        BarsUpdate();
    }
    public abstract void EffectUpdate();

    [Header("Damage popup")]
    [SerializeField] private DamagePopup damagePopupPrefab;
    private DamagePopup damagePopup;

    public void Create(float damage, bool isHeal, bool isCrit, bool isMiss, bool isEvasion)
    {
        damagePopup = Instantiate(damagePopupPrefab, new Vector2(transform.position.x + Random.Range(0, transform.localScale.x / 2), transform.position.y + transform.localScale.y), Quaternion.identity);
        if (isMiss)
        {
            damagePopup.GetComponent<TextMeshPro>().SetText("Miss");
            damagePopup.transform.localScale = new Vector2(damagePopup.transform.localScale.x + 5 / 100, damagePopup.transform.localScale.y + 5 / 100);
            damagePopup.textColor = new Color(1f, 0.9030769f, 0.25f, 1f);
        }
        else if (isEvasion)
        {
            damagePopup.GetComponent<TextMeshPro>().SetText("Evade");
            damagePopup.transform.localScale = new Vector2(damagePopup.transform.localScale.x + 5 / 100, damagePopup.transform.localScale.y + 5 / 100);
            damagePopup.textColor = new Color(1f, 0.9030769f, 0.25f, 1f);
        }
        else
        {
            damagePopup.GetComponent<TextMeshPro>().SetText(damage + (isCrit ? " crit" : ""));
            damagePopup.transform.localScale = new Vector2(damagePopup.transform.localScale.x + (damage / 100) * (isCrit ? 1.5f : 1), damagePopup.transform.localScale.y + damage / 100);
            if (damagePopup.transform.localScale.x >= 2)
            {
                damagePopup.transform.localScale = new Vector2(2, 2);
            }
            damagePopup.textColor = new Color(0.682353f, 0.1215686f + (damage / 122) + (isCrit ? 0.2f : 0f), 0.2078431f, 1f);
            if (isHeal)
            {
                damagePopup.textColor = new Color(0.05513528f, 0.5566038f, 0.1028308f, 1f);
            }
            else
            {
                damagePopup.textColor = new Color(0.682353f, (0.1215686f + damage / 122) + (isCrit ? 0.2f : 0f), 0.2078431f, 1f);
            }
        }

    }
}
