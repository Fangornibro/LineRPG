using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UIElements;

public class Room : MonoBehaviour
{
    public enum Statement { unable, completed, next }
    public Statement state;
    public List<Sprite> EventSprites, LocationSprites;
    private SpriteRenderer sr, EventSr, LocationSr;
    private FightManager fm;
    private SquadInfoPanel sip;
    private Inventory inventory;

    public int width, height, x, y, id;
    [HideInInspector]
    public Sprite defaultSprite;
    [HideInInspector]
    public bool isBoss = false, isStart = false;
    [HideInInspector]
    public Squad squad;
    private void Awake()
    {
        sip = GameObject.Find("SquadInfoPanel").GetComponent<SquadInfoPanel>();
        fm = GameObject.Find("FightManager").GetComponent<FightManager>();
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();

        sr = GetComponent<SpriteRenderer>();
        EventSr = transform.Find("EventIcon").GetComponent<SpriteRenderer>();
        LocationSr = transform.Find("LocationIcon").GetComponent<SpriteRenderer>();
        if (fm.events.Count != 0)
        {
            EventSr.sprite = EventSprites[Random.Range(2, EventSprites.Count)];
        }
        else
        {
            EventSr.sprite = EventSprites[Random.Range(3, EventSprites.Count)];
        }
        LocationSr.sprite = LocationSprites[Random.Range(1, LocationSprites.Count)];
        if (EventSr.sprite.name == "FightIcon")
        {
            //Enemy type
            squad = fm.squads[Random.Range(0, fm.squads.Count)];
        }
        else if (EventSr.sprite.name == "BreadIcon")
        {
            squad = fm.baker;
        }
        else if (EventSr.sprite.name == "ChestIcon")
        {
            squad = fm.chest;
        }
        else if (EventSr.sprite.name == "QuestionIcon")
        {
            int rand = Random.Range(0, fm.events.Count);
            squad = fm.events[rand];
            fm.events.RemoveAt(rand);
        }
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
            EventSr.sprite = EventSprites[1];
            squad = fm.bosses[Random.Range(0, fm.bosses.Count)];
            isBoss = false;
        }
        else if (isStart)
        {
            EventSr.sprite = EventSprites[0];
            LocationSr.sprite = LocationSprites[0];
            squad = fm.elder;
            isStart = false;
        }
    }
    void OnMouseDown()
    {
        if (state == Statement.next)
        {
            sip.isOpened = true;
            sip.RoomInfoReceiving(id, EventSr.sprite.name, LocationSr.sprite.name, squad);
            inventory.isInventOpen = true;
        }
    }
}
