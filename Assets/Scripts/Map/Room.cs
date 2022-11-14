using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public enum Statement { unable, completed, next }
    public Statement state;
    public List<Sprite> EventSprites, LocationSprites;
    private SpriteRenderer sr, EventSr, LocationSr;
    private Map map;

    public int width, height, x, y, id;
    [HideInInspector]
    public Sprite defaultSprite;
    [HideInInspector]
    public bool isBoss = false;
    private void Awake()
    {
        map = GameObject.Find("Map").GetComponent<Map>();
        sr = GetComponent<SpriteRenderer>();
        EventSr = transform.Find("EventIcon").GetComponent<SpriteRenderer>();
        LocationSr = transform.Find("LocationIcon").GetComponent<SpriteRenderer>();
        EventSr.sprite = EventSprites[Random.Range(1, EventSprites.Count)];
        LocationSr.sprite = LocationSprites[Random.Range(0, LocationSprites.Count)];
    }
    private void Update()
    {
        if (state == Statement.unable)
        {
            sr.color = Color.gray;
        }
        else if (state == Statement.completed)
        {
            sr.color = new Color(0.2117647f, 0.4901961f, 0.2156863f);
        }
        else if (state == Statement.next)
        {
            sr.color = new Color(0.745283f, 0.6887745f, 0.2847544f);
        }
        if (isBoss)
        {
            EventSr.sprite = EventSprites[0];
        }
    }
    void OnMouseDown()
    {
        if (state == Statement.next)
        {
            map.NextRoom(id, EventSr.sprite.name, LocationSr.sprite.name);
        }
    }
}
