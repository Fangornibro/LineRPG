using UnityEngine;
using static Room;

public class RoomSelector : MonoBehaviour
{
    private Room lastRoom;
    [SerializeField] private GameManager uiManager;
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
            uiManager.InventoryVisible(false);
            uiManager.SquadInfoInvisible();
            if (lastRoom.state == Statement.unable)
            {
                color = new Color(0.5943396f, 0.5943396f, 0.5943396f);
            }
            else if (lastRoom.state == Statement.completed)
            {
                color = new Color(0.375979f, 0.6698113f, 0.3801175f);
            }
            else if (lastRoom.state == Statement.next)
            {
                color = new Color(0.9176471f, 0.7137255f, 0.3843137f);
            }
            LineRenderer line = lastRoom.GetComponent<LineRenderer>();
            lastRoom.transform.Find("Outline").GetComponent<SpriteRenderer>().color = color;
            line.startColor = color;
            line.endColor = color;
            line.sortingOrder = 0;
            line.startWidth = 0.3f;
            line.endWidth = 0.3f;
        }
    }
}
