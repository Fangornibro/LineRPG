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
    public Cell cell;
    public string Name, rarity ,description;
    private GameObject inventory;
    public int damageOrArmour, cost;
    public float criticalChance, criticalDamage;
    public Texture2D cursorTexture;
    private FightManager ld;
    public TextMeshProUGUI damageOrArmourText, manaCostText;
    private AbilityOnCursor abilityOnCursor;
    //Sounds
    public AudioSource abilitySound;
    //Effect
    public enum Effect { none, HPSteal, AOE, passivePlusMana, passiveArmorEveryRound, passiveSharpenedWeapon, disarm, armorDestruction, poison }
    public Effect effect;
    //Attack, Block or passive
    public string AttackBlockOrPassive;
    //Player
    private Player player;
    [HideInInspector]
    public bool isTakeable = true;
    private void Start()
    {
        //Player
        player = GameObject.Find("Player").GetComponent<Player>();
        abilityOnCursor = GameObject.Find("AbilityOnCursor").GetComponent<AbilityOnCursor>();
        inventory = GameObject.FindGameObjectWithTag("Inventory");
        ld = GameObject.Find("LevelDialogue").GetComponent<FightManager>();
        damageOrArmourText.SetText(damageOrArmour.ToString());
        manaCostText.SetText(cost.ToString());
    }
    public void UpdatePassiveItem()
    {
        if (effect == Effect.passivePlusMana)
        {
            player.passiveMana++;
        }
        else if (effect == Effect.passiveArmorEveryRound)
        {
            player.passiveArmor += 2;
        }
        else if (effect == Effect.passiveSharpenedWeapon)
        {
            player.maxPassiveDamage += 2;
        }
    }
    

    public void DropOneItem()
    {
        Destroy(gameObject);
    }

    public void Use()
    {
        if (!ld.isEnemiesStillHit && AttackBlockOrPassive != "passive")
        {
            abilityOnCursor.newCursor(damageOrArmour, criticalChance, criticalDamage, cost, cursorTexture, AttackBlockOrPassive, abilitySound, effect);
        }
    }

    public void ChangeItemCell(Cell cellToChange)
    {
        if (cellToChange.icon == null)
        {
            if (cellToChange.GetComponent<CellType>().cellType == GetComponent<CellType>().cellType || cellToChange.GetComponent<CellType>().cellType == CellType.Type.Everything)
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
            if ((cellToChange.GetComponent<CellType>().cellType == GetComponent<CellType>().cellType || cellToChange.GetComponent<CellType>().cellType == CellType.Type.Everything) && (cell.GetComponent<CellType>().cellType == cellToChange.icon.GetComponent<CellType>().cellType || cell.GetComponent<CellType>().cellType == CellType.Type.Everything))
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