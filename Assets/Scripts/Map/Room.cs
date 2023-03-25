using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

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

    


    [HideInInspector] public bool isStart = false;
    [HideInInspector] public Squad squad;
    [HideInInspector] public List<Room> connectedRooms = new List<Room>();

    [HideInInspector] public int maxDifficult = 4;
    [HideInInspector] public int difficult = 0;

    //Initialisation
    private bool firstPush = true;
    private AudioSource buttonDownSound, buttonUpSound;
    private SpriteRenderer spriteRenderer;
    private LineRenderer lineRenderer;
    private SpriteRenderer outlineSpriteRenderer;
    private FightManager fightManager;
    private RoomSelector roomSelector;
    private CameraScript cameraScript;
    private GameManager uiManager;

    [SerializeField] private SpriteRenderer EventSr;
    private void Awake()
    {
        //Initialisation
        lineRenderer = GetComponent<LineRenderer>();
        uiManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        fightManager = uiManager.fightManager;
        spriteRenderer = GetComponent<SpriteRenderer>();
        outlineSpriteRenderer = transform.Find("Outline").GetComponent<SpriteRenderer>();
        RoomRandomisation();
    }

    public void RoomRandomisation()
    {
        int randLoc = UnityEngine.Random.Range(0, locationSprites.Count);
        spriteRenderer.sprite = locationSprites[randLoc].sprites[UnityEngine.Random.Range(0, locationSprites[randLoc].sprites.Count)];
        locationName = locationSprites[randLoc].name;
        EventSr.sprite = EventSprites[UnityEngine.Random.Range(4, EventSprites.Count)];
        if (EventSr.sprite.name == "BreadIcon")
        {
            squad = fightManager.baker;
        }
    }

    public void SetBoss()
    {
        EventSr.sprite = EventSprites[1];
        squad = fightManager.bosses[UnityEngine.Random.Range(0, fightManager.bosses.Count)];
    }

    public void SetStart()
    {
        isStart = true;
        state = Statement.next;
        EventSr.sprite = EventSprites[0];
        squad = fightManager.elder;
    }

    public void SetCursed(Squad cursedSquad)
    {
        if (state != Statement.completed)
        {
            EventSr.sprite = EventSprites[2];
            squad = cursedSquad;
            squad.isCursed = true;
        }
    }
    void OnMouseDown()
    {
        if (firstPush)
        {
            roomSelector = GameObject.Find("AbilityOnCursor").GetComponent<RoomSelector>();
            cameraScript = GameObject.Find("Main Camera").GetComponent<CameraScript>();
            buttonDownSound = GameObject.Find("buttonDownSound").GetComponent<AudioSource>();
            buttonUpSound = GameObject.Find("buttonUpSound").GetComponent<AudioSource>();
            firstPush = false;
        }
        if (!cameraScript.isKinematic && !EventSystem.current.IsPointerOverGameObject())
        {
            Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -15);
            roomSelector.ChangeSelection(this);
            buttonDownSound.Play();
            if (state == Statement.next)
            {
                uiManager.InventoryVisible(true);
                uiManager.SquadInfoVisible(this, EventSr.sprite.name, locationName, squad);
            }
        }
    }

    void OnMouseUp()
    {
        if (!cameraScript.isKinematic && buttonUpSound != null)
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
        lineRenderer.positionCount = connectedRooms.Count * 2;
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
        lineRenderer.SetPositions(vecFin.ToArray());
    }
    public void CanBePlaced(List<Room> rooms)
    {
        foreach (Room r in rooms)
        {
            if (Vector3.Distance(r.transform.position, transform.position) < 3)
            {
                Merging(r);
            }
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
        lineRenderer.startWidth = 0.2f;
        lineRenderer.endWidth = 0.2f;
        lineRenderer.sortingOrder = -3;
        Color color = new Color(0.4339623f, 0.4339623f, 0.4339623f);
        outlineSpriteRenderer.color = color;
        if (state == Statement.completed)
        {
            lineRenderer.sortingOrder = -1;
            outlineSpriteRenderer.color = new Color(0.2117647f, 0.4901961f, 0.2156863f);
            color = new Color(0.8396226f, 0.5861914f, 0.1703008f);
        }
        else if (state == Statement.next)
        {
            outlineSpriteRenderer.color = new Color(0.8396226f, 0.5861914f, 0.1703008f);
        }
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
    }

    public void SetDifficultForFight(float maxDist)
    {
        if (EventSr.sprite.name == "FightIcon")
        {
            difficult = Mathf.RoundToInt(Vector3.Distance(Vector3.zero, transform.position) / (maxDist / maxDifficult));
            List<Squad> possibleSquads = new List<Squad>();
            for (int i = 0; i < fightManager.squads.Count; i++)
            {
                if (fightManager.squads[i].difficult == difficult)
                {
                    possibleSquads.Add(fightManager.squads[i]);
                }
            }
            //Enemy type
            squad = possibleSquads[UnityEngine.Random.Range(0, possibleSquads.Count)];
        }
        else if (EventSr.sprite.name == "ChestIcon")
        {
            difficult = Mathf.RoundToInt(Vector3.Distance(Vector3.zero, transform.position) / (maxDist / maxDifficult)) + 1;
            List<Squad> possibleSquads = new List<Squad>();
            for (int i = 0; i < fightManager.chests.Count; i++)
            {
                if (fightManager.chests[i].difficult == difficult)
                {
                    possibleSquads.Add(fightManager.chests[i]);
                }
            }
            //Chest type
            squad = possibleSquads[UnityEngine.Random.Range(0, possibleSquads.Count)];
        }
        else if (EventSr.sprite.name == "QuestionIcon")
        {
            difficult = Mathf.RoundToInt(Vector3.Distance(Vector3.zero, transform.position) / (maxDist / maxDifficult));
            List<Squad> possibleSquads = new List<Squad>();
            for (int i = 0; i < fightManager.events.Count; i++)
            {
                if (fightManager.events[i].difficult == difficult)
                {
                    possibleSquads.Add(fightManager.events[i]);
                }
            }
            //Event type
            if (possibleSquads.Count != 0)
            {
                int rand = UnityEngine.Random.Range(0, possibleSquads.Count);
                squad = possibleSquads[rand];
                fightManager.events.Remove(squad);
            }
            else
            {
                EventSr.sprite = EventSprites[3];
                SetDifficultForFight(maxDist);
            }
        }
    }
}
