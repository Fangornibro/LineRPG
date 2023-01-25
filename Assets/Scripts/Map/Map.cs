using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;
using static Room;
using static UnityEditor.FilePathAttribute;

public class Map : MonoBehaviour
{
    public Room roomPrefab;
    [HideInInspector]
    public List<Room> rooms, newrooms = new List<Room>();
    private FightManager fm;
    public int numOfRooms, maxConnections, numberOfBosses;
    private RoomSelector rs;
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
                        newRoom.j = j;
                        newRoom.i = i;
                        newRoom.k = k;
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
                        yield return new WaitForSeconds(0.025f);
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
            List<Room> mayBosses = new List<Room>();
            foreach (Room r in rooms)
            {
                r.UpdateAllLines();
                Destroy(r.transform.Find("Area").gameObject);
                r.SetColorsToDefault();
                if (r.connectedRooms.Count == 1)
                {
                    mayBosses.Add(r);
                }
            }
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
        }
    }
    public void NextRoom(Room room, string curEventString, string curLocationString , Squad squad)
    {
        room.state = Room.Statement.completed;
        Camera.main.transform.position = new Vector3(150, 0, -15);
        fm.curEventString = curEventString;
        fm.curLocationString = curLocationString;
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
    void Start()
    {
        rs = GameObject.Find("AbilityOnCursor").GetComponent<RoomSelector>();
        fm = GameObject.Find("FightManager").GetComponent<FightManager>();
        //Rooms spawn
        rooms.Add(Instantiate(roomPrefab, transform.position, Quaternion.Euler(Vector3.zero), transform));
        rooms[0].isStart = true;
        rooms[0].state = Room.Statement.next;
        StartCoroutine(RoomsSpawn());
    }
    
}
