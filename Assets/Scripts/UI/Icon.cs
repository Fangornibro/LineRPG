using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine.EventSystems;

public class Icon : MonoBehaviour
{
    public Item item;
    public Cell cell;
    public string name, itemType ,description;
    private GameObject inventory;
    public int damageOrArmour, cost;
    public Texture2D cursorTexture;
    private FightManager ld;
    public TextMeshProUGUI damageOrArmourText, manaCostText;
    private AbilityOnCursor abilityOnCursor;
    //Sounds
    public AudioSource abilitySound;
    //Effect
    public enum Effect { none, HPSteal }
    public Effect effect;
    private void Start()
    {
        abilityOnCursor = GameObject.Find("AbilityOnCursor").GetComponent<AbilityOnCursor>();
        inventory = GameObject.FindGameObjectWithTag("Inventory");
        ld = GameObject.Find("LevelDialogue").GetComponent<FightManager>();
        damageOrArmourText.SetText(damageOrArmour.ToString());
        manaCostText.SetText(cost.ToString());
    }

    public void DropOneItem()
    {
        Destroy(gameObject);
    }

    public void Use()
    {
        if (!ld.isEnemiesStillHit)
        {
            if (item.Name == "Regular Punch")
            {
                abilityOnCursor.newCursor(damageOrArmour, cost, cursorTexture, "attack", abilitySound, effect);
            }
            else if (item.Name == "Regular Block")
            {
                abilityOnCursor.newCursor(damageOrArmour, cost, cursorTexture, "block", abilitySound, effect);
            }
            else if (item.Name == "Blood Sucking")
            {
                abilityOnCursor.newCursor(damageOrArmour, cost, cursorTexture, "attack", abilitySound, effect);
            }
        }
    }

    public void ChangeItemCell(Cell cellToChange)
    {
        if (cellToChange.icon == null)
        {
            if (cellToChange.GetComponent<CellType>().cellType == item.GetComponent<CellType>().cellType || cellToChange.GetComponent<CellType>().cellType == CellType.Type.Everything)
            {
                cellToChange.icon = transform.GetComponent<Icon>().cell.icon;
                foreach (Cell cellinv in inventory.GetComponent<Inventory>().cells)
                {
                    if (cellinv == cell)
                    {
                        cellinv.icon = null;
                        break;
                    }
                }
                cell = cellToChange; 
                if (cell.GetComponent<CellType>().cellType == CellType.Type.Usable)
                {
                    transform.SetParent(GameObject.Find("VisibleInventory").transform);
                }
                else
                {
                    transform.SetParent(GameObject.Find("Inventory").transform);
                }
                transform.position = transform.GetComponent<Icon>().cell.transform.position;
            }
        }
        else
        {
            if ((cellToChange.GetComponent<CellType>().cellType == item.GetComponent<CellType>().cellType || cellToChange.GetComponent<CellType>().cellType == CellType.Type.Everything) && (cell.GetComponent<CellType>().cellType == cellToChange.icon.item.GetComponent<CellType>().cellType || cell.GetComponent<CellType>().cellType == CellType.Type.Everything))
            {
                if (cell.GetComponent<CellType>().cellType == CellType.Type.Usable)
                {
                    cellToChange.icon.transform.SetParent(GameObject.Find("VisibleInventory").transform);
                }
                else
                {
                    cellToChange.icon.transform.SetParent(GameObject.Find("Inventory").transform);
                }
                cellToChange.icon.transform.position = transform.position;
                Icon temp = cellToChange.icon;
                cellToChange.icon = transform.GetComponent<Icon>().cell.icon;
                foreach (Cell cellinv in inventory.GetComponent<Inventory>().cells)
                {
                    if (cellinv == cell)
                    {
                        cellinv.icon = temp;
                        cellinv.icon.cell = cellinv;
                        break;
                    }
                }
                cell = cellToChange;
                if (cell.GetComponent<CellType>().cellType == CellType.Type.Usable)
                {
                    transform.SetParent(GameObject.Find("VisibleInventory").transform);
                }
                else
                {
                    transform.SetParent(GameObject.Find("Inventory").transform);
                }
                transform.position = transform.GetComponent<Icon>().cell.transform.position;
            }
        }
    }
}