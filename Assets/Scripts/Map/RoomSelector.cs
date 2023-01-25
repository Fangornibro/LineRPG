using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using static Room;
using static UnityEditor.FilePathAttribute;

public class RoomSelector : MonoBehaviour
{
    private Room lastRoom;
    private SquadInfoPanel sip;
    private Inventory inventory;
    private void Start()
    {
        sip = GameObject.Find("SquadInfoPanel").GetComponent<SquadInfoPanel>();
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape) && lastRoom != null)
        {
            lastRoom.GetComponent<LineRenderer>().SetWidth(0.2f, 0.2f);
            lastRoom.SetColorsToDefault();
            if (lastRoom.state == Statement.unable)
            {
                lastRoom.GetComponent<LineRenderer>().sortingOrder = -3;
            }
            else if (lastRoom.state == Statement.completed)
            {
                lastRoom.GetComponent<LineRenderer>().sortingOrder = -1;
            }
            else if (lastRoom.state == Statement.next)
            {
                lastRoom.GetComponent<LineRenderer>().sortingOrder = -2;
            }
            lastRoom = null;
        }
    }
    public void ChangeSelection(Room selectedRoom)
    {
        if (lastRoom != null)
        {
            lastRoom.GetComponent<LineRenderer>().SetWidth(0.2f, 0.2f);
            lastRoom.SetColorsToDefault();
            if (lastRoom.state == Statement.unable)
            {
                lastRoom.GetComponent<LineRenderer>().sortingOrder = -3;
            }
            else if (lastRoom.state == Statement.completed)
            {
                lastRoom.GetComponent<LineRenderer>().sortingOrder = -1;
            }
            else if (lastRoom.state == Statement.next)
            {
                lastRoom.GetComponent<LineRenderer>().sortingOrder = -2;
            }
        }
        lastRoom = selectedRoom;
        if (lastRoom != null)
        {
            if (lastRoom.state == Statement.unable)
            {
                lastRoom.sr.color = new Color(0.6792453f, 0.6792453f, 0.6792453f);
                lastRoom.GetComponent<LineRenderer>().SetColors(new Color(0.6792453f, 0.6792453f, 0.6792453f), new Color(0.6792453f, 0.6792453f, 0.6792453f));
                sip.isOpened = false;
                inventory.isInventOpen = false;
            }
            else if (lastRoom.state == Statement.completed)
            {
                lastRoom.sr.color = new Color(0.375979f, 0.6698113f, 0.3801175f);
                lastRoom.GetComponent<LineRenderer>().SetColors(new Color(0.375979f, 0.6698113f, 0.3801175f), new Color(0.375979f, 0.6698113f, 0.3801175f));
            }
            else if (lastRoom.state == Statement.next)
            {
                lastRoom.sr.color = new Color(0.9176471f, 0.7137255f, 0.3843137f);
                lastRoom.GetComponent<LineRenderer>().SetColors(new Color(0.9176471f, 0.7137255f, 0.3843137f), new Color(0.9176471f, 0.7137255f, 0.3843137f));
                sip.isOpened = true;
                inventory.isInventOpen = true;
            }
            lastRoom.GetComponent<LineRenderer>().sortingOrder = 0;
            lastRoom.GetComponent<LineRenderer>().SetWidth(0.3f, 0.3f);
        }
    }
}
