using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UIElements;

public class FightManager : MonoBehaviour
{
    public List<string> fightEvents;
    public List<string> questionEvents;
    //Enemy types
    public List<GameObject> enemies;
    //Reward list
    public List<Item> rewardItems;
    [HideInInspector]
    public bool start = false, startTempChecking = false;
    public static DialogueStructure ds;
    public DialogueBranch dialoguebranch1, dialoguebranch2, dialoguebranch3, dialoguebranch4, dialoguebranch5, dialoguebranch6, startFight, runAway;
    public List<Sprite> Icons;
    public Sprite playerIcon;
    [HideInInspector]
    public string curEventString, curLocationString;
    private Transform temp;
    //Player
    private Player player;
    //Current turn
    private int curTurn = 1;
    private TextMeshProUGUI turnText;
    //Is enemy still hit/in hit
    [HideInInspector]
    public bool isEnemiesStillHit = false;
    //Start level
    public List<Item> StartObjects;
    //Event hud
    EventHud eh;
    public void Start()
    {
        //Player
        player = GameObject.Find("Player").GetComponent<Player>();
        //Temp
        temp = GameObject.Find("Temp").transform;
        //Turn text
        turnText = GameObject.Find("TurnText").GetComponent<TextMeshProUGUI>();
        //Event hud
        eh = GameObject.Find("EventHud").GetComponent<EventHud>();
    }
    public void RoomStart()
    {
        player.NewTurn();
        if (curEventString == "FightIcon")
        {
            Enemy curEnemy = default(Enemy);
            string curFightEvent = fightEvents[Random.Range(0, fightEvents.Count)];
            foreach (GameObject e in enemies)
            {
                if (e.name == curFightEvent)
                {
                    curEnemy = e.GetComponent<Enemy>();
                    break;
                }
            }
            int chance, numberOfEnemies;
            startFight = new DialogueBranch("Me", "There is no other choice.", playerIcon, "(Start fight)", null, null, null, null, null, null, null, 1);
            runAway = new DialogueBranch("Me", "Lucky!", playerIcon, "(Run away)", null, null, null, null, null, null, null, 2);
            Sprite curIcon = default(Sprite);
            foreach (Sprite s in Icons)
            {
                if (s.name == curFightEvent + "Idle1")
                {
                    curIcon = s; 
                    break;
                }
            }
            if (curFightEvent == "InferiorDemon")
            {
                numberOfEnemies = Random.Range(2, 4);
                for (int i = 0; i < numberOfEnemies; i++)
                {
                    Instantiate(curEnemy, new Vector3(150 + i * 10, i % 2 == 0 ? -5 : 0, 0), Quaternion.Euler(0, 0, 0), temp);
                }
                chance = Random.Range(1, 2 * numberOfEnemies);
                if (chance == 1)
                {
                    dialoguebranch2 = new DialogueBranch("Inferior Demon", "Where is he?", curIcon, "Bye Bye!", null, null, null, runAway, null, null, null, 0);
                }
                else
                {
                    dialoguebranch2 = new DialogueBranch("Inferior Demon", "Stop!", curIcon, "What a sticky demon.", null, null, null, startFight, null, null, null, 0);
                }
                dialoguebranch1 = new DialogueBranch("Inferior Demon", "Human!", curIcon, "Now you will die!", "Not today.", null, null, startFight, dialoguebranch2, null, null, 0);
            }
            else if (curFightEvent == "PettyDemon")
            {
                numberOfEnemies = Random.Range(1, 2);
                for (int i = 0; i < numberOfEnemies; i++)
                {
                    Instantiate(curEnemy, new Vector3(150 + i * 10, i % 2 == 0 ? -5 : 0, 0), Quaternion.Euler(0, 0, 0), temp);
                }
                chance = Random.Range(1, 4 * numberOfEnemies);
                if (chance == 1)
                {
                    dialoguebranch2 = new DialogueBranch("Petty Demon", "He ran away...", curIcon, "Bye Bye!", null, null, null, runAway, null, null, null, 0);
                }
                else
                {
                    dialoguebranch2 = new DialogueBranch("Petty Demon", "Not so fast!", curIcon, "What a sticky demon.", null, null, null, startFight, null, null, null, 0);
                }
                dialoguebranch1 = new DialogueBranch("Petty Demon", "A new victim has arrived!", curIcon, "Now you will die!", "Not today.", null, null, startFight, dialoguebranch2, null, null, 0);
            }
            else if (curFightEvent == "Bloodhound")
            {
                numberOfEnemies = 3;
                for (int i = 0; i < numberOfEnemies; i++)
                {
                    Instantiate(curEnemy, new Vector3(150 + i * 10, i % 2 == 0 ? -5 : 0, 0), Quaternion.Euler(0, 0, 0), temp);
                }
                chance = Random.Range(1, 4 * numberOfEnemies);
                if (chance == 1)
                {
                    dialoguebranch2 = new DialogueBranch("Bloodhound", "Woof, woof, woof!", curIcon, "Bye Bye!", null, null, null, runAway, null, null, null, 0);
                }
                else
                {
                    dialoguebranch2 = new DialogueBranch("Bloodhound", "Woof!", curIcon, "What a sticky demon.", null, null, null, startFight, null, null, null, 0);
                }
                dialoguebranch1 = new DialogueBranch("Bloodhound", "Woof, woof!", curIcon, "Now you will die!", "Not today.", null, null, startFight, dialoguebranch2, null, null, 0);
            }
        }
        else if (curEventString == "HouseIcon")
        {
            GameObject OldMan = default;
            foreach (GameObject e in enemies)
            {
                if (e.name == "OldMan")
                {
                    OldMan = e;
                    break;
                }
            }
            Sprite curIcon = default(Sprite);
            foreach (Sprite s in Icons)
            {
                if (s.name == "OldManIdle1")
                {
                    curIcon = s;
                    break;
                }
            }
            Instantiate(OldMan, new Vector3(150, 0, 0), Quaternion.Euler(0, 0, 0), temp);
            dialoguebranch2 = new DialogueBranch("Old Man", "No problem. I'll teach you a couple tricks.", curIcon, "Thanks... I guess.", null, null, null, null, null, null, null, 1);
            dialoguebranch1 = new DialogueBranch("Old Man", "Woke up? There is no time to lie down, it's time to go into battle.", curIcon, "But I can't fight at all.", null, null, null, dialoguebranch2, null, null, null, 0);
        }
        else if (curEventString == "QuestionIcon")
        {
            GameObject curEventPerson = default;
            int randEvent = Random.Range(0, questionEvents.Count);
            if (questionEvents.Count == 0)
            {
                //Back to map
                eh.Activation("Running away");
                ds = null;
                return;
            }
            string curQuestionEvent = questionEvents[randEvent];
            questionEvents.RemoveAt(randEvent);
            int chance1, chance2;
            Sprite curIcon = default(Sprite);
            foreach (Sprite s in Icons)
            {
                if (s.name == curQuestionEvent + "Idle1")
                {
                    curIcon = s;
                    break;
                }
            }
            if (curQuestionEvent == "Vampire")
            {
                foreach (GameObject e in enemies)
                {
                    if (e.name == "Vampire")
                    {
                        curEventPerson = e;
                        break;
                    }
                }
                Instantiate(curEventPerson, new Vector3(150, 0, 0), Quaternion.Euler(0, 0, 0), temp);
                chance1 = Random.Range(1, 3);
                chance2 = Random.Range(1, 6);
                dialoguebranch6 = new DialogueBranch("Me", "See if he has something.", playerIcon, "He has something.(Take reward)", null, null, null, null, null, null, null, 2);
                dialoguebranch5 = new DialogueBranch("Me", "See if he has something.", playerIcon, "His body has disappeared.(Leave)", null, null, null, null, null, null, null, 1);
                dialoguebranch4 = new DialogueBranch("Vampire", "Noooooooo!", curIcon, "I don't care.", null, null, null, null, null, null, null, 1);
                if (chance1 == 1)
                {
                    dialoguebranch3 = new DialogueBranch("Vampire", "Thank you. It was delicious.", curIcon, "He just flew away...", null, null, null, null, null, null, null, 3);
                }
                else
                {
                    dialoguebranch3 = new DialogueBranch("Vampire", "Yummy! Here is your reward for salvation.", curIcon, "Auch!(Take reward)", null, null, null, null, null, null, null, 4);
                }
                if (chance2 == 1)
                {
                    dialoguebranch2 = new DialogueBranch("Vampire", "Why?!", curIcon, "Die!", null, null, null, dialoguebranch6, null, null, null, 0);
                }
                else
                {
                    dialoguebranch2 = new DialogueBranch("Vampire", "Why?!", curIcon, "Die!", null, null, null, dialoguebranch5, null, null, null, 0);
                }
                dialoguebranch1 = new DialogueBranch("Vampire", "Help meeee...", curIcon, "(Finish off the vampire)", "(Let the vampire drink your blood" + "(-" + Mathf.RoundToInt(player.maxHP / 4) + " HP)" + ")", "Leave", null, dialoguebranch2, dialoguebranch3, dialoguebranch4, null, 0);
            }
        }
        else if (curEventString == "BreadIcon")
        {
            GameObject curEventPerson = default;
            int chance;
            foreach (GameObject e in enemies)
            {
                if (e.name == "OldMan")
                {
                    curEventPerson = e;
                    break;
                }
            }
            Sprite curIcon = default(Sprite);
            foreach (Sprite s in Icons)
            {
                if (s.name == "OldManIdle1")
                {
                    curIcon = s;
                    break;
                }
            }
            Instantiate(curEventPerson, new Vector3(150, 0, 0), Quaternion.Euler(0, 0, 0), temp);
            chance = Random.Range(1, 3);
            dialoguebranch5 = new DialogueBranch("Me", "He stole " + 20 + " coins from me.", playerIcon, "What a freak.", null, null, null, null, null, null, null, 4);
            dialoguebranch4 = new DialogueBranch("Baker", "Come again.", curIcon, "Bye.", null, null, null, null, null, null, null, 1);
            if (chance == 1)
            {
                dialoguebranch3 = new DialogueBranch("Baker", "It's okay, I've got some leftover food for you.", curIcon, "Thanks!", null, null, null, null, null, null, null, 2);
            }
            else
            {
                dialoguebranch3 = new DialogueBranch("Baker", "I think you're lying.", curIcon, "Ha?", null, null, null, dialoguebranch5, null, null, null, 0);
            }
            dialoguebranch2 = new DialogueBranch("Baker", "Enjoy your meal!", curIcon, "Thanks.", null, null, null, null, null, null, null, 3);
            dialoguebranch1 = new DialogueBranch("Baker", "Would you like to eat?", curIcon, "Yeah sure(Pay " + 20 + " gold.)", "I do not have money.", "Leave", null, dialoguebranch2, dialoguebranch3, dialoguebranch4, null, 0);
        }
        else
        {
            //Back to map
            eh.Activation("Running away");
        }
        ds = new DialogueStructure(dialoguebranch1);
    }
    public void Update()
    {
        if (start)
        {
            if (curEventString == "FightIcon")
            {
                int st = ds.Interaction();
                if (st == 1)
                {
                    player.SetArmour(0);
                    curTurn = 1;
                    turnText.text = "Turn " + curTurn.ToString();
                    AllEnemiesPrepareHit();
                    startTempChecking = true;
                }
                else if (st == 2)
                {
                    //Back to map
                    eh.Activation("Running away");
                }
            }  
            else if (curEventString == "HouseIcon")
            {
                int st = ds.Interaction();
                if (st == 1)
                {
                    eh.rewardItems.Add(rewardItems[0]);
                    eh.rewardItems.Add(rewardItems[1]);
                    eh.Activation("Victory");
                }
            }
            else if (curEventString == "QuestionIcon")
            {
                if (ds != null)
                {
                    int st = ds.Interaction();
                    if (st == 1)
                    {
                        eh.Activation("Running away");
                    }
                    else if (st == 2)
                    {
                        eh.rewardItems.Add(rewardItems[2]);
                        eh.Activation("Victory");
                    }
                    else if (st == 3)
                    {
                        player.GetHit(Mathf.RoundToInt(player.maxHP / 4), EnemyAttack.Effect.none, "Vampire");
                        eh.Activation("Running away");
                    }
                    else if (st == 4)
                    {
                        player.GetHit(Mathf.RoundToInt(player.maxHP / 4), EnemyAttack.Effect.none, "Vampire");
                        eh.rewardItems.Add(rewardItems[2]);
                        eh.Activation("Victory");
                    }
                }
            }
            else if (curEventString == "BreadIcon")
            {
                int st = ds.Interaction();
                if (st == 1)
                {
                    eh.Activation("Running away");
                }
                else if (st == 2)
                {
                    player.GetHeal(5);
                    eh.Activation("Victory");
                }
                else if (st == 3)
                {
                    //Minus money
                    player.GetHeal(10);
                    eh.Activation("Victory");
                }
                else if (st == 4)
                {
                    //Minus money
                    eh.Activation("Victory");
                }
            }
        }
        if (startTempChecking)
        {
            tempChecking();
        }
    }
    private void tempChecking()
    {
        if (temp.childCount == 0)
        {
            startTempChecking = false;
            //Back to map
            eh.Activation("Victory");
        }
    }
    public void NextTurn()
    {
        curTurn++;
        turnText.text= "Turn " + curTurn.ToString();
        player.NewTurn();
        StartCoroutine(AllEnemiesHit());
    }

    private IEnumerator AllEnemiesHit()
    {
        isEnemiesStillHit = true;
        for (int i = 0; i < temp.childCount; i++)
        {
            Enemy curEnemy = temp.GetChild(i).GetComponent<Enemy>();
            if (!curEnemy.death)
            {
                curEnemy.StartAttackAnimation();
                GameObject.Find(curEnemy.nextAttack.attackSound).GetComponent<AudioSource>().Play();
                if (curEnemy.nextAttack.effect == EnemyAttack.Effect.armorUp)
                {
                    curEnemy.GetArmor(curEnemy.nextAttack.damage);
                }
                else if (curEnemy.nextAttack.effect == EnemyAttack.Effect.flock)
                {
                    for (int j = 0; j < temp.childCount; j++)
                    {
                        Enemy curEnemyForFlock = temp.GetChild(j).GetComponent<Enemy>();
                        curEnemyForFlock.plusDamage++;
                    }
                }
                else
                {
                    curEnemy.Hit();
                }
                yield return new WaitForSeconds(0.6f);
            }
        }
        isEnemiesStillHit = false;
        AllEnemiesPrepareHit();
    }

    private void AllEnemiesPrepareHit()
    {
        for (int i = 0; i < temp.childCount; i++)
        {
            Enemy curEnemy = temp.GetChild(i).GetComponent<Enemy>();
            if (!curEnemy.death)
            {
                curEnemy.nextAttack = curEnemy.attacks[Random.Range(0, curEnemy.attacks.Count)];
                curEnemy.transform.Find("AttackIcon").GetComponent<SpriteRenderer>().sprite = curEnemy.nextAttack.attackIcon;
            }
        }
    }

    public bool IsAllEnemiesStillInHit()
    {
        for (int i = 0; i < temp.childCount; i++)
        {
            Enemy curEnemy = temp.GetChild(i).GetComponent<Enemy>();
            if (curEnemy.death)
            {
                return true;
            }
        }
        return false;
    }


    public void BackToMap()
    {
        for(int i = 0; i < temp.childCount; i++)
        {
            Destroy(temp.GetChild(i).gameObject);
        }
        
        start = false;
        Camera.main.transform.position = new Vector3(13.5f, 0, -15);
    }
}
