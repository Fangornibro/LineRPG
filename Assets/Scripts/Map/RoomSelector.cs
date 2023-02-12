using UnityEngine;
using static Room;

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
            lastRoom.SetColorsToDefault();
            lastRoom = null;
        }
    }
    public void ChangeSelection(Room selectedRoom)
    {
        if (lastRoom != null)
        {
            lastRoom.SetColorsToDefault();
        }
        lastRoom = selectedRoom;
        if (lastRoom != null)
        {
            Color color = Color.white;
            if (lastRoom.state == Statement.unable)
            {
                color = new Color(0.5943396f, 0.5943396f, 0.5943396f);
                sip.isOpened = false;
                inventory.isInventOpen = false;
            }
            else if (lastRoom.state == Statement.completed)
            {
                color = new Color(0.375979f, 0.6698113f, 0.3801175f);
            }
            else if (lastRoom.state == Statement.next)
            {
                color = new Color(0.9176471f, 0.7137255f, 0.3843137f);
                sip.isOpened = true;
                inventory.isInventOpen = true;
            }
            lastRoom.transform.Find("Outline").GetComponent<SpriteRenderer>().color = color;
            lastRoom.GetComponent<LineRenderer>().SetColors(color, color);
            lastRoom.GetComponent<LineRenderer>().sortingOrder = 0;
            lastRoom.GetComponent<LineRenderer>().SetWidth(0.3f, 0.3f);
        }
    }
}
