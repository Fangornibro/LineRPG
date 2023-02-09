using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FightManager : MonoBehaviour
{
    //Enemy types
    public List<Squad> squads, events, bosses;
    public Squad elder, baker, chest;
    //Gold
    public Icon goldIcon;
    //Reward list
    public List<Icon> startItems, commonItems, rareItems, epicItems, legendaryItems, eventItems;
    [HideInInspector]
    public bool start = false, startTempChecking = false;
    private DialogueStructure ds;
    [HideInInspector]
    public string curEventString, curLocationString;
    [HideInInspector]
    public Squad curSquad;
    //Room position(for camera)
    [HideInInspector]
    public Vector2 roomPos;
    //Current turn
    private int curTurn = 1;
    //Is enemy still hit/in hit
    [HideInInspector]
    public bool isEnemiesStillHit = false;
    //Start position
    public int startPositionx, startPositiony;
    //Gold sum
    private int Gold = 0;

    //Camera
    private CameraScript cam;
    //Temp
    private Transform temp;
    //Player
    private Player player;
    //Turn text
    private TextMeshProUGUI turnText;
    //Event hud
    private EventHud eh;
    //Bottom panel
    private BottomPanel bp;
    //FightHUD
    private FightHUD fh;
    //Map
    private Map map;
    //Poison sound
    private AudioSource poisonSound;
    public void Start()
    {
        //Camera
        cam = GameObject.Find("Main Camera").GetComponent<CameraScript>();
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
        //FightHUD
        fh = GameObject.Find("FightHUD").GetComponent<FightHUD>();
        //Mapp
        map = GameObject.Find("Map").GetComponent<Map>();
        //Poison sound
        poisonSound = GameObject.Find("poisonSound").GetComponent<AudioSource>();
    }
    public void RoomStart()
    {
        //Camera
        cam.isKinematic = true;
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
            map.gameObject.SetActive(false);
            bp.checkPassive();
            player.FightStart();
            int st = ds.Interaction();
            if (st == 101)
            {
                for (int i = 0; i < startItems.Count; i++)
                {
                    eh.rewardItems.Add(startItems[i]);
                }
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
                eh.rewardItems.Add(eventItems[0]);
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
                    eh.rewardItems.Add(eventItems[0]);
                    eh.Activation("Victory");
                }
            }
            else if (st == 401)
            {
                RewardCalculation();
                eh.Activation("Victory");
            }
            else if (st == 1)
            {
                fh.VisibilityChange(true);
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
                fh.VisibilityChange(true);
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
            RewardCalculation();
            Icon gold = goldIcon;
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
            temp.GetChild(i).GetComponent<Enemy>().SetArmor(0);
        }
        yield return new WaitForSeconds(0.6f);
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
        fh.VisibilityChange(false);
        map.gameObject.SetActive(true);
        for (int i = 0; i < temp.childCount; i++)
        {
            Destroy(temp.GetChild(i).gameObject);
        }
        
        start = false;
        cam.transform.position = new Vector3(roomPos.x, roomPos.y, -15);
        cam.isKinematic = false;
    }

    public void AddGold(int gold)
    {
        Gold += gold;
    }

    public void AddEnemies(List<Enemy> enemies)
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            Enemy curEnemy = Instantiate(enemies[i], new Vector2(startPositionx + (temp.childCount - 1) * 10, (temp.childCount - 1) % 2 == 0 ? startPositiony : startPositiony + 5), Quaternion.Euler(0, 0, 0), temp);
            curEnemy.nextAttack = curEnemy.attacks[Random.Range(1, curEnemy.attacks.Count)];

            curEnemy.transform.Find("AttackIcon").GetComponent<SpriteRenderer>().sprite = curEnemy.nextAttack.attackIcon;
            curEnemy.EffectUpdate();
        }
    }
    private void RewardCalculation()
    {
        if (curSquad.difficult == 0)
        {
            int rand = Random.Range(0, 3);
            if (rand == 0)
            {
                eh.rewardItems.Add(commonItems[Random.Range(0, commonItems.Count)]);
            }
        }
        else if (curSquad.difficult == 1)
        {
            int rand = Random.Range(0, 1);
            if (rand == 0)
            {
                eh.rewardItems.Add(commonItems[Random.Range(0, commonItems.Count)]);
            }
        }
        else if (curSquad.difficult == 2)
        {
            int rand = Random.Range(0, 9);
            if (rand == 0 || rand == 1)
            {
                
            }
            else if (rand == 2)
            {
                eh.rewardItems.Add(rareItems[Random.Range(0, rareItems.Count)]);
            }
            else
            {
                eh.rewardItems.Add(commonItems[Random.Range(0, commonItems.Count)]);
            }
        }
        else if (curSquad.difficult == 3)
        {
            int rand = Random.Range(0, 5);
            if (rand == 0)
            {
                eh.rewardItems.Add(rareItems[Random.Range(0, rareItems.Count)]);
            }
            else
            {
                eh.rewardItems.Add(commonItems[Random.Range(0, commonItems.Count)]);
            }
        }
        else if (curSquad.difficult == 4)
        {
            eh.rewardItems.Add(rareItems[Random.Range(0, rareItems.Count)]);
        }
        else if (curSquad.difficult == 5)
        {
            int rand = Random.Range(0, 9);
            if (rand == 0)
            {
                eh.rewardItems.Add(epicItems[Random.Range(0, epicItems.Count)]);
            }
            else
            {
                eh.rewardItems.Add(rareItems[Random.Range(0, rareItems.Count)]);
            }
        }
        else if (curSquad.difficult >= 6)
        {
            int rand = Random.Range(0, 5);
            if (rand == 0)
            {
                eh.rewardItems.Add(epicItems[Random.Range(0, epicItems.Count)]);
            }
            else
            {
                eh.rewardItems.Add(rareItems[Random.Range(0, rareItems.Count)]);
            }
        }
    }
}
