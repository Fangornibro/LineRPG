using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Enemy : MonoBehaviour
{
    private SpriteRenderer sr;
    //Stats
    public int HP, maxHP, armor, gold;
    public int plusDamage = 0;
    private float HPSizeMultiple;
    //AllAttacks
    public List<EnemyAttack> attacks;
    [HideInInspector]
    public EnemyAttack nextAttack;
    public EnemyAttack noneAttack;
    //Hp bar
    private SpriteRenderer hpBar;
    //Death
    public Material deathMat;
    private float deathTime = 0;
    [HideInInspector]
    public bool death = false;
    private Animator anim;
    private float hitDuration = 0.7f, attackDuration = 0.5f;
    private bool gotHit = false, attack = false;
    //Shaking
    private Vector3 defaultPos;
    private float Shakingx, Shakingy;
    private float timeBtwShaking = 0.1f;
    //Player
    private Player player;
    //Patricles
    public ParticleSystem particleSystemPrefab;
    //Damage popup
    [SerializeField]
    private DamagePopup textPrefab;
    private DamagePopup damagePopup;
    //Armor
    TextMeshPro armorText;
    SpriteRenderer armorGO;
    //Name
    public string Name;
    //Temp
    private Transform temp;
    //Have skin?
    public bool isAnySkins;
    //Crit sound
    private AudioSource critSound;
    //Effects
    public List<GameObject> effectIconPrefabs = new List<GameObject>();
    [HideInInspector]
    public List<GameObject> curEffectIcons = new List<GameObject>();
    [HideInInspector]
    public int poison = 0;
    //Fight manager
    private FightManager fm;
    public void Create(float damage, bool isCrit)
    {
        damagePopup = GameObject.Instantiate(textPrefab, new Vector2(transform.position.x + UnityEngine.Random.Range(0, transform.localScale.x), transform.position.y + transform.localScale.y), Quaternion.identity);
        TextMeshPro text = damagePopup.GetComponent<TextMeshPro>();
        text.SetText(damage.ToString());
        damagePopup.transform.localScale = new Vector2(damagePopup.transform.localScale.x + (damage / 100) * (isCrit ? 1.5f : 1), damagePopup.transform.localScale.y + damage / 100);
        if (damagePopup.transform.localScale.x >= 2)
        {
            damagePopup.transform.localScale = new Vector2(2, 2);
        }
        damagePopup.textColor = new Color(0.682353f, 0.1215686f + (damage / 122) + (isCrit ? 0.2f : 0f), 0.2078431f, 1f);
    }
    void Start()
    {
        //Crit sound
        critSound = GameObject.Find("CritSound").GetComponent<AudioSource>();
        //Temp
        temp = GameObject.Find("Temp").transform;
        //Armor
        armorText = transform.Find("ArmorText").GetComponent<TextMeshPro>();
        armorGO = transform.Find("Armor").GetComponent<SpriteRenderer>();
        if (armor <= 0)
        {
            armorText.text = "";
            armorGO.color = new Color(1f, 1f, 1f, 0f);
        }
        else
        {
            armorText.text = armor.ToString();
        }
        //HP bar
        hpBar = transform.Find("HpBarFront").GetComponent<SpriteRenderer>();
        HPSizeMultiple = hpBar.size.x / HP;
        //Player
        player = GameObject.Find("Player").GetComponent<Player>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        defaultPos = transform.position;
        if (isAnySkins)
        {
            anim.SetInteger("Type", UnityEngine.Random.Range(1, 5));
        }
        //Fight manager
        fm = GameObject.Find("FightManager").GetComponent<FightManager>();
    }

    private void GetArmor(int Armor)
    {
        armor += Armor;
        armorText.SetText(armor.ToString());
        armorGO.color = new Color(1f, 1f, 1f, 1f);
    }

    private void RunningAway()
    {
        death = true;
    }
    public void Hit()
    {
        StartAttackAnimation();
        if (nextAttack.attackSound != null)
        {
            AudioSource sound = Instantiate(nextAttack.attackSound, new Vector2(0, 0), Quaternion.Euler(0, 0, 0), transform);
            sound.Play();
            if (nextAttack.effect == EnemyAttack.Effect.armorUp)
            {
                GetArmor(nextAttack.damage);
            }
            else if (nextAttack.effect == EnemyAttack.Effect.runningAway)
            {
                gold = 0;
                RunningAway();
            }
            else if (nextAttack.effect == EnemyAttack.Effect.flock)
            {
                for (int j = 0; j < temp.childCount; j++)
                {
                    Enemy curEnemyForFlock = temp.GetChild(j).GetComponent<Enemy>();
                    curEnemyForFlock.plusDamage++;
                    curEnemyForFlock.EffectUpdate();
                }
            }
            else
            {
                int damage;
                bool isCrit;
                if (UnityEngine.Random.Range(0, 100) >= nextAttack.criticalChance)
                {
                    damage = nextAttack.damage + plusDamage;
                    isCrit = false;
                }
                else
                {
                    damage = Convert.ToInt32(Math.Round((nextAttack.damage + plusDamage) * nextAttack.criticalDamage, MidpointRounding.AwayFromZero));
                    isCrit = true;
                    critSound.Play();
                }
                player.GetHit(damage, isCrit, nextAttack.effect, Name);
            }
        }
    }
    //Get hit
    private void OnMouseDown()
    {
        if (!death)
        {
            player.Hit(this);
        }
    }
    public void GetHit(int damage, bool isCrit)
    {
        //Player set passive damage to 0
        player.passiveDamage = 0;
        //Particles
        Instantiate(particleSystemPrefab, new Vector2(transform.position.x, transform.position.y + transform.localScale.y / 2), Quaternion.Euler(0, 0, 0));
        //Popup
        Create(damage, isCrit);
        //Minus armor and HP
        armor -= damage;
        if (armor < 0)
        {
            HP += armor;
            armor = 0;
        }
        if (HP <= 0)
        {
            HP = 0;
            death = true;
        }
        if (armor <= 0)
        {
            armorText.text = "";
            armorGO.color = new Color(1f, 1f, 1f, 0f);
        }
        else
        {
            armorGO.color = new Color(1f, 1f, 1f, 1f);
            armorText.text = armor.ToString();
        }
        hpBar.size = new Vector2(HPSizeMultiple * HP, hpBar.size.y);
        //Enemy animation
        anim.SetBool("GotHit", true);
        sr.color = new Color(0.75f, 0.25f, 0.25f);
        gotHit = true;
    }
    public void EffectUpdate()
    {
        foreach (GameObject gm in curEffectIcons)
        {
            GameObject.Destroy(gm.gameObject);
        }
        curEffectIcons.Clear();

        if (plusDamage > 0)
        {
            GameObject effectIcon = Instantiate(effectIconPrefabs[0], transform.position, Quaternion.Euler(0, 0, 0), transform);
            effectIcon.transform.GetChild(0).GetComponent<TextMeshPro>().SetText(plusDamage.ToString());
            curEffectIcons.Add(effectIcon);
        }
        if (poison > 0)
        {
            GameObject effectIcon = Instantiate(effectIconPrefabs[1], transform.position, Quaternion.Euler(0, 0, 0), transform);
            effectIcon.transform.GetChild(0).GetComponent<TextMeshPro>().SetText(poison.ToString() + "t");
            curEffectIcons.Add(effectIcon);
        }
        if (nextAttack == noneAttack)
        {
            GameObject effectIcon = Instantiate(effectIconPrefabs[2], transform.position, Quaternion.Euler(0, 0, 0), transform);
            effectIcon.transform.GetChild(0).GetComponent<TextMeshPro>().SetText("");
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
            else if ((i + 4) % 3 == 0)
            {
                curEffectIcons[i].transform.position = transform.position + new Vector3(3, (i - 3), 0);
            }
        }
    }
    public void StartAttackAnimation()
    {
        anim.SetBool("Attack", true);
        attack = true;
    }

    private void Update()
    {
        if (death)
        {
            deathTime += Time.deltaTime;
            sr.material = deathMat;
            sr.material.SetFloat("_OutlineWidth", deathTime);
            if (deathTime >= 1)
            {
                fm.AddGold(gold);
                GameObject.Destroy(gameObject);
            }
        }
        else
        {
            if (gotHit)
            {
                hitDuration -= Time.deltaTime;
                if (timeBtwShaking <= 0)
                {
                    Shakingx = UnityEngine.Random.Range(-1f, 1f);
                    Shakingy = UnityEngine.Random.Range(-1f, 1f);
                    timeBtwShaking = 0.1f;
                }
                else
                {
                    timeBtwShaking -= Time.deltaTime;
                }
                transform.position = defaultPos + new Vector3(Shakingx, Shakingy, 0);
            }
            else
            {
                transform.position = defaultPos;
                anim.SetBool("GotHit", false);
                sr.color = new Color(1f, 1f, 1f);
            }
            if (hitDuration <= 0)
            {
                gotHit = false;
                hitDuration = 0.7f;
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
    }
}