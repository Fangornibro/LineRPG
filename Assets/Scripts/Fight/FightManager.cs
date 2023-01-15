using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UIElements;

public class FightManager : MonoBehaviour
{
    //Enemy types
    public List<Enemy> enemies;
    public List<GameObject> events;
    public GameObject oldMan, baker, chest;
    //Reward list
    public List<Icon> rewardItems;
    [HideInInspector]
    public bool start = false, startTempChecking = false;
    public static DialogueStructure ds;
    public DialogueBranch dialoguebranch1, dialoguebranch2, dialoguebranch3, dialoguebranch4, dialoguebranch5, dialoguebranch6, startFight, runAway;
    [HideInInspector]
    public string curEventString, curLocationString;
    //Temp
    private Transform temp;
    //Player
    private Player player;
    //Player icon
    private Sprite playerIcon;
    //Current turn
    private int curTurn = 1;
    //Turn text
    private TextMeshProUGUI turnText;
    //Is enemy still hit/in hit
    [HideInInspector]
    public bool isEnemiesStillHit = false;
    //Event hud
    private EventHud eh;
    //Bottom panel
    private BottomPanel bp;
    //Start position
    public int startPositionx, startPositiony;
    public void Start()
    {
        //Player
        player = GameObject.Find("Player").GetComponent<Player>();
        //Icon
        playerIcon = player.GetComponent<SpriteRenderer>().sprite;
        //Temp
        temp = GameObject.Find("Temp").transform;
        //Turn text
        turnText = GameObject.Find("TurnText").GetComponent<TextMeshProUGUI>();
        //Event hud
        eh = GameObject.Find("EventHud").GetComponent<EventHud>();
        //Bottom panel
        bp = GameObject.Find("VisibleInventory").GetComponent<BottomPanel>();
    }
    public void RoomStart()
    {
        if (curEventString == "FightIcon")
        {
            //Enemy type
            Enemy curEnemy = enemies[Random.Range(0, enemies.Count)];
            //Number of enemies, chance to leave
            int numberOfEnemies = Random.Range(curEnemy.minNumberOfEnemies, curEnemy.maxNumberOfEnemies), chance = numberOfEnemies * curEnemy.chanceToLeave;
            //Start fight dialogue, run away dialogue
            startFight = new DialogueBranch("Me", "There is no other choice.", playerIcon, "(Start fight)", null, null, null, null, null, null, null, 1);
            runAway = new DialogueBranch("Me", "Lucky!", playerIcon, "(Run away)", null, null, null, null, null, null, null, 2);
            //Icon
            Sprite curIcon = curEnemy.GetComponent<SpriteRenderer>().sprite;
            //Spawn
            for (int i = 0; i < numberOfEnemies; i++)
            {
                Instantiate(curEnemy, new Vector2(startPositionx + i * 10, i % 2 == 0 ? startPositiony : startPositiony + 5), Quaternion.Euler(0, 0, 0), temp);
            }
            //Dialogue
            switch (curEnemy.Name)
            {
                case "Inferior Demon":
                    if (chance == 1)
                    {
                        dialoguebranch2 = new DialogueBranch(curEnemy.Name, "Where is he?", curIcon, "Bye Bye!", null, null, null, runAway, null, null, null, 0);
                    }
                    else
                    {
                        dialoguebranch2 = new DialogueBranch(curEnemy.Name, "Stop!", curIcon, "What a sticky demon.", null, null, null, startFight, null, null, null, 0);
                    }
                    dialoguebranch1 = new DialogueBranch(curEnemy.Name, "Human!", curIcon, "Now you will die!", "Not today.", null, null, startFight, dialoguebranch2, null, null, 0);
                    break;
                case "Petty Demon":
                    if (chance == 1)
                    {
                        dialoguebranch2 = new DialogueBranch(curEnemy.Name, "He ran away...", curIcon, "Bye Bye!", null, null, null, runAway, null, null, null, 0);
                    }
                    else
                    {
                        dialoguebranch2 = new DialogueBranch(curEnemy.Name, "Not so fast!", curIcon, "What a sticky demon.", null, null, null, startFight, null, null, null, 0);
                    }
                    dialoguebranch1 = new DialogueBranch(curEnemy.Name, "A new victim has arrived!", curIcon, "Now you will die!", "Not today.", null, null, startFight, dialoguebranch2, null, null, 0);
                    break;
                case "Bloodhound":
                    if (chance == 1)
                    {
                        dialoguebranch2 = new DialogueBranch(curEnemy.Name, "Woof, woof, woof!", curIcon, "Bye Bye!", null, null, null, runAway, null, null, null, 0);
                    }
                    else
                    {
                        dialoguebranch2 = new DialogueBranch(curEnemy.Name, "Woof!", curIcon, "What a sticky demon.", null, null, null, startFight, null, null, null, 0);
                    }
                    dialoguebranch1 = new DialogueBranch(curEnemy.Name, "Woof, woof!", curIcon, "Now you will die!", "Not today.", null, null, startFight, dialoguebranch2, null, null, 0);
                    break;
                case "Middle Demon":
                    dialoguebranch3 = new DialogueBranch("Me", "He got me!", playerIcon, "(Take damage)", null, null, null, null, null, null, null, 3);
                    if (chance == 1)
                    {
                        dialoguebranch2 = new DialogueBranch(curEnemy.Name, "More victims will come...", curIcon, "Creepy.", null, null, null, runAway, null, null, null, 0);
                    }
                    else
                    {
                        dialoguebranch2 = new DialogueBranch(curEnemy.Name, "I won't let you!", curIcon, "Graaah!", null, null, null, dialoguebranch3, null, null, null, 0);
                    }
                    dialoguebranch1 = new DialogueBranch(curEnemy.Name, "All mankind must die for the needs of the king.", curIcon, "Now you will die!", "Not today.", null, null, startFight, dialoguebranch2, null, null, 0);
                    break;
                case "Eyeball":
                    if (chance == 1)
                    {
                        dialoguebranch2 = new DialogueBranch(curEnemy.Name, "*Stares*", curIcon, "*Gone*", null, null, null, runAway, null, null, null, 0);
                    }
                    else
                    {
                        dialoguebranch2 = new DialogueBranch(curEnemy.Name, "*Stares*", curIcon, "Can't hide from you.", null, null, null, startFight, null, null, null, 0);
                    }
                    dialoguebranch1 = new DialogueBranch(curEnemy.Name, "*Stares*", curIcon, "Now you will die!", "Not today.", null, null, startFight, dialoguebranch2, null, null, 0);
                    break;
            }
        }
        else if (curEventString == "QuestionIcon")
        {
            int randEvent = Random.Range(0, events.Count);
            if (events.Count == 0)
            {
                //Back to map
                eh.Activation("Running away");
                ds = null;
                return;
            }
            int chance1, chance2;
            //Event selection
            GameObject curEventPerson = events[randEvent];
            events.RemoveAt(randEvent);
            //Icon
            Sprite curIcon = curEventPerson.GetComponent<SpriteRenderer>().sprite;
            //Spawn
            Instantiate(curEventPerson, new Vector2(startPositionx, startPositiony), Quaternion.Euler(0, 0, 0), temp);
            switch (curEventPerson.name)
            {
                case "Vampire":
                    {
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
                        break;
                    }
            }
        }
        else if (curEventString == "HouseIcon")
        {
            //Icon
            Sprite curIcon = oldMan.GetComponent<SpriteRenderer>().sprite;
            Instantiate(oldMan, new Vector2(startPositionx, startPositiony), Quaternion.Euler(0, 0, 0), temp);
            dialoguebranch2 = new DialogueBranch("Old Man", "No problem. I'll teach you a couple tricks.", curIcon, "Thanks... I guess.", null, null, null, null, null, null, null, 1);
            dialoguebranch1 = new DialogueBranch("Old Man", "Woke up? There is no time to lie down, it's time to go into battle.", curIcon, "But I can't fight at all.", null, null, null, dialoguebranch2, null, null, null, 0);
        }
        else if (curEventString == "BreadIcon")
        {
            int chance;
            //Icon
            Sprite curIcon = baker.GetComponent<SpriteRenderer>().sprite;
            Instantiate(baker, new Vector2(startPositionx, startPositiony), Quaternion.Euler(0, 0, 0), temp);
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
        else if (curEventString == "ChestIcon")
        {
            //Icon
            Sprite curIcon = chest.GetComponent<SpriteRenderer>().sprite;
            Instantiate(chest, new Vector2(startPositionx, startPositiony), Quaternion.Euler(0, 0, 0), temp);
            dialoguebranch3 = new DialogueBranch("Me", "Unjustified risk.", playerIcon, "(Leave)", null, null, null, null, null, null, null, 1);
            dialoguebranch2 = new DialogueBranch("Me", "Opening...", curIcon, "(Loot)", null, null, null, null, null, null, null, 2);
            dialoguebranch1 = new DialogueBranch("Me", "Is this a chest? Looks suspicious.", playerIcon, "Open chest.", "Better get out of here.", null, null, dialoguebranch2, dialoguebranch3, null, null, 0);
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
            bp.checkPassive();
            player.FightStart();
            if (curEventString == "FightIcon")
            {
                int st = ds.Interaction();
                if (st == 1)
                {
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
                else if (st == 3)
                {
                    player.GetHit(5, false, EnemyAttack.Effect.none, "Middle Demon");
                    curTurn = 1;
                    turnText.text = "Turn " + curTurn.ToString();
                    AllEnemiesPrepareHit();
                    startTempChecking = true;
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
                        player.GetHit(Mathf.RoundToInt(player.maxHP / 4), false, EnemyAttack.Effect.none, "Vampire");
                        eh.Activation("Running away");
                    }
                    else if (st == 4)
                    {
                        player.GetHit(Mathf.RoundToInt(player.maxHP / 4), false, EnemyAttack.Effect.none, "Vampire");
                        eh.rewardItems.Add(rewardItems[2]);
                        eh.Activation("Victory");
                    }
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
            else if (curEventString == "ChestIcon")
            {
                int st = ds.Interaction();
                if (st == 1)
                {
                    eh.Activation("Running away");
                }
                if (st == 2)
                {
                    eh.rewardItems.Add(rewardItems[Random.Range(3, rewardItems.Count)]);
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
            eh.rewardItems.Add(rewardItems[Random.Range(3, rewardItems.Count)]);
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
            if (!curEnemy.death && curEnemy.nextAttack != null)
            {
                yield return new WaitForSeconds(0.6f);
                curEnemy.Hit();
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
                if (curEnemy.poison > 0)
                {
                    curEnemy.GetHit(3, false);
                    curEnemy.poison--;
                }
            }
            if (!curEnemy.death)
            {
                curEnemy.nextAttack = curEnemy.attacks[Random.Range(0, curEnemy.attacks.Count)];
                curEnemy.transform.Find("AttackIcon").GetComponent<SpriteRenderer>().sprite = curEnemy.nextAttack.attackIcon;
                curEnemy.EffectUpdate();
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
