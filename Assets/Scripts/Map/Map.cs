using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;
using static UnityEditor.FilePathAttribute;

public class Map : MonoBehaviour
{
    private LineRenderer lr;
    public int mapWidth, mapHeight;
    public Room roomPrefab;
    [HideInInspector]
    public List<Room> rooms, newrooms = new List<Room>();
    private Transform tr;
    private FightManager fm;
    public void RoomsSpawn()
    {
        //for sorted by id list of rooms
        for (int x = 0; x < mapHeight * mapWidth; x++)
        {
            newrooms.Add(null);
        }
        for (int i = 0; i < mapHeight; i++)
        {
            for (int j = 0; j < mapWidth; j++)
            {
                //Set position
                roomPrefab.y = i * roomPrefab.height * 3;
                roomPrefab.x = j * roomPrefab.width * 3;
                //Creating room
                Room room = GameObject.Instantiate(roomPrefab, new Vector3(roomPrefab.x, roomPrefab.y, 0), new Quaternion(0, 0, 0, 0).normalized, tr);
                //Add room to rooms list
                rooms.Add(room);
                //Set room id and sort by this id (for snake effect)
                if (i % 2 == 0)
                {
                    room.id = j + i * 10;
                    newrooms[j + i * 10] = room;
                }
                else
                {
                    room.id = 9 - j + i * 10;
                    newrooms[9 - j + i * 10] = room;
                }
            }
        }
        rooms = newrooms;
        rooms[rooms.Count - 1].isBoss = true;
        rooms[0].isStart = true;
    }
    public void NextRoom(int id, string curEventString, string curLocationString , Squad squad)
    {
        foreach (Room r in rooms)
        {
            if (r.id == id)
            {
                r.state = Room.Statement.completed;
                Camera.main.transform.position = new Vector3(150, 0, -15);
                fm.curEventString = curEventString;
                fm.curLocationString = curLocationString;
                fm.curSquad = squad;
                fm.RoomStart();
                fm.start = true;
            }
            else if (r.id == id + 1)
            {
                r.state = Room.Statement.next;
                break;
            }
        }
    }
    void Start()
    {
        fm = GameObject.Find("FightManager").GetComponent<FightManager>();
        tr = transform;
        //Rooms spawn
        RoomsSpawn();
        //Draw a line
        lr = GetComponent<LineRenderer>();
        lr.positionCount = rooms.Count;
        for (int i = 0; i < rooms.Count; i++)
        {
            lr.SetPosition(i, rooms[i].transform.position);
        }
        rooms[0].state = Room.Statement.next;
    }
    
}
