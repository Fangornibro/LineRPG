using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Room : MonoBehaviour
{
    public enum Statement { unable, completed, next }
    public Statement state;
    public List<Sprite> EventSprites;

    [System.Serializable]
    public class LocationSprites
    {
        public string name;
        public List<Sprite> sprites;
    }
    public List<LocationSprites> locationSprites;
    private string locationName;

    private SpriteRenderer sr;
    private SpriteRenderer EventSr;
    private FightManager fm;
    private SquadInfoPanel sip;
    private RoomSelector rs;
    private CameraScript cam;

    [HideInInspector]
    public Sprite defaultSprite;
    [HideInInspector]
    public bool isBoss = false, isStart = false;
    [HideInInspector]
    public Squad squad;

    public List<Room> connectedRooms = new List<Room>();
    private LineRenderer lr;

    private AudioSource buttonDownSound, buttonUpSound;
    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        sip = GameObject.Find("SquadInfoPanel").GetComponent<SquadInfoPanel>();
        fm = GameObject.Find("FightManager").GetComponent<FightManager>();
        rs = GameObject.Find("AbilityOnCursor").GetComponent<RoomSelector>();
        cam = GameObject.Find("Main Camera").GetComponent<CameraScript>();

        buttonDownSound = GameObject.Find("buttonDownSound").GetComponent<AudioSource>();
        buttonUpSound = GameObject.Find("buttonUpSound").GetComponent<AudioSource>();

        sr = GetComponent<SpriteRenderer>();
        int randLoc = Random.Range(0, locationSprites.Count);
        sr.sprite = locationSprites[randLoc].sprites[Random.Range(0, locationSprites[randLoc].sprites.Count)];
        locationName = locationSprites[randLoc].name;
        EventSr = transform.Find("EventIcon").GetComponent<SpriteRenderer>();
        if (fm.events.Count != 0)
        {
            EventSr.sprite = EventSprites[Random.Range(2, EventSprites.Count)];
        }
        else
        {
            EventSr.sprite = EventSprites[Random.Range(3, EventSprites.Count)];
        }
        if (EventSr.sprite.name == "FightIcon")
        {
            int diff = Mathf.RoundToInt(Vector3.Distance(Vector3.zero, transform.position)/25);
            List<Squad> possibleSquads = new List<Squad>();
            for (int i = 0; i < fm.squads.Count; i++)
            {
                if (fm.squads[i].difficult == diff)
                {
                    possibleSquads.Add(fm.squads[i]);
                }
            }
            //Enemy type
            squad = possibleSquads[Random.Range(0, possibleSquads.Count)];
        }
        else if (EventSr.sprite.name == "BreadIcon")
        {
            squad = fm.baker;
        }
        else if (EventSr.sprite.name == "ChestIcon")
        {
            squad = fm.chest;
            squad.difficult = Mathf.RoundToInt(Vector3.Distance(Vector3.zero, transform.position) / 10) + Random.Range(1, 4);
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
    }

    public void SetStart()
    {
        isStart = true;
        state = Room.Statement.next;
        EventSr.sprite = EventSprites[0];
        squad = fm.elder;
    }
    void OnMouseDown()
    {
        if (!cam.isKinematic && !EventSystem.current.IsPointerOverGameObject())
        {
            Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -15);
            rs.ChangeSelection(this);
            buttonDownSound.Play();
            if (state == Statement.next)
            {
                sip.RoomInfoReceiving(this, EventSr.sprite.name, locationName, squad);
            }
        }
    }

    void OnMouseUp()
    {
        if (!cam.isKinematic)
        {
            buttonUpSound.Play();
        }
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
    public void CanBePlaced(List<Room> rooms)
    {
        foreach (Room r in rooms)
        {
            if (Vector3.Distance(r.transform.position, transform.position) < 3)
            {
                Merge(r);
            }
        }
    }

    private void Merge(Room other)
    {
        if (other.isStart)
        {
            Destroy(gameObject);
        }
        else
        {
            Merging(other);
        }
    }

    private void Merging(Room other)
    {
        for (int i = 0; i < other.connectedRooms.Count; i++)
        {
            if (other.connectedRooms[i] != null)
            {
                other.connectedRooms[i].connectedRooms.Remove(other);
                other.connectedRooms[i].connectedRooms.Add(this);
            }
        }
        connectedRooms.AddRange(other.connectedRooms);
        Destroy(other.gameObject);
    }

    public void SetColorsToDefault()
    {
        GetComponent<LineRenderer>().SetWidth(0.2f, 0.2f);
        Color color = Color.white;
        if (state == Statement.unable)
        {
            GetComponent<LineRenderer>().sortingOrder = -3;
            color = new Color(0.4339623f, 0.4339623f, 0.4339623f);
        }
        else if (state == Statement.completed)
        {
            GetComponent<LineRenderer>().sortingOrder = -1;
            color = new Color(0.2117647f, 0.4901961f, 0.2156863f);
        }
        else if (state == Statement.next)
        {
            GetComponent<LineRenderer>().sortingOrder = -2;
            color = new Color(0.8396226f, 0.5861914f, 0.1703008f);
        }
        transform.Find("Outline").GetComponent<SpriteRenderer>().color = color;
        GetComponent<LineRenderer>().SetColors(color, color);
    }
}
