using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Animation
    private bool gotHit, attack;
    private SpriteRenderer sr;
    private Animator anim;
    private float hitDuration = 0.4f, attackDuration = 0.5f;


    [HideInInspector] public int gold;
    [HideInInspector] public int curMana, maxMana = 3, HP, maxHP = 30, armor;
    [HideInInspector] public float passiveCriticalChance, passiveCriticalDamage;


    [Header("Damage popup")]
    [SerializeField] private DamagePopup textPrefab;
    private DamagePopup damagePopup;


    [Space]
    [Space]
    [Header("Effects")]
    public List<GameObject> effectIconPrefabs = new List<GameObject>();
    [HideInInspector] public List<GameObject> curEffectIcons = new List<GameObject>();
    [HideInInspector] public int poison = 0, maxPassiveDamage = 0, passiveDamage = 0, passiveMana = 0, weakness = 0, passiveArmor = 0;
    private int minusManaEffect = 0;


    [Space]
    [Space]
    [Header("Initialisations")]
    [SerializeField] private Transform temp;
    [SerializeField] private EventHUD eh;
    [SerializeField] private TopPanels topPanels;
    [SerializeField] private AbilityOnCursor abilityOnCursor;


    [Space]
    [Space]
    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI armorText;
    [SerializeField] private TextMeshProUGUI goldText;


    [Space]
    [Space]
    [Header("Sounds")]
    [SerializeField] private AudioSource armorUpSound;
    [SerializeField] private AudioSource poisonSound;
    [SerializeField] private AudioSource critSound;


    [Space]
    [Space]
    [Header("Only in editor")]
    [SerializeField] private PlayerSelection defPlayer;
    public void Create(float damage, bool isHeal, bool isCrit)
    {
        damagePopup = Instantiate(textPrefab, new Vector2(transform.position.x + UnityEngine.Random.Range(0, transform.localScale.x), transform.position.y + transform.localScale.y), Quaternion.identity);
        TextMeshPro text = damagePopup.GetComponent<TextMeshPro>();
        text.SetText(damage + (isCrit ? " crit" : ""));
        damagePopup.transform.localScale = new Vector2(damagePopup.transform.localScale.x + (damage / 100) * (isCrit ? 1.5f : 1), damagePopup.transform.localScale.y + damage / 100);
        if (damagePopup.transform.localScale.x >= 2)
        {
            damagePopup.transform.localScale = new Vector2(2, 2);
        }
        if (isHeal)
        {
            damagePopup.textColor = new Color(0.05513528f, 0.5566038f, 0.1028308f, 1f);
        }
        else
        {
            damagePopup.textColor = new Color(0.682353f, (0.1215686f + damage / 122) + (isCrit ? 0.2f : 0f), 0.2078431f, 1f);
        }
    }
#if (UNITY_EDITOR)
    private void Awake()
    {
        if (SelectCharacterButton.curPlayer == null)
        {
            SelectCharacterButton.curPlayer = defPlayer;
        }
    }
#endif

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        PlayerSelection p = SelectCharacterButton.curPlayer;
        maxHP = p.maxHP;
        maxMana = p.maxMana;
        passiveCriticalChance = p.passiveCriticalChance;
        passiveCriticalDamage = p.passiveCriticalDamage;
        sr.sprite = p.GetComponent<SpriteRenderer>().sprite;
        anim.runtimeAnimatorController = p.animatorController;
        HP = maxHP;
        curMana = maxMana;

        BarsUpdate();
    }
    public void EffectUpdate()
    {
        foreach (GameObject gm in curEffectIcons)
        {
            Destroy(gm.gameObject);
        }
        curEffectIcons.Clear();

        if (passiveMana > 0)
        {
            GameObject effectIcon = Instantiate(effectIconPrefabs[0], transform.position, Quaternion.Euler(0, 0, 0), transform);
            effectIcon.transform.GetChild(0).GetComponent<TextMeshPro>().SetText(passiveMana.ToString());
            curEffectIcons.Add(effectIcon);
        }
        if (passiveArmor > 0)
        {
            GameObject effectIcon = Instantiate(effectIconPrefabs[1], transform.position, Quaternion.Euler(0, 0, 0), transform);
            effectIcon.transform.GetChild(0).GetComponent<TextMeshPro>().SetText(passiveArmor.ToString());
            curEffectIcons.Add(effectIcon);
        }
        if (passiveDamage > 0)
        {
            GameObject effectIcon = Instantiate(effectIconPrefabs[2], transform.position, Quaternion.Euler(0, 0, 0), transform);
            effectIcon.transform.GetChild(0).GetComponent<TextMeshPro>().SetText(passiveDamage.ToString());
            curEffectIcons.Add(effectIcon);
        }
        if (poison > 0)
        {
            GameObject effectIcon = Instantiate(effectIconPrefabs[6], transform.position, Quaternion.Euler(0, 0, 0), transform);
            effectIcon.transform.GetChild(0).GetComponent<TextMeshPro>().SetText(poison.ToString() + "t");
            curEffectIcons.Add(effectIcon);
        }
        if (minusManaEffect > 0)
        {
            GameObject effectIcon = Instantiate(effectIconPrefabs[3], transform.position, Quaternion.Euler(0, 0, 0), transform);
            effectIcon.transform.GetChild(0).GetComponent<TextMeshPro>().SetText(minusManaEffect.ToString());
            curEffectIcons.Add(effectIcon);
        }
        if (weakness > 0)
        {
            GameObject effectIcon = Instantiate(effectIconPrefabs[5], transform.position, Quaternion.Euler(0, 0, 0), transform);
            effectIcon.transform.GetChild(0).GetComponent<TextMeshPro>().SetText(weakness.ToString());
            curEffectIcons.Add(effectIcon);
        }
        for (int i = 0; i < curEffectIcons.Count; i++)
        {
            if ((i + 4) % 3 == 1)
            {
                curEffectIcons[i].transform.position = transform.position + new Vector3(-3, (i - 1), 0);
            }
            else if ((i + 4) % 3 == 2)
            {
                curEffectIcons[i].transform.position = transform.position + new Vector3(0, (i - 2), 0);
            }
            else if((i + 4) % 3 == 0)
            {
                curEffectIcons[i].transform.position = transform.position + new Vector3(3, (i - 3), 0);
            }
        }
    }
    public void NewTurn()
    {
        minusManaEffect = 0;
        weakness = 0;
        if (poison > 0)
        {
            poisonSound.Play();
            GetHit(3 * poison, false, EnemyAttack.Effect.throughArmor, "poison");
            poison--;
        }
        curMana = maxMana + passiveMana;
        if (passiveArmor > 0)
        {
            armor += passiveArmor;
            armorUpSound.Play();
        }
        passiveDamage = maxPassiveDamage;
        BarsUpdate();
        EffectUpdate();
    }
    public void FightStart()
    {
        weakness = 0;
        poison = 0;
        minusManaEffect = 0;
        curMana = maxMana + passiveMana;
        armor = 0;
        passiveDamage = maxPassiveDamage;
        BarsUpdate();
        EffectUpdate();
    }
    public void BarsUpdate()
    {
        armorText.SetText(armor.ToString());
        topPanels.UpdateMana((maxMana + passiveMana), curMana);
        topPanels.UpdateHP(maxHP, 20, HP);
    }
    public void Hit(Enemy enemy)
    {
        if (abilityOnCursor.isOnCursor && (abilityOnCursor.abilityType == Icon.Type.attack || abilityOnCursor.abilityType == Icon.Type.magicAttack))
        {
            if (curMana >= abilityOnCursor.curCost)
            {
                int damage;
                bool isCrit;
                if (UnityEngine.Random.Range(0, 100) >= passiveCriticalChance + abilityOnCursor.curCriticalChance)
                {
                    damage = abilityOnCursor.curDamageOrArmour - weakness;
                    isCrit = false;
                }
                else
                {
                    damage = Convert.ToInt32(Math.Round((abilityOnCursor.curDamageOrArmour - weakness) * (passiveCriticalDamage + abilityOnCursor.curCriticalDamage), MidpointRounding.AwayFromZero));
                    isCrit = true;
                    critSound.Play();
                }
                //Sounds
                abilityOnCursor.abilitySound.Play();
                //Player animation 
                StartAttackAnimation();
                //Minus mana
                MinusMana(abilityOnCursor.curCost);
                //Lifesteal effect
                if (abilityOnCursor.effect == Icon.Effect.HPSteal)
                {
                    GetHeal(damage / 4);
                }
                //Poison effect
                if (abilityOnCursor.effect == Icon.Effect.poison)
                {
                    enemy.poison++;
                    enemy.EffectUpdate();
                }
                //Disarm effect
                if (abilityOnCursor.effect == Icon.Effect.disarm && enemy.nextAttack.effect == EnemyAttack.Effect.defaultAttack)
                {
                    int rand = UnityEngine.Random.Range(0, 2);
                    if (rand == 0)
                    {
                        enemy.nextAttack = enemy.noneAttack;
                        enemy.transform.Find("AttackIcon").GetComponent<SpriteRenderer>().sprite = enemy.noneAttack.attackIcon;
                        enemy.EffectUpdate();
                    }
                }
                //AOE effect
                if (abilityOnCursor.effect == Icon.Effect.AOE)
                {
                    for (int j = 0; j < temp.childCount; j++)
                    {
                        Enemy curEnemyForAOE = temp.GetChild(j).GetComponent<Enemy>();
                        curEnemyForAOE.GetHit(damage, isCrit);
                    }
                }
                else
                {
                    if (abilityOnCursor.effect == Icon.Effect.armorDestruction && enemy.armor > 0)
                    {
                        enemy.GetHit(damage * 2, isCrit);
                    }
                    else
                    {
                        enemy.GetHit(damage, isCrit);
                    }
                }
                EffectUpdate();
            }
        }
    }


    public void GetHit(int damage, bool isCrit, EnemyAttack.Effect effect, string enemyName)
    {
        int damageRand;
        if (damage / 10 == 0)
        {
            damageRand = UnityEngine.Random.Range(-1, 2);
        }
        else
        {
            damageRand = UnityEngine.Random.Range(-damage / 10, (damage / 10) + 1);
        }
        damage += damageRand;
        //Popup
        Create(damage, false, isCrit);
        if (effect == EnemyAttack.Effect.minusMana)
        {
            minusManaEffect++;
            MinusMana(1);
            EffectUpdate();
        }
        if (effect == EnemyAttack.Effect.poison)
        {
            poison++;
            EffectUpdate();
        }
        if (effect == EnemyAttack.Effect.weakness)
        {
            weakness++;
            EffectUpdate();
        }
        if (effect == EnemyAttack.Effect.throughArmor)
        {
            HP -= damage;
        }
        else
        {
            armor -= damage;
            if (armor < 0)
            {
                HP += armor;
                armor = 0;
            }
        }
        if (HP <= 0)
        {
            HP = 0;
            //Back to map
            eh.Activation("Defeat");
            eh.eventHudText.SetText("You have been killed by " + enemyName + ".");
        }
        BarsUpdate();
        anim.SetBool("GotHit", true);
        sr.color = new Color(0.75f, 0.25f, 0.25f, 1f);
        gotHit = true;
    }
    public void GetHeal(int heal)
    {
        //Popup
        Create(heal, true, false);
        HP += heal;
        if (HP >= maxHP)
        {
            HP = maxHP;
        }
        BarsUpdate();
    }
    private void Update()
    {
        if (gotHit)
        {
            hitDuration -= Time.deltaTime;
        }
        else
        {
            anim.SetBool("GotHit", false);
            sr.color = new Color(1f, 1f, 1f);
        }
        if (hitDuration <= 0)
        {
            gotHit = false;
            hitDuration = 0.4f;
        }
        if (attack)
        {
            attackDuration -= Time.deltaTime;
            if (attackDuration <= 0)
            {
                attack = false;
                anim.SetBool("Attack", false);
                attackDuration = 0.5f;
            }
        }
    }
    public void AddGold(int Gold)
    {
        gold += Gold;
        if (gold <= 0)
        {
            gold = 0;
        }
        goldText.SetText(gold.ToString());
    }
    public void MinusMana(int cost)
    {
        curMana -= cost;
        if (curMana < 0)
        {
            curMana = 0;
        }
        BarsUpdate();
    }
    public void SetArmor(int armour)
    {
        this.armor = armour;
        armorText.SetText(armour.ToString());
    }

    private void OnMouseDown()
    {
        if (abilityOnCursor.isOnCursor && abilityOnCursor.abilityType == Icon.Type.block)
        {
            if (curMana >= abilityOnCursor.curCost)
            {
                //Sounds
                abilityOnCursor.abilitySound.Play();
                MinusMana(abilityOnCursor.curCost);

                armor += abilityOnCursor.curDamageOrArmour;
                BarsUpdate();
            }
        }
    }
    public void StartAttackAnimation()
    {
        anim.SetBool("Attack", true);
        attack = true;
    }
}
