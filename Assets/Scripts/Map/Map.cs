using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Map : MonoBehaviour
{
    public Room roomPrefab;
    [HideInInspector]
    public List<Room> rooms;
    private FightManager fm;
    public int numOfRooms, maxConnections, numberOfBosses;
    private RoomSelector rs;

    //Camera
    private Vector3 targetPos;
    private CameraScript cam;
    private bool isStartedAnim = false;
    //Room info
    private Room room;
    private string eventName, locationName;
    private Squad squad;

    void Start()
    {
        cam = GameObject.Find("Main Camera").GetComponent<CameraScript>();
        rs = GameObject.Find("AbilityOnCursor").GetComponent<RoomSelector>();
        fm = GameObject.Find("FightManager").GetComponent<FightManager>();
        //Rooms spawn
        cam.isKinematic = true;
        rooms.Add(Instantiate(roomPrefab, transform.position, Quaternion.Euler(Vector3.zero), transform));
        rooms[0].SetStart();
        targetPos = rooms[0].transform.position;
        StartCoroutine(RoomsSpawn());
    }

    private void Update()
    {
        if (isStartedAnim)
        {
            isStartedAnim = cam.Approximation(targetPos, 15, false);
        }
    }
    private IEnumerator RoomsSpawn()
    {
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
                        yield return new WaitForEndOfFrame();
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
            Start();
        }
        else
        {
            float maxPosX = 0, minPosX = 0, maxPosY = 0, minPosY = 0;
            List<Room> mayBosses = new List<Room>();
            foreach (Room r in rooms)
            {
                if (r.transform.position.x > maxPosX)
                {
                    maxPosX = r.transform.position.x;
                }
                if (r.transform.position.x < minPosX)
                {
                    minPosX = r.transform.position.x;
                }
                if (r.transform.position.y > maxPosY)
                {
                    maxPosY = r.transform.position.y;
                }
                if (r.transform.position.y < minPosY)
                {
                    minPosY = r.transform.position.y;
                }
                r.UpdateAllLines();
                r.SetColorsToDefault();
                if (r.connectedRooms.Count == 1)
                {
                    mayBosses.Add(r);
                }
            }
            cam.coordinatesReceived(maxPosX, minPosX, maxPosY, minPosY);
            if (numberOfBosses > mayBosses.Count)
            {
                numberOfBosses = mayBosses.Count;
            }
            for (int i = 0; i < numberOfBosses; i++)
            {
                int rand = Random.Range(0, mayBosses.Count - 1);
                mayBosses[rand].isBoss= true;
                mayBosses.RemoveAt(rand);
            }
            //Effect
            isStartedAnim = true;
        }
    }

    public void RoomInfoReceiving(Room Room, string EventName, string LocationName, Squad Squad)
    {
        room = Room;
        eventName = EventName;
        locationName = LocationName;
        squad = Squad;
    }

    public void NextRoom()
    {
        room.state = Room.Statement.completed;
        Camera.main.transform.position = new Vector3(150, 0, -15);
        fm.curEventString = eventName;
        fm.curLocationString = locationName;
        fm.curSquad = squad;
        fm.roomPos = room.transform.position;
        fm.RoomStart();
        fm.start = true;
        foreach (Room r in room.connectedRooms)
        {
            if (r.state == Room.Statement.unable)
            {
                r.state = Room.Statement.next;
                r.SetColorsToDefault();
            }
        }
        rs.ChangeSelection(null);
    }
}
