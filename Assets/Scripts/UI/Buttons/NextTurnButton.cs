using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NextTurnButton : MonoBehaviour, IButton
{
    private FightManager fm;
    private AudioSource nextTurnSound;
    private void Start()
    {
        fm = GameObject.Find("FightManager").GetComponent<FightManager>();
        nextTurnSound = GameObject.Find("NextTurnSound").GetComponent<AudioSource>();
    }
    void IButton.OnPointerClick()
    {
        if (!fm.isEnemiesStillHit && !fm.IsAllEnemiesStillInHit())
        {
            nextTurnSound.Play();
            fm.NextTurn();
        }
    }
}
