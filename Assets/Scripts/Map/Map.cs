using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Map : MonoBehaviour
{
    [HideInInspector] public List<Room> rooms;
    //Camera
    private Vector3 targetPos;
    //Room info
    [HideInInspector] public Room room;
    private Room roomFromInfoRec;
    private string locationName;
    [HideInInspector] public Squad squad;
    private float maxDist = 0;


    [Header("Stats")]
    [SerializeField] private int numOfRooms;
    [SerializeField] private int maxConnections;
    [SerializeField] private int numberOfBosses;


    [Space]
    [Space]
    [Header("Initialisations")]
    [SerializeField] private Room roomPrefab;
    [SerializeField] private FightManager fightManager;
    [SerializeField] private RoomSelector roomSelector;
    [SerializeField] private CameraScript cameraScript;
    [SerializeField] private GameManager gameManager;
    void Start()
    {
        //Rooms spawn
        if (!SelectCharacterButton.isTutuorial)
        {
            StartRoomsSpawnCoroutine(false);
        }
    }
    public void StartRoomsSpawnCoroutine(bool isTutorial)
    {
        StartCoroutine(RoomsSpawn(isTutorial));
    }
    private IEnumerator RoomsSpawn(bool isTutorial)
    {
        cameraScript.isKinematic = true;
        rooms.Add(Instantiate(roomPrefab, transform.position, Quaternion.Euler(Vector3.zero), transform));
        for (int j = 0; j < 100; j++)
        {
        M2:
            for (int i = 0; i < rooms.Count; i++)
            {
                for (int k = 0; k < 3; k++)
                {
                    for (int s = 0; s < rooms.Count; s++)
                    {
                        if (rooms[s] == null)
                        {
                            rooms.RemoveAt(s);
                            s--;
                        }
                    }
                    if (rooms.Count >= numOfRooms)
                    {
                        goto M1;
                    }
                    try
                    {
                        if (rooms[i].connectedRooms.Count < maxConnections)
                        {
                        }
                    }
                    catch
                    {
                        goto M2;
                    }
                    if (rooms[i].connectedRooms.Count < maxConnections)
                    {
                        Room newRoom = Instantiate(roomPrefab, rooms[i].transform.position, Quaternion.Euler(Vector3.zero), transform);
                        newRoom.transform.position = newRoom.transform.position + new Vector3(0, 5);
                        int randAngel = Random.Range(0, 359);
                        newRoom.transform.RotateAround(rooms[i].transform.position, Vector3.forward, randAngel);
                        newRoom.transform.rotation = Quaternion.Euler(Vector3.zero);
                        newRoom.CanBePlaced(rooms);
                        rooms.Add(newRoom);
                        newRoom.connectedRooms.Add(rooms[i]);
                        rooms[i].connectedRooms.Add(newRoom);
                        foreach (Room r in rooms)
                        {
                            for (int v = 0; v < r.connectedRooms.Count; v++)
                            {
                                if (r.connectedRooms[v] == null)
                                {
                                    r.connectedRooms.RemoveAt(v);
                                    v--;
                                }
                            }
                        }
                        yield return null;
                    }
                }
            }
        }
    M1:
        if (rooms.Count < numOfRooms)
        {
            foreach (Room r in rooms)
            {
                Destroy(r.gameObject);
            }
            rooms.Clear();
            StartRoomsSpawnCoroutine(isTutorial);
        }
        else
        {
            float maxPosX = 0, minPosX = 0, maxPosY = 0, minPosY = 0;
            List<Room> mayBosses = new List<Room>();
            foreach (Room r in rooms)
            {
                Vector2 rpos = r.transform.position;
                if (rpos.x > maxPosX)
                {
                    maxPosX = rpos.x;
                }
                if (rpos.x < minPosX)
                {
                    minPosX = rpos.x;
                }
                if (rpos.y > maxPosY)
                {
                    maxPosY = rpos.y;
                }
                if (rpos.y < minPosY)
                {
                    minPosY = rpos.y;
                }
                if (Vector2.Distance(rpos, Vector2.zero) > maxDist)
                {
                    maxDist = Vector2.Distance(rpos, Vector2.zero);
                }
            }
            cameraScript.coordinatesReceived(maxPosX, minPosX, maxPosY + 20, minPosY);
            room = rooms[0];
            targetPos = room.transform.position;
            if (!isTutorial)
            {
                gameManager.FocusPanelVisible(true);
                //Start room
                rooms[0].SetStart();
                cameraScript.StartApproximationCoroutine(targetPos, 15, false);
            }
            else
            {
                room.state = Room.Statement.next;
            }
            //Set difficult
            foreach (Room r in rooms)
            {
                r.SetDifficultForFight(maxDist);
                if (r.connectedRooms.Count == 1 && r.squad.difficult >= r.maxDifficult - 1)
                {
                    mayBosses.Add(r);
                }
                r.UpdateAllLines();
                r.SetColorsToDefault();
            }
            if (numberOfBosses > mayBosses.Count)
            {
                numberOfBosses = mayBosses.Count;
            }
            for (int i = 0; i < numberOfBosses; i++)
            {
                int rand = Random.Range(0, mayBosses.Count - 1);
                mayBosses[rand].SetBoss();
                mayBosses.RemoveAt(rand);
            }
        }
    }

    public void SetCursedRoom(Squad squad)
    {
        List<Room> tempRoomList = new List<Room>();
        foreach (Room r in rooms)
        {
            if (squad.difficult == r.difficult && !r.squad.isCursed)
            {
                tempRoomList.Add(r);
            }
        }
        tempRoomList[Random.Range(0, tempRoomList.Count)].SetCursed(squad);
    }

    public void RoomInfoReceiving(Room Room, string LocationName, Squad Squad)
    {
        roomFromInfoRec = Room;
        locationName = LocationName;
        squad = Squad;
    }

    public void NextRoom()
    {
        if (room != null)
        {
            foreach (Room r in room.connectedRooms)
            {
                r.state = Room.Statement.unable;
                r.SetColorsToDefault();
            }
        }
        room = roomFromInfoRec;
        room.state = Room.Statement.completed;
        foreach (Room r in room.connectedRooms)
        {
            if (r != room)
            {
                if (r.state == Room.Statement.completed)
                {
                    r.RoomRandomisation();
                    r.SetDifficultForFight(maxDist);
                }
                r.state = Room.Statement.next;
            }
            r.SetColorsToDefault();
        }
        fightManager.RoomStart(locationName, squad);
        roomSelector.ChangeSelection(null);
        gameManager.MapVisible(false);
    }
}
