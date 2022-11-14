using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class LevelDialogue : MonoBehaviour
{
    public List<string> EnemyTypes = new List<string>() { "InferiorDemon", "PettyDemon", "MediumDemons" };
    [HideInInspector]
    public bool start = false;
    public static DialogueStructure ds;
    public DialogueBranch dialoguebranch1, dialoguebranch2, startFight, runAway;
    public Sprite player, InferiorDemonIcon, PettyDemonIcon;
    public Enemy InferiorDemonPrefab, PettyDemonPrefab;
    [HideInInspector]
    public string curEventString, curLocationString;
    private string enemyType;
    private Transform temp;
    public void Start()
    {
        temp = GameObject.Find("Temp").transform;
        if (curEventString == "FightIcon")
        {
            enemyType = EnemyTypes[Random.Range(0, EnemyTypes.Count)];
            int chance, numberOfEnemies;
            startFight = new DialogueBranch("Me", "There is no other choice.", player, "(Start fight)", null, null, null, null, null, null, null, 1);
            runAway = new DialogueBranch("Me", "Lucky!", player, "(Run away)", null, null, null, null, null, null, null, 2);
            if (enemyType == "InferiorDemon")
            {
                numberOfEnemies = Random.Range(1, 4);
                for (int i = 0; i < numberOfEnemies; i++)
                {
                    GameObject.Instantiate(InferiorDemonPrefab, new Vector3(140 + i * 10, -15, 0), new Quaternion(0, -180, 0, 0).normalized, temp);
                }
                chance = Random.Range(1, 5);
                if (chance == 1)
                {
                    dialoguebranch2 = new DialogueBranch("Inferior Demon", "Where is he?", InferiorDemonIcon, "Bye Bye!", null, null, null, runAway, null, null, null, 0);
                }
                else
                {
                    dialoguebranch2 = new DialogueBranch("Inferior Demon", "Stop!", InferiorDemonIcon, "What a sticky demon.", null, null, null, startFight, null, null, null, 0);
                }
                dialoguebranch1 = new DialogueBranch("Inferior Demon", "Human!", InferiorDemonIcon, "Now you will die!", "Not today.", null, null, startFight, dialoguebranch2, null, null, 0);
                ds = new DialogueStructure(dialoguebranch1);
            }
            else if (enemyType == "PettyDemon")
            {
                numberOfEnemies = Random.Range(1, 3);
                for (int i = 0; i < numberOfEnemies; i++)
                {
                    GameObject.Instantiate(PettyDemonPrefab, new Vector3(140 + i * 10, -15, 0), new Quaternion(0, -180, 0, 0).normalized, temp);
                }
                chance = Random.Range(1, 10);
                if (chance == 1)
                {
                    dialoguebranch2 = new DialogueBranch("Petty Demon", "He ran away...", PettyDemonIcon, "Bye Bye!", null, null, null, runAway, null, null, null, 0);
                }
                else
                {
                    dialoguebranch2 = new DialogueBranch("Petty Demon", "Not so fast!", PettyDemonIcon, "What a sticky demon.", null, null, null, startFight, null, null, null, 0);
                }
                dialoguebranch1 = new DialogueBranch("Petty Demon", "A new victim has arrived!", PettyDemonIcon, "Now you will die!", "Not today.", null, null, startFight, dialoguebranch2, null, null, 0);
                ds = new DialogueStructure(dialoguebranch1);
            }
        }
    }
    public void Update()
    {
        if (start)
        {
            if (curEventString == "FightIcon")
            {
                if (enemyType == "InferiorDemon")
                {
                    int st = ds.Interaction();
                    if (st == 1)
                    {

                    }
                    else if (st == 2)
                    {
                        BackToMap();
                    }
                }
                else if (enemyType == "PettyDemon")
                {
                    int st = ds.Interaction();
                    if (st == 1)
                    {

                    }
                    else if (st == 2)
                    {
                        BackToMap();
                    }
                }
            }
            else
            {
                BackToMap();
            }
        }
    }

    private void BackToMap()
    {
        for(int i = 0; i < temp.childCount; i++)
        {
            GameObject.Destroy(temp.GetChild(i).gameObject);
        }
        
        start = false;
        Camera.main.transform.position = new Vector3(13.5f, 0, -15);
    }
}
