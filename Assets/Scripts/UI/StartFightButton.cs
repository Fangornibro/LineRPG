using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.EventSystems;

public class StartFightButton : MonoBehaviour
{
    private SquadInfoPanel sip;
    private Inventory inventory;
    private Map map;

    private int id;
    private Squad squad;
    private string eventName, locationName;
    private void Start()
    {
        sip = GameObject.Find("SquadInfoPanel").GetComponent<SquadInfoPanel>();
        map = GameObject.Find("Map").GetComponent<Map>();
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
    }
    public void OnPointerClick()
    {
        sip.isOpened= false;
        inventory.isInventOpen= false;
        map.NextRoom(id, eventName, locationName, squad);
    }

    public void RoomInfoReceiving(int Id, string EventName, string LocationName, Squad Squad)
    {
        id = Id;
        eventName= EventName;
        locationName = LocationName;
        squad = Squad;
    }
}
