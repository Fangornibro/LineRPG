using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class BottomPanel : MonoBehaviour
{
    public List<Cell> cells;
    private FightManager fm;
    private Inventory inv;
    private Player player;
    private void Start()
    {
        fm = GameObject.Find("LevelDialogue").GetComponent<FightManager>();
        inv = GameObject.Find("Inventory").GetComponent<Inventory>();
        player = GameObject.Find("Player").GetComponent<Player>();
    }
    void Update()
    {
        foreach (Cell c in cells)
        {
            if (Input.GetKeyDown(c.key) && c.icon != null)
            {
                c.icon.Use();
            }
        }

        if (DialogueStructure.isDialogueOpen || (!fm.startTempChecking && !inv.isInventOpen))
        {
            GetComponent<RectTransform>().anchoredPosition = new Vector2(-3000, -3000);
        }
        else
        {
            GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }
    }

    public void checkPassive()
    {
        player.passiveMana = 0;
        player.passiveDamage = 0;
        player.passiveArmor = 0;
        foreach (Cell c in cells)
        {
            if (c.icon != null)
            {
                if (c.icon.AttackBlockOrPassive == "passive")
                {
                    c.icon.UpdatePassiveItem();
                }
            }
        }
    }
}
