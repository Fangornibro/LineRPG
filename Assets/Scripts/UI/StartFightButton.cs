using UnityEngine;


public class StartFightButton : MonoBehaviour, IButton
{
    private SquadInfoPanel sip;
    private Inventory inventory;
    private VisibleInventory visibleInventory;
    private CameraScript cam;

    private bool isStartedAnim = false;
    [HideInInspector]
    public Room room;

    private void Start()
    {
        cam = GameObject.Find("Main Camera").GetComponent<CameraScript>();
        sip = GameObject.Find("SquadInfoPanel").GetComponent<SquadInfoPanel>();
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        visibleInventory = GameObject.Find("VisibleInventory").GetComponent<VisibleInventory>();
    }
    void IButton.OnPointerClick()
    {
        bool canStart = false;
        if (!room.isStart)
        {
            foreach (Cell c in visibleInventory.cells)
            {
                if (c.icon != null && (c.icon.type == Icon.Type.attack || c.icon.type == Icon.Type.magicAttack))
                {
                    canStart = true;
                }
            }
        }
        else
        {
            canStart = true;
        }
        if (canStart)
        {
            isStartedAnim = true;
            sip.isOpened = false;
            inventory.isInventOpen = false;
        }
    }

    private void Update()
    {
        if (isStartedAnim)
        {
            isStartedAnim = cam.Approximation(room.transform.position, 1, true);
        }
    }
}
