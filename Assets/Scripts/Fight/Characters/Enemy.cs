using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Enemy : Character
{
    [Header("Stats")]
    //Stats
    public string Name;
    public int gold;
    private int startHP;
    private float HPSizeMultiple;


    [Space]
    [Space]
    [Header("Attacks")]
    public List<EnemyAttack> attacks;
    [HideInInspector] public EnemyAttack nextAttack;
    public EnemyAttack noneAttack;


    [Space]
    [Space]
    [Header("Effects")]
    public List<GameObject> effectIconPrefabs = new List<GameObject>();
    [HideInInspector] public List<GameObject> curEffectIcons = new List<GameObject>();
    [SerializeField] private List<Enemy> divideEnemies;



    [Space]
    [Space]
    [Header("Initialisations")]
    private SpriteRenderer sr;
    private Animator anim;
    [SerializeField] private Transform effectPos;
    [SerializeField] private SpriteRenderer hpBar;
    [SerializeField] private Material deathMat;
    [SerializeField] private ParticleSystem particleSystemPrefab;
    [SerializeField] private DamagePopup textPrefab;
    [SerializeField] private TextMeshPro armorText, hpText;
    [SerializeField] private GameObject armorGO;
    private GameManager gameManager;
    private AudioSource curAttackSound;


    //Death
    private float deathTime = 0;
    [HideInInspector] public bool death = false, runningAway = false;
    private float attackDuration = 0.5f;
    private bool attack = false;

    //Shaking
    private float Shakingx, Shakingy;

    //Start initialisations
    private Player player;
    private FightManager fightManager;

    //Cell
    [HideInInspector] public Transform cell;

    //Stages
    private bool hasStages;

    //Effects
    [HideInInspector] public bool isTerrifying = false;
    
    void Awake()
    {
        //Initialisations
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        player = GameManager.player;
        fightManager = gameManager.fightManager;

        //HP bar
        startHP = maxHP;
        AddHPForDifficult();

        //Skins
        if (HasParameter("Type", anim))
        {
            anim.SetInteger("Type", UnityEngine.Random.Range(1, 5));
        }
        hasStages = HasParameter("Stage", anim);
        EffectUpdate();
        BarsUpdate();
    }

    public void AddHPForDifficult()
    {
        maxHP += Mathf.RoundToInt(startHP * 0.2f * fightManager.globalDifficult);
        HP += Mathf.RoundToInt(startHP * 0.2f * fightManager.globalDifficult);
        HPSizeMultiple = hpBar.size.x / maxHP;
        BarsUpdate();
    }

    public void Hit(int difficult)
    {
        difficultDamageMultiplier = Mathf.RoundToInt(nextAttack.damage * 0.2f * difficult);
        StartAttackAnimation();
        if (curAttackSound != null)
        {
            Destroy(curAttackSound.gameObject);
        }
        if (nextAttack.attackSound != null)
        {
            curAttackSound = Instantiate(nextAttack.attackSound, Vector2.zero, Quaternion.identity, transform);
        }
        else
        {
            curAttackSound= null;
        }
        nextAttack.SetValues(curAttackSound);
        nextAttack.action.MakeAnAction(player, this);
        difficultDamageMultiplier = 0;
    }

    private void OnMouseDown()
    {
        if (!death && !player.isDisarm)
        {
            player.Hit(this);
        }
    }
    private Vector2 deffaultPosition;
    public override void GetHit(int damage, bool isCrit, bool isMiss, bool isEvasion, bool isThroughArmor)
    {
        Create(damage, false, isCrit, isMiss, isEvasion);
        if (!isMiss && !isEvasion)
        {
            if (deffaultPosition != Vector2.zero)
            {
                transform.position = deffaultPosition;
            }
            //Particles
            Instantiate(particleSystemPrefab, new Vector2(transform.position.x, transform.position.y + transform.localScale.y / 2), Quaternion.Euler(0, 0, 0), transform);
            //Minus armor and HP
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
            if (HP <= 0)
            {
                HP = 0;
                if (divideEnemies.Count != 0)
                {
                    anim.SetBool("Death", true);
                }
                death = true;
            }
            if (hasStages)
            {
                if (HP >= maxHP * 2 / 3)
                {
                    anim.SetInteger("Stage", 1);
                }
                else if (HP < maxHP * 2 / 3 && HP >= maxHP * 1 / 3)
                {
                    anim.SetInteger("Stage", 2);
                }
                else if (HP < maxHP * 1 / 3)
                {
                    anim.SetInteger("Stage", 3);
                }
            }
            

            BarsUpdate();
            //Enemy animation
            anim.SetBool("GotHit", true);
            sr.color = new Color(0.75f, 0.25f, 0.25f);
            deffaultPosition = transform.position;
            StartCoroutine(Shacking(deffaultPosition));
        }
        
    }

    public static bool HasParameter(string paramName, Animator animator)
    {
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.name == paramName)
                return true;
        }
        return false;
    }

    public override void BarsUpdate()
    {
        if (armor <= 0)
        {
            armorText.text = "";
            armorGO.SetActive(false);
        }
        else
        {
            armorGO.SetActive(true);
            armorText.text = armor.ToString();
        }
        hpBar.size = new Vector2(HPSizeMultiple * HP, hpBar.size.y);
        hpText.SetText(HP + "/" + maxHP);
    }

    public void SetArmor(int armor)
    {
        this.armor = armor;
        BarsUpdate();
    }
    public override void EffectUpdate()
    {
        foreach (GameObject gm in curEffectIcons)
        {
            Destroy(gm.gameObject);
        }
        curEffectIcons.Clear();

        if (plusDamage > 0)
        {
            GameObject effectIcon = Instantiate(effectIconPrefabs[0], Vector3.zero, Quaternion.Euler(0, 0, 0), effectPos);
            effectIcon.transform.GetChild(0).GetComponent<TextMeshPro>().SetText(plusDamage.ToString());
            curEffectIcons.Add(effectIcon);
        }
        if (poison > 0)
        {
            GameObject effectIcon = Instantiate(effectIconPrefabs[1], Vector3.zero, Quaternion.Euler(0, 0, 0), effectPos);
            effectIcon.transform.GetChild(0).GetComponent<TextMeshPro>().SetText(poison.ToString() + "t");
            curEffectIcons.Add(effectIcon);
        }
        if (nextAttack == noneAttack)
        {
            GameObject effectIcon = Instantiate(effectIconPrefabs[2], Vector3.zero, Quaternion.Euler(0, 0, 0), effectPos);
            effectIcon.transform.GetChild(0).GetComponent<TextMeshPro>().SetText("");
            curEffectIcons.Add(effectIcon);
        }
        if (divideEnemies.Count != 0)
        {
            GameObject effectIcon = Instantiate(effectIconPrefabs[3], Vector3.zero, Quaternion.Euler(0, 0, 0), effectPos);
            effectIcon.transform.GetChild(0).GetComponent<TextMeshPro>().SetText("");
            curEffectIcons.Add(effectIcon);
        }
        if (fightManager.globalDifficult != 0)
        {
            GameObject effectIcon = Instantiate(effectIconPrefabs[4], Vector3.zero, Quaternion.Euler(0, 0, 0), effectPos);
            effectIcon.transform.GetChild(0).GetComponent<TextMeshPro>().SetText(fightManager.globalDifficult.ToString());
            curEffectIcons.Add(effectIcon);
        }
        if (plusMissChance > 0)
        {
            GameObject effectIcon = Instantiate(effectIconPrefabs[5], Vector3.zero, Quaternion.Euler(0, 0, 0), effectPos);
            effectIcon.transform.GetChild(0).GetComponent<TextMeshPro>().SetText(plusMissChance.ToString());
            curEffectIcons.Add(effectIcon);
        }
        if (isTerrifying)
        {
            GameObject effectIcon = Instantiate(effectIconPrefabs[6], Vector3.zero, Quaternion.Euler(0, 0, 0), effectPos);
            effectIcon.transform.GetChild(0).GetComponent<TextMeshPro>().SetText("");
            curEffectIcons.Add(effectIcon);
        }

        for (int i = 0; i < curEffectIcons.Count; i++)
        {
            if ((i + 4) % 3 == 1)
            {
                curEffectIcons[i].transform.localPosition = new Vector3(-1, i, 0);
            }
            else if ((i + 4) % 3 == 2)
            {
                curEffectIcons[i].transform.localPosition = new Vector3(0, i - 1, 0);
            }
            else if ((i + 4) % 3 == 0)
            {
                curEffectIcons[i].transform.localPosition = new Vector3(1, i - 2, 0);
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
        if (death && divideEnemies.Count == 0)
        {
            deathTime += Time.deltaTime;
            sr.material = deathMat;
            sr.material.SetFloat("_OutlineWidth", deathTime);
            if (deathTime >= 1)
            {
                Die();
            }
        }
        else if (runningAway)
        {
            deathTime += Time.deltaTime;
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1 - deathTime);
            if (deathTime >= 1)
            {
                Destroy(gameObject);
            }
        }
        else
        {
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
    private IEnumerator Shacking(Vector2 defaultPos)
    {
        if (!death && !runningAway)
        {
            for (int i = 0; i < 7; i++)
            {
                Shakingx = UnityEngine.Random.Range(-0.5f, 0.5f);
                Shakingy = UnityEngine.Random.Range(-0.5f, 0.5f);
                transform.position = defaultPos + new Vector2(Shakingx, Shakingy);
                yield return new WaitForSeconds(0.1f);
            }
            transform.position = defaultPos;
            anim.SetBool("GotHit", false);
            sr.color = new Color(1f, 1f, 1f);
        }
    }
    public void Die()
    {
        if (divideEnemies.Count != 0)
        {
            List<Vector3> pos = new List<Vector3> { cell.position + new Vector3(-3, 0, 0), cell.position + new Vector3(3, 0, 0) };
            fightManager.AddEnemies(divideEnemies, pos);
        }
        fightManager.AddGold(gold);
        Destroy(gameObject);
    }
}