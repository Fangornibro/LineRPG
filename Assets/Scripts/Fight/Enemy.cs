using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private SpriteRenderer sr;
    //Stats
    public float HP, armor;
    public int plusDamage = 0;
    private float HPSizeMultiple;
    //AllAttacks
    public List<EnemyAttack> attacks;
    [HideInInspector]
    public EnemyAttack nextAttack;
    //Hp bar
    private Transform hpBar;
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
    //Outline
    public SpriteRenderer outlinePrefab;
    private List<SpriteRenderer> outlineList;
    //Player
    private Player player;
    //StartFight
    public DialogueBranch dialoguebranch1, dialoguebranch2;
    //Patricles
    public ParticleSystem particleSystemPrefab;
    //Damage popup
    [SerializeField]
    private DamagePopup textPrefab;
    private DamagePopup damagePopup;
    //Armor
    TextMeshPro armorText;
    SpriteRenderer armorGO;
    //Number of enemies, chance to leave
    public int minNumberOfEnemies, maxNumberOfEnemies, chanceToLeave;
    //Name
    public string Name;
    //Temp
    private Transform temp;
    //Have skin?
    public bool isAnySkins;
    public void Create(float damage)
    {
        damagePopup = GameObject.Instantiate(textPrefab, new Vector2(transform.position.x + Random.Range(0, transform.localScale.x), transform.position.y + transform.localScale.y), Quaternion.identity);
        TextMeshPro text = damagePopup.GetComponent<TextMeshPro>();
        text.SetText(damage.ToString());
        damagePopup.transform.localScale = new Vector2(damagePopup.transform.localScale.x + damage / 100, damagePopup.transform.localScale.y + damage / 100);
        if (damagePopup.transform.localScale.x >= 2)
        {
            damagePopup.transform.localScale = new Vector2(2, 2);
        }
        damagePopup.textColor = new Color(0.682353f, 0.1215686f + damage / 122, 0.2078431f, 1f);
    }
    void Start()
    {
        //Random include second number
        maxNumberOfEnemies++; chanceToLeave++;
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
        hpBar = transform.Find("HpBarFront");
        HPSizeMultiple = hpBar.localScale.x / HP;
        //Player
        player = GameObject.Find("Player").GetComponent<Player>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        defaultPos = transform.position;
        //Outline
        outlineList = new List<SpriteRenderer>();
        outlineList.Add(GameObject.Instantiate(outlinePrefab, new Vector3(transform.position.x + 0.62f, transform.position.y, 0), new Quaternion(0, 0, 0, 0).normalized, transform));
        outlineList.Add(GameObject.Instantiate(outlinePrefab, new Vector3(transform.position.x - 0.62f, transform.position.y, 0), new Quaternion(0, 0, 0, 0).normalized, transform));
        outlineList.Add(GameObject.Instantiate(outlinePrefab, new Vector3(transform.position.x, transform.position.y + 0.62f, 0), new Quaternion(0, 0, 0, 0).normalized, transform));
        outlineList.Add(GameObject.Instantiate(outlinePrefab, new Vector3(transform.position.x, transform.position.y - 0.62f, 0), new Quaternion(0, 0, 0, 0).normalized, transform));
        foreach (SpriteRenderer s in outlineList)
        {
            s.gameObject.SetActive(false);
        }
        if (isAnySkins)
        {
            anim.SetInteger("Type", Random.Range(1, 5));
        }
    }

    private void OnMouseEnter()
    {
        foreach (SpriteRenderer s in outlineList)
        {
            s.gameObject.SetActive(true);
            s.flipX = sr.flipX;
        }
    }
    private void OnMouseExit()
    {
        foreach (SpriteRenderer s in outlineList)
        {
            s.gameObject.SetActive(false);
        }
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
        if (nextAttack.effect == EnemyAttack.Effect.armorUp)
        {
            GetArmor(nextAttack.damage);
        }
        else if (nextAttack.effect == EnemyAttack.Effect.runningAway)
        {
            RunningAway();
        }
        else if (nextAttack.effect == EnemyAttack.Effect.weakness)
        {
            player.weakness++;
        }
        else if (nextAttack.effect == EnemyAttack.Effect.flock)
        {
            for (int j = 0; j < temp.childCount; j++)
            {
                Enemy curEnemyForFlock = temp.GetChild(j).GetComponent<Enemy>();
                curEnemyForFlock.plusDamage++;
            }
        }
        else
        {
            player = GameObject.Find("Player").GetComponent<Player>();
            player.GetHit(nextAttack.damage + plusDamage, nextAttack.effect, Name);
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
    public void GetHit(int damage)
    {
        //Player set passive damage to 0
        player.passiveDamage = 0;
        //Particles
        Instantiate(particleSystemPrefab, new Vector2(transform.position.x, transform.position.y + transform.localScale.y / 2), Quaternion.Euler(0, 0, 0));
        //Popup
        Create(damage);
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
        hpBar.localScale = new Vector2(HPSizeMultiple * HP, hpBar.localScale.y);
        //Enemy animation
        anim.SetBool("GotHit", true);
        sr.color = new Color(0.75f, 0.25f, 0.25f);
        gotHit = true;
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
            if (outlineList != null)
            {
                foreach (SpriteRenderer s in outlineList)
                {
                    GameObject.Destroy(s.gameObject);
                }
                outlineList.Clear();
            }
            deathTime += Time.deltaTime;
            sr.material = deathMat;
            sr.material.SetFloat("_OutlineWidth", deathTime);
            if (deathTime >= 1)
            {
                GameObject.Destroy(gameObject);
            }
        }
        else
        {
            foreach (SpriteRenderer s in outlineList)
            {
                s.sprite = sr.sprite;
            }
            if (gotHit)
            {
                hitDuration -= Time.deltaTime;
                if (timeBtwShaking <= 0)
                {
                    Shakingx = Random.Range(-1f, 1f);
                    Shakingy = Random.Range(-1f, 1f);
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