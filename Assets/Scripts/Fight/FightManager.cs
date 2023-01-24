using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FightManager : MonoBehaviour
{
    //Enemy types
    public List<Squad> squads, events, bosses;
    public Squad elder, baker, chest;
    //Reward list
    public List<Icon> rewardItems;
    [HideInInspector]
    public bool start = false, startTempChecking = false;
    private DialogueStructure ds;
    [HideInInspector]
    public string curEventString, curLocationString;
    [HideInInspector]
    public Squad curSquad;
    //Temp
    private Transform temp;
    //Player
    private Player player;
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
    //Gold sum
    private int Gold = 0;
    private AudioSource poisonSound;
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
        //Bottom panel
        bp = GameObject.Find("VisibleInventory").GetComponent<BottomPanel>();
        //Poison sound
        poisonSound = GameObject.Find("poisonSound").GetComponent<AudioSource>();
    }
    public void RoomStart()
    {
        //switch (curEnemy.Name)
        //{
        //    case "Inferior Demon":
        //        if (chance == 1)
        //        {
        //            dialoguebranch2 = new DialogueBranch(curEnemy.Name, "Where is he?", curIcon, "Bye Bye!", null, null, null, runAway, null, null, null, 0);
        //        }
        //        else
        //        {
        //            dialoguebranch2 = new DialogueBranch(curEnemy.Name, "Stop!", curIcon, "What a sticky demon.", null, null, null, startFight, null, null, null, 0);
        //        }
        //        dialoguebranch1 = new DialogueBranch(curEnemy.Name, "Human!", curIcon, "Now you will die!", "Not today.", null, null, startFight, dialoguebranch2, null, null, 0);
        //        break;
        //    case "Petty Demon":
        //        if (chance == 1)
        //        {
        //            dialoguebranch2 = new DialogueBranch(curEnemy.Name, "He ran away...", curIcon, "Bye Bye!", null, null, null, runAway, null, null, null, 0);
        //        }
        //        else
        //        {
        //            dialoguebranch2 = new DialogueBranch(curEnemy.Name, "Not so fast!", curIcon, "What a sticky demon.", null, null, null, startFight, null, null, null, 0);
        //        }
        //        dialoguebranch1 = new DialogueBranch(curEnemy.Name, "A new victim has arrived!", curIcon, "Now you will die!", "Not today.", null, null, startFight, dialoguebranch2, null, null, 0);
        //        break;
        //    case "Eyeball":
        //        if (chance == 1)
        //        {
        //            dialoguebranch2 = new DialogueBranch(curEnemy.Name, "*Stares*", curIcon, "*Gone*", null, null, null, runAway, null, null, null, 0);
        //        }
        //        else
        //        {
        //            dialoguebranch2 = new DialogueBranch(curEnemy.Name, "*Stares*", curIcon, "Can't hide from you.", null, null, null, startFight, null, null, null, 0);
        //        }
        //        dialoguebranch1 = new DialogueBranch(curEnemy.Name, "*Stares*", curIcon, "Now you will die!", "Not today.", null, null, startFight, dialoguebranch2, null, null, 0);
        //        break;
        //}
        if (curSquad.squadName == "Baker")
        {
            curSquad.dialogues[0].choice1text = "Yeah sure(Pay " + ((player.maxHP-player.HP) * 3) + " gold.)";
            if (player.gold < (player.maxHP - player.HP) * 3)
            {
                curSquad.dialogues[0].button1active = false;
            }
            else
            {
                curSquad.dialogues[0].button1active = true;
            }
        }
        else if (curSquad.dialogues[0].person == "Vampire")
        {
            curSquad.dialogues[0].choice2text = "Let the vampire drink your blood (" + player.maxHP/4 + " hp)";        
        }
        //Selection
        List<GameObject> selectedEnemies = curSquad.enemies[Random.Range(0, curSquad.enemies.Count)].myList;
        //Spawn
        for (int i = 0; i < selectedEnemies.Count; i++)
        {
            Instantiate(selectedEnemies[i], new Vector2(startPositionx + i * 10, i % 2 == 0 ? startPositiony : startPositiony + 5), Quaternion.Euler(0, 0, 0), temp);
        }
        //Dialogue
        ds = new DialogueStructure(curSquad.dialogues);
    }
    public void Update()
    {
        if (start)
        {
            bp.checkPassive();
            player.FightStart();
            int st = ds.Interaction();
            if (st == 101)
            {
                eh.rewardItems.Add(rewardItems[1]);
                eh.rewardItems.Add(rewardItems[2]);
                eh.Activation("Victory");
            }
            else if (st == 201)
            {
                eh.Activation("Leaving");
            }
            else if (st == 202)
            {
                player.GetHeal(player.maxHP/4);
                eh.Activation("Leaving");
            }
            else if (st == 203)
            {
                player.AddGold(-((player.maxHP - player.HP) * 3));
                player.GetHeal(player.maxHP - player.HP);
                eh.Activation("Leaving");
            }
            else if (st == 204)
            {
                player.AddGold(-player.gold / 5);
                eh.Activation("Leaving");
            }
            else if (st == 205)
            {
                curSquad.dialogues[5].text = "He stole " + player.gold/ 5 + " coins from me.";
            }
            else if (st == 301)
            {
                eh.Activation("Leaving");
            }
            else if (st == 302)
            {
                eh.rewardItems.Add(rewardItems[3]);
                eh.Activation("Victory");
            }
            else if (st == 303)
            {
                player.GetHit(player.maxHP / 4, false, EnemyAttack.Effect.none, "Vampire");
                if (player.HP > 0)
                {
                    eh.Activation("Leaving");
                }
            }
            else if (st == 304)
            {
                player.GetHit(player.maxHP / 4, false, EnemyAttack.Effect.none, "Vampire");
                if (player.HP > 0)
                {
                    eh.rewardItems.Add(rewardItems[3]);
                    eh.Activation("Victory");
                }
            }
            else if (st == 401)
            {
                eh.rewardItems.Add(rewardItems[Random.Range(4, rewardItems.Count)]);
                eh.Activation("Victory");
            }
            else if (st == 1)
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
                curTurn = 1;
                turnText.text = "Turn " + curTurn.ToString();
                AllEnemiesPrepareHit();
                startTempChecking = true;
                player.NewTurn();
                StartCoroutine(AllEnemiesHit());
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
            eh.rewardItems.Add(rewardItems[Random.Range(4, rewardItems.Count)]);
            Icon gold = rewardItems[0];
            gold.damageOrArmour = Gold;
            Gold = 0;
            eh.rewardItems.Add(gold);
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
        yield return new WaitForSeconds(0.6f);
        player.armor = 0;
        isEnemiesStillHit = false;
        AllEnemiesPrepareHit();
        player.BarsUpdate();
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
                    poisonSound.Play();
                    curEnemy.GetHit(3, false);
                    curEnemy.poison--;
                }
            }
            if (!curEnemy.death)
            {
                curEnemy.nextAttack = curEnemy.attacks[Random.Range(1, curEnemy.attacks.Count)];
                double thirtyProcent = curEnemy.maxHP * 0.3;
                if (thirtyProcent > 15)
                {
                    thirtyProcent = 15;
                }
                if (curEnemy.HP <= thirtyProcent)
                {
                    if (Random.Range(0, 10) < 3)
                    {
                        curEnemy.nextAttack = curEnemy.attacks[0];
                    }
                }
               
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

    public void AddGold(int gold)
    {
        Gold += gold;
    }
}
