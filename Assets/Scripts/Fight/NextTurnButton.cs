using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NextTurnButton : MonoBehaviour
{
    private FightManager fm;
    private AudioSource nextTurnSound;
    private void Start()
    {
        fm = GameObject.Find("FightManager").GetComponent<FightManager>();
        nextTurnSound = GameObject.Find("NextTurnSound").GetComponent<AudioSource>();
    }
    public void OnPointerClick()
    {
        if (!fm.isEnemiesStillHit && !fm.IsAllEnemiesStillInHit())
        {
            nextTurnSound.Play();
            fm.NextTurn();
        }
    }
}
