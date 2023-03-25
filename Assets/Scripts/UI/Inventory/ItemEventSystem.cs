using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemEventSystem : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private Inventory inventory;
    private FightManager fightManager;
    private Player player;
    private GameManager gameManager;
    private Item item;

    private AudioSource buttonDownSound, buttonUpSound, coinSound;

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        fightManager = gameManager.fightManager;
        inventory = gameManager.inventory;
        player = GameManager.player;

        buttonDownSound = gameManager.buttonDownSound;
        buttonUpSound = gameManager.buttonUpSound;
        coinSound = gameManager.coinSound;

        item = GetComponent<Item>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            takeAndUse();
        }
    }

    public void takeAndUse()
    {
        if (item.isTakeable)
        {
            if (GetComponent<CellType>().cellType == CellType.Type.CoinBag)
            {
                coinSound.Play();
                player.AddGold(Convert.ToInt32(item.damageOrArmourText.text));
                item.DropOneItem();
                gameManager.ContextMenuInvisible();
            }
            else
            {
                foreach (Cell cell in inventory.GetComponent<Inventory>().cells)
                {
                    if (cell.icon == null)
                    {
                        item.cell = cell;
                        item.transform.SetParent(item.cell.transform.parent);
                        item.GetComponent<RectTransform>().anchoredPosition = cell.GetComponent<RectTransform>().anchoredPosition + new Vector2(-4.5f, 4.5f);
                        cell.icon = item;
                        item.transform.localScale = Vector3.one;
                        item.isTakeable = false;
                        gameManager.ContextMenuInvisible();
                        break;
                    }
                }
            }
        }
        else
        {
            if (item.cell.GetComponent<CellType>().cellType == CellType.Type.Usable && !inventory.isActiveAndEnabled)
            {
                item.Use();
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            buttonDownSound.Play();
            transform.GetComponent<Image>().color = new Color(0.3584906f, 0.3584906f, 0.3584906f, 1);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            buttonUpSound.Play();
            transform.GetComponent<Image>().color = new Color(0.6603774f, 0.6603774f, 0.6603774f, 1);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.GetComponent<Image>().color = Color.white;
        Item icon = transform.GetComponent<Item>();
        gameManager.ContextMenuVisible(icon.Name, icon.rarity, icon.description, transform.position, icon.abilityTypeSprites);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.GetComponent<Image>().color = new Color(0.6603774f, 0.6603774f, 0.6603774f, 1);
        gameManager.ContextMenuInvisible();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!fightManager.startTempChecking && inventory.isActiveAndEnabled)
        {
            gameManager.ContextMenuInvisible();
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                transform.position = eventData.pointerCurrentRaycast.screenPosition;
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!fightManager.startTempChecking)
        {
            transform.SetParent(GameObject.Find("Canvas").transform);
            if (inventory.isActiveAndEnabled)
            {
                gameManager.ContextMenuInvisible();
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!fightManager.startTempChecking && inventory.isActiveAndEnabled)
        {
            foreach (Cell cellinv in inventory.cells)
            {
                if (cellinv.GetComponent<Collider2D>().Distance(GetComponent<Collider2D>()).distance < cellinv.GetComponent<RectTransform>().sizeDelta.x / 4)
                {
                    item.ChangeItemCell(cellinv);
                    gameManager.ContextMenuVisible(item.Name, item.rarity, item.description, transform.position, item.abilityTypeSprites);
                }
                else
                {
                    transform.position = item.cell.transform.position;
                    transform.SetParent(item.cell.transform.parent);
                }
            }
        }
    }
}
