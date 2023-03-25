using UnityEngine;


public class StartFightButton : MonoBehaviour, IButton
{
    [HideInInspector]
    public Room room;


    [Header("Initialisations")]
    [SerializeField] private BottomInventory bottomInventory;
    [SerializeField] private CameraScript cameraScript;
    [SerializeField] private GameManager uiManager;

    void IButton.OnPointerClick()
    {
        bool canStart = false;
        if (!room.isStart)
        {
            foreach (Cell c in bottomInventory.cells)
            {
                if (c.icon != null && (c.icon.type == Item.Type.attack))
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
            cameraScript.StartApproximationCoroutine(room.transform.position, 1, true);
            uiManager.SquadInfoInvisible();
            uiManager.InventoryVisible(false);
            uiManager.FocusPanelVisible(false);
        }
        else
        {
            uiManager.NoAttackSkillVisible(true);
        }
    }
}
