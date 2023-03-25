using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Item : MonoBehaviour
{
    [Header("Stats")]
    public string Name;
    [TextArea(2, 4)] public string description;
    public int damage, cost;
    public float criticalChance, criticalDamage, missChance;
    public AudioSource abilitySound;
    public Action action;

    public enum Rarity { gold, common, rare, epic, legendary }
    public Rarity rarity;
    public enum Effect { none, passivePlusMana, passiveArmorEveryRound, passiveSharpenedWeapon, passiveTerrifyingSmell }
    public Effect effect;
    public enum Type { attack, Buff, passive }
    public Type type;
    public bool isMagic, isAOE;
    [HideInInspector] public bool isTakeable = true;
    public List<Sprite> abilityTypeSprites;


    [Space]
    [Space]
    [Header("Initialisations")]
    public TextMeshProUGUI damageOrArmourText, manaCostText;
    private Inventory inventory;
    private BottomInventory bottomInventory;
    private FightManager fightManager;
    private AbilityOnCursor abilityOnCursor;
    private GameManager gameManager;
    private Player player;
    private Transform temp;

    [HideInInspector] public Cell cell;
    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        player = GameManager.player;
        abilityOnCursor = gameManager.abilityOnCursor;
        inventory = gameManager.inventory;
        bottomInventory = gameManager.bottomInventory;
        fightManager = gameManager.fightManager;
        temp = GameManager.temp;
        damageOrArmourText.SetText(damage.ToString());
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
        else if (effect == Effect.passiveTerrifyingSmell)
        {
            for (int i = 0; i < temp.childCount; i++)
            {
                try
                {
                    Enemy curEnemy = temp.GetChild(i).GetComponent<Enemy>();
                    if (!curEnemy.death)
                    {
                        curEnemy.isTerrifying = true;
                        curEnemy.EffectUpdate();
                    }
                }
                catch { }
            }
        }
    }
    
    public void DropOneItem()
    {
        Destroy(gameObject);
    }

    public void Use()
    {
        if (!fightManager.isEnemiesStillHit && type != Type.passive)
        {
            abilityOnCursor.newCursor(this);
        }
    }

    public void ChangeItemCell(Cell cellToChange)
    {
        if (cellToChange.icon == null)
        {
            if (cellToChange.GetComponent<CellType>().cellType == GetComponent<CellType>().cellType || cellToChange.GetComponent<CellType>().cellType == CellType.Type.Everything)
            {
                cellToChange.icon = transform.GetComponent<Item>().cell.icon;
                foreach (Cell cellinv in inventory.cells)
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
                    transform.SetParent(bottomInventory.transform);
                }
                else
                {
                    transform.SetParent(inventory.transform);
                }
                transform.position = transform.GetComponent<Item>().cell.transform.position;
            }
        }
        else
        {
            if ((cellToChange.GetComponent<CellType>().cellType == GetComponent<CellType>().cellType || cellToChange.GetComponent<CellType>().cellType == CellType.Type.Everything) && (cell.GetComponent<CellType>().cellType == cellToChange.icon.GetComponent<CellType>().cellType || cell.GetComponent<CellType>().cellType == CellType.Type.Everything))
            {
                if (cell.GetComponent<CellType>().cellType == CellType.Type.Usable)
                {
                    cellToChange.icon.transform.SetParent(bottomInventory.transform);
                }
                else
                {
                    cellToChange.icon.transform.SetParent(inventory.transform);
                }
                cellToChange.icon.transform.position = transform.GetComponent<Item>().cell.transform.position;
                Item temp = cellToChange.icon;
                cellToChange.icon = transform.GetComponent<Item>().cell.icon;
                foreach (Cell cellinv in inventory.cells)
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
                    transform.SetParent(bottomInventory.transform);
                }
                else
                {
                    transform.SetParent(inventory.transform);
                }
                transform.position = transform.GetComponent<Item>().cell.transform.position;
            }
        }
    }
}