using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NextTurnButton : MonoBehaviour, IPointerClickHandler
{
    private FightManager fm;
    private void Start()
    {
        fm = GameObject.Find("LevelDialogue").GetComponent<FightManager>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!fm.isEnemiesStillHit && !fm.IsAllEnemiesStillInHit())
        {
            fm.NextTurn();
        }
    }
}
