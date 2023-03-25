using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : Character
{
    //Animation
    private bool gotHit, attack;
    [HideInInspector] public bool isDisarm = false;
    private SpriteRenderer sr;
    private Animator anim;
    private float hitDuration = 0.4f, attackDuration = 0.5f;


    [HideInInspector] public int gold;
    [HideInInspector] public int curMana, maxMana;


    [Space]
    [Space]
    [Header("Effects")]
    public List<GameObject> effectIconPrefabs;
    [HideInInspector] public List<GameObject> curEffectIcons = new List<GameObject>();
    [HideInInspector] public int maxPassiveDamage = 0, passiveMana = 0, passiveArmor = 0, minusManaEffect = 0;


    [Space]
    [Space]
    [Header("Initialisations")]
    [SerializeField] private Transform temp;
    [SerializeField] private EventHUD eh;
    [SerializeField] private TopPanels topPanels;
    [SerializeField] private AbilityOnCursor abilityOnCursor;
    [SerializeField] private GameManager gameManager;


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
        passiveMissChance = p.passiveMissChance;
        evasionChance = p.evasionChance;
        sr.sprite = p.GetComponent<SpriteRenderer>().sprite;
        anim.runtimeAnimatorController = p.animatorController;
        HP = maxHP;
        curMana = maxMana;

        BarsUpdate();
    }
    public override void EffectUpdate()
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
        if (plusDamage + oneTimePlusDamage > 0)
        {
            GameObject effectIcon = Instantiate(effectIconPrefabs[2], transform.position, Quaternion.Euler(0, 0, 0), transform);
            effectIcon.transform.GetChild(0).GetComponent<TextMeshPro>().SetText((plusDamage + oneTimePlusDamage).ToString());
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
        if (minusDamage > 0)
        {
            GameObject effectIcon = Instantiate(effectIconPrefabs[5], transform.position, Quaternion.Euler(0, 0, 0), transform);
            effectIcon.transform.GetChild(0).GetComponent<TextMeshPro>().SetText(minusDamage.ToString());
            curEffectIcons.Add(effectIcon);
        }
        if (plusMissChance > 0)
        {
            GameObject effectIcon = Instantiate(effectIconPrefabs[8], Vector3.zero, Quaternion.Euler(0, 0, 0), transform);
            effectIcon.transform.GetChild(0).GetComponent<TextMeshPro>().SetText("");
            curEffectIcons.Add(effectIcon);
        }
        if (isDisarm)
        {
            GameObject effectIcon = Instantiate(effectIconPrefabs[4], Vector3.zero, Quaternion.Euler(0, 0, 0), transform);
            effectIcon.transform.GetChild(0).GetComponent<TextMeshPro>().SetText("");
            curEffectIcons.Add(effectIcon);
        }
        for (int i = 0; i < curEffectIcons.Count; i++)
        {
            if ((i + 4) % 3 == 1)
            {
                curEffectIcons[i].transform.position = transform.position + new Vector3(-1, i, 0);
            }
            else if ((i + 4) % 3 == 2)
            {
                curEffectIcons[i].transform.position = transform.position + new Vector3(0, i - 1, 0);
            }
            else if((i + 4) % 3 == 0)
            {
                curEffectIcons[i].transform.position = transform.position + new Vector3(1, i - 2, 0);
            }
        }
    }
    public void NewTurn()
    {
        isDisarm = false;
        minusManaEffect = 0;
        minusDamage = 0;
        if (poison > 0)
        {
            poisonSound.Play();
            GetHit(2 * poison, false, false, false, true);
            poison--;
        }
        curMana = maxMana + passiveMana;
        if (passiveArmor > 0)
        {
            armor += passiveArmor;
            armorUpSound.Play();
        }
        oneTimePlusDamage = maxPassiveDamage;
        BarsUpdate();
        EffectUpdate();
    }
    public void FightStart()
    {
        minusDamage = 0;
        poison = 0;
        minusManaEffect = 0;
        curMana = maxMana + passiveMana;
        armor = 0;
        oneTimePlusDamage = maxPassiveDamage;
        BarsUpdate();
        EffectUpdate();
    }
    public override void BarsUpdate()
    {
        armorText.SetText(armor.ToString());
        topPanels.UpdateMana((maxMana + passiveMana), curMana);
        topPanels.UpdateHP(maxHP, HP);
    }
    public void Hit(Character enemy)
    {
        if (abilityOnCursor.isOnCursor && (abilityOnCursor.abilityType == Item.Type.attack))
        {
            if (curMana >= abilityOnCursor.curCost)
            {
                //Player animation 
                StartAttackAnimation();
                //Minus mana
                MinusMana(abilityOnCursor.curCost);
                abilityOnCursor.action.MakeAnAction(enemy, this);
                EffectUpdate();
            }
        }
    }


    public override void GetHit(int damage, bool isCrit, bool isMiss, bool isEvasion, bool isThroughArmor)
    {
        Create(damage, false, isCrit, isMiss, isEvasion);
        if (!isMiss && !isEvasion)
        {
            if (isThroughArmor)
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
                
            //}
            if (HP <= 0)
            {
                HP = 0;
                gameManager.EventHUDVisible(EventHUD.statement.Defeat);
                eh.eventHudText.SetText("You have been killed by " + gameManager.map.squad.squadName + " squad.");
            }
            BarsUpdate();
            anim.SetBool("GotHit", true);
            sr.color = new Color(0.75f, 0.25f, 0.25f, 1f);
            gotHit = true;
            EffectUpdate();
        }
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
    public void StartAttackAnimation()
    {
        anim.SetBool("Attack", true);
        attack = true;
    }
    private void OnMouseDown()
    {
        if (abilityOnCursor.isOnCursor && abilityOnCursor.abilityType == Item.Type.Buff)
        {
            if (curMana >= abilityOnCursor.curCost)
            {
                abilityOnCursor.action.MakeAnAction(null, this);
                MinusMana(abilityOnCursor.curCost);
                BarsUpdate();
            }
        }
    }
}
