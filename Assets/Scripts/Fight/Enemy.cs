using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int HP;
    public Material outline;
    private Material def;
    private SpriteRenderer sr;
    //Hit
    AbilityOnCursor abilityOnCursor;
    private Animator anim;
    private float hitDuration = 0.7f;
    private bool gotHit = false;
    //Shaking
    private Vector3 defaultPos;
    private float Shakingx, Shakingy;
    private float timeBtwShaking = 0.1f;
    void Start()
    {
        abilityOnCursor = GameObject.Find("AbilityOnCursor").GetComponent<AbilityOnCursor>();
        sr = GetComponent<SpriteRenderer>();
        def = sr.material;
        anim = GetComponent<Animator>();
        defaultPos = transform.position;
    }

    private void OnMouseEnter()
    {
        sr.material = outline;
        sr.material.SetColor("_OutlineColor", new Color(1f, 1f, 0.5f));
    }
    private void OnMouseExit()
    {
        sr.material = def;
    }
    private void OnMouseDown()
    {
        if (abilityOnCursor.curDamage != 0)
        {
            HP -= abilityOnCursor.curDamage;
            if (HP <= 0)
            {
                GameObject.Destroy(gameObject);
            }
            anim.SetBool("GotHit", true);
            sr.color = new Color(0.75f, 0.25f, 0.25f);
            gotHit = true;
        }
    }
    private void Update()
    {
        if (gotHit)
        {
            hitDuration -= Time.deltaTime;
            if (timeBtwShaking <= 0)
            {
                Shakingx = Random.RandomRange(-1f, 1f);
                Shakingy = Random.RandomRange(-1f, 1f);
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
    }
}