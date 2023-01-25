using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class Room : MonoBehaviour
{
    public enum Statement { unable, completed, next }
    public Statement state;
    public List<Sprite> EventSprites, LocationSprites;
    [HideInInspector]
    public SpriteRenderer sr;
    private SpriteRenderer EventSr, LocationSr;
    private FightManager fm;
    private SquadInfoPanel sip;
    private RoomSelector rs;

    public int width, height, x, y;
    [HideInInspector]
    public Sprite defaultSprite;
    [HideInInspector]
    public bool isBoss = false, isStart = false;
    [HideInInspector]
    public Squad squad;


    private int numbersOfAreas = 0;
    [HideInInspector]
    public int j, i, k;

    public List<Room> connectedRooms = new List<Room>();
    private LineRenderer lr;

    private AudioSource buttonDownSound, buttonUpSound;
    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        sip = GameObject.Find("SquadInfoPanel").GetComponent<SquadInfoPanel>();
        fm = GameObject.Find("FightManager").GetComponent<FightManager>();
        rs = GameObject.Find("AbilityOnCursor").GetComponent<RoomSelector>();

        buttonDownSound = GameObject.Find("buttonDownSound").GetComponent<AudioSource>();
        buttonUpSound = GameObject.Find("buttonUpSound").GetComponent<AudioSource>();

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
        rs.ChangeSelection(this);
        buttonDownSound.Play();
        if (state == Statement.next)
        {
            sip.RoomInfoReceiving(this, EventSr.sprite.name, LocationSr.sprite.name, squad);
        }
    }

    void OnMouseUp()
    {
        buttonUpSound.Play();
    }

    public void UpdateAllLines()
    {
        M1:
        for (int i = 0; i < connectedRooms.Count; i++)
        {
            for (int j = 0; j < connectedRooms.Count; j++)
            {
                if (connectedRooms[i] == connectedRooms[j] && j != i)
                {
                    connectedRooms.RemoveAt(i);
                    goto M1;
                }
            }
        }
        //Draw a line
        lr.positionCount = connectedRooms.Count * 2;
        List<Vector3> vec = new List<Vector3>();
        List<Vector3> vecFin = new List<Vector3>();
        for (int i = 0; i < connectedRooms.Count; i++)
        {
            vec.Add(connectedRooms[i].transform.position);
        }
        for (int i = 0; i < vec.Count; i++)
        {
            vecFin.Add(transform.position);
            vecFin.Add(vec[i]);
        }
        lr.SetPositions(vecFin.ToArray());
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "RoomArea")
        {
            numbersOfAreas++;
            if (numbersOfAreas >= 2)
            {
                Merge(collision.transform.parent.GetComponent<Room>());
            }
        }
    }

    private void Merge(Room other)
    {
        if (j < other.j)
        {
            Merging(other);
            numbersOfAreas = 1;
        }
        else if (j == other.j)
        {
            if (i < other.i)
            {
                Merging(other);
                numbersOfAreas = 1;
            }
            else if (i == other.i)
            {
                if (k < other.k)
                {
                    Merging(other);
                    numbersOfAreas = 1;
                }
            }
        }
    }

    private void Merging(Room other)
    {
        foreach (Room r in other.connectedRooms)
        {
            r.connectedRooms.Remove(other);
            r.connectedRooms.Add(this);
        }
        connectedRooms.AddRange(other.connectedRooms);
        Destroy(other.gameObject);
    }

    public void SetColorsToDefault()
    {
        if (state == Statement.unable)
        {
            GetComponent<LineRenderer>().sortingOrder = -3;
            sr.color = new Color(0.490566f, 0.490566f, 0.490566f);
            GetComponent<LineRenderer>().SetColors(new Color(0.490566f, 0.490566f, 0.490566f), new Color(0.490566f, 0.490566f, 0.490566f));
        }
        else if (state == Statement.completed)
        {
            GetComponent<LineRenderer>().sortingOrder = -1;
            sr.color = new Color(0.2117647f, 0.4901961f, 0.2156863f);
            GetComponent<LineRenderer>().SetColors(new Color(0.2117647f, 0.4901961f, 0.2156863f), new Color(0.2117647f, 0.4901961f, 0.2156863f));
        }
        else if (state == Statement.next)
        {
            GetComponent<LineRenderer>().sortingOrder = -2;
            sr.color = new Color(0.8396226f, 0.5861914f, 0.1703008f);
            GetComponent<LineRenderer>().SetColors(new Color(0.490566f, 0.490566f, 0.490566f), new Color(0.490566f, 0.490566f, 0.490566f));
        }
    }
}
