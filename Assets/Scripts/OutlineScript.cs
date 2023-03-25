using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineScript : MonoBehaviour
{
    private SpriteRenderer sr;
    private AbilityOnCursor abilityOnCursor;
    //Outline
    public SpriteRenderer outlinePrefab;
    private List<SpriteRenderer> outlineList;
    public enum Character { enemy, player }
    public Character character;
    private void Start()
    {
        abilityOnCursor = GameObject.Find("AbilityOnCursor").GetComponent<AbilityOnCursor>();
        sr = GetComponent<SpriteRenderer>();
        //Outline
        outlineList = new List<SpriteRenderer> { 
            Instantiate(outlinePrefab, new Vector3(transform.position.x + 0.248f, transform.position.y, 0), new Quaternion(0, 0, 0, 0).normalized, transform), 
            Instantiate(outlinePrefab, new Vector3(transform.position.x - 0.248f, transform.position.y, 0), new Quaternion(0, 0, 0, 0).normalized, transform),
            Instantiate(outlinePrefab, new Vector3(transform.position.x, transform.position.y + 0.248f, 0), new Quaternion(0, 0, 0, 0).normalized, transform),
            Instantiate(outlinePrefab, new Vector3(transform.position.x, transform.position.y - 0.248f, 0), new Quaternion(0, 0, 0, 0).normalized, transform)
        };
        foreach (SpriteRenderer s in outlineList)
        {
            s.gameObject.SetActive(false);
        }
    }
    private void Update()
    {
        foreach (SpriteRenderer s in outlineList)
        {
            s.sprite = sr.sprite;
        }
    }

    private void OnMouseOver()
    {
        if (character == Character.enemy && (abilityOnCursor.abilityType == Item.Type.attack) && abilityOnCursor.isOnCursor)
        {
            foreach (SpriteRenderer s in outlineList)
            {
                s.gameObject.SetActive(true);
                s.flipX = sr.flipX;
            }
        }
        else if (character == Character.player && abilityOnCursor.abilityType == Item.Type.Buff && abilityOnCursor.isOnCursor)
        {
            foreach (SpriteRenderer s in outlineList)
            {
                s.gameObject.SetActive(true);
                s.flipX = sr.flipX;
            }
        }
        else
        {
            foreach (SpriteRenderer s in outlineList)
            {
                s.gameObject.SetActive(false);
            }
        }
    }
    private void OnMouseExit()
    {
        foreach (SpriteRenderer s in outlineList)
        {
            s.gameObject.SetActive(false);
        }
    }
}
