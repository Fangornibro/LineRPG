using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestScript : MonoBehaviour
{
    private FightManager fightManager;
    [SerializeField] private Squad mimic, somethingInTheBox;
    private void Start()
    {
        fightManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().fightManager;
    }
    public void DestroyChest()
    {
        Destroy(gameObject);
    }
    public void SpawnMimic()
    {
        fightManager.SpawnSquad(mimic);
    }

    public void SpawnSomethingInTheBox()
    {
        fightManager.SpawnSquad(somethingInTheBox);
    }
}
