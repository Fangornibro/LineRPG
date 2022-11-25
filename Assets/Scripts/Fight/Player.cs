using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static EnemyAttack;
using static UnityEditorInternal.ReorderableList;

public class Player : MonoBehaviour
{
    public int curMana, maxMana;
    public float HP, maxHP, armor;
    [HideInInspector]
    public float passiveArmor = 0;
    [HideInInspector]
    public int maxPassiveDamage = 0, passiveDamage = 0, passiveMana = 0;
    public int gold;
    private bool gotHit, attack;
    private SpriteRenderer sr;
    private Animator anim;
    private float hitDuration = 0.4f, attackDuration = 0.5f, hpBarHeight, defaultHpBarHeight;
    //Hp visualisation
    private RectTransform hpBar;
    private TextMeshProUGUI HPText, armourText, goldText;

    private FightActivation fightActivation;
    //Damage popup
    [SerializeField]
    private DamagePopup textPrefab;
    private DamagePopup damagePopup;
    //Outline
    public SpriteRenderer outlinePrefab;
    private List<SpriteRenderer> outlineList;
    //Event hud
    EventHud eh;

    AbilityOnCursor abilityOnCursor;
    public void Create(float damage, bool isHeal)
    {
        damagePopup = GameObject.Instantiate(textPrefab, new Vector2(transform.position.x + Random.Range(0, transform.localScale.x), transform.position.y + transform.localScale.y), Quaternion.identity);
        TextMeshPro text = damagePopup.GetComponent<TextMeshPro>();
        text.SetText(damage.ToString());
        damagePopup.transform.localScale = new Vector2(damagePopup.transform.localScale.x + damage / 100, damagePopup.transform.localScale.y + damage / 100);
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
            damagePopup.textColor = new Color(0.682353f, 0.1215686f + damage / 122, 0.2078431f, 1f);
        }
    }
    private void Start()
    {
        //Event hud
        eh = GameObject.Find("EventHud").GetComponent<EventHud>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        fightActivation = GameObject.Find("Fight").GetComponent<FightActivation>();
        hpBar = GameObject.Find("HPBarFront").GetComponent<RectTransform>();
        HPText = GameObject.Find("HPText").GetComponent<TextMeshProUGUI>();
        armourText = GameObject.Find("ArmorText").GetComponent<TextMeshProUGUI>();
        goldText = GameObject.Find("GoldText").GetComponent<TextMeshProUGUI>();
        HP = maxHP;
        HPText.SetText(HP.ToString());
        defaultHpBarHeight = hpBar.sizeDelta.y;

        abilityOnCursor = GameObject.Find("AbilityOnCursor").GetComponent<AbilityOnCursor>();

        //Outline
        outlineList = new List<SpriteRenderer>();
        outlineList.Add(GameObject.Instantiate(outlinePrefab, new Vector3(transform.position.x + 0.62f, transform.position.y, 0), new Quaternion(0, 0, 0, 0).normalized, transform));
        outlineList.Add(GameObject.Instantiate(outlinePrefab, new Vector3(transform.position.x - 0.62f, transform.position.y, 0), new Quaternion(0, 0, 0, 0).normalized, transform));
        outlineList.Add(GameObject.Instantiate(outlinePrefab, new Vector3(transform.position.x, transform.position.y + 0.62f, 0), new Quaternion(0, 0, 0, 0).normalized, transform));
        outlineList.Add(GameObject.Instantiate(outlinePrefab, new Vector3(transform.position.x, transform.position.y - 0.62f, 0), new Quaternion(0, 0, 0, 0).normalized, transform));
        foreach (SpriteRenderer s in outlineList)
        {
            s.gameObject.active = false;
        }
    }
    public void NewTurn()
    {
        hpBarHeight = defaultHpBarHeight / maxHP;
        curMana = maxMana + passiveMana;
        armor += passiveArmor;
        passiveDamage = maxPassiveDamage;
        armourText.SetText(armor.ToString());
        fightActivation.UpdateMana((maxMana + passiveMana), curMana);
    }
    public void GetHit(int damage, Effect effect, string enemyName)
    {
        int damageRand;
        if (damage / 10 == 0)
        {
            damageRand = Random.Range(-1, 2);
        }
        else
        {
            damageRand = Random.Range(-damage / 10, (damage / 10) + 1);
        }
        damage += damageRand;
        //Popup
        Create(damage, false);
        if (effect == Effect.minusMana)
        {
            curMana--;
            if (curMana <= 0)
            {
                curMana = 0;
            }
            fightActivation.UpdateMana((maxMana + passiveMana), curMana);
        }
        if (effect == Effect.throughArmor)
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
            eh.Activation("Loose");
            eh.LooseText.SetText("You have been killed by " + enemyName + ".");
        }
        hpBar.sizeDelta = new Vector2(hpBar.sizeDelta.x, HP * hpBarHeight);
        HPText.SetText(HP.ToString());
        armourText.SetText(armor.ToString());
        anim.SetBool("GotHit", true);
        sr.color = new Color(0.75f, 0.25f, 0.25f, 1f);
        gotHit = true;
    }
    public void GetHeal(int heal)
    {
        //Popup
        Create(heal, true);
        HP += heal;
        if (HP >= maxHP)
        {
            HP = maxHP;
        }
        hpBar.sizeDelta = new Vector2(hpBar.sizeDelta.x, HP * hpBarHeight);
        HPText.SetText(HP.ToString());
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
        foreach (SpriteRenderer s in outlineList)
        {
            s.sprite = sr.sprite;
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
        fightActivation.UpdateMana((maxMana + passiveMana), curMana);
    }
    public void SetArmour(int armour)
    {
        this.armor = armour;
        armourText.SetText(armour.ToString());
    }
    private void OnMouseEnter()
    {
        foreach (SpriteRenderer s in outlineList)
        {
            s.gameObject.active = true;
            s.flipX = sr.flipX;
        }
    }
    private void OnMouseExit()
    {
        foreach (SpriteRenderer s in outlineList)
        {
            s.gameObject.active = false;
        }
    }
    private void OnMouseDown()
    {
        if (abilityOnCursor.isOnCursor && abilityOnCursor.abilityType == "block")
        {
            if (curMana >= abilityOnCursor.curCost)
            {
                //Sounds
                abilityOnCursor.abilitySound.Play();
                MinusMana(abilityOnCursor.curCost);

                armor += abilityOnCursor.curDamageOrArmour;
                armourText.SetText(armor.ToString());
                hpBar.sizeDelta = new Vector2(hpBar.sizeDelta.x, HP * hpBarHeight);
                HPText.SetText(HP.ToString());
            }
        }
    }
    public void StartAttackAnimation()
    {
        anim.SetBool("Attack", true);
        attack = true;
    }
}
