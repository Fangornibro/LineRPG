using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FightManager : MonoBehaviour
{
    [HideInInspector] public bool start = false, startTempChecking = false;
    //Location name
    [HideInInspector] public string curLocationName;
    //Current squad
    [HideInInspector] public Squad curSquad;
    //Room position(for camera)
    [HideInInspector] public Vector2 roomPos;
    //Current turn
    private int curTurn = 1;
    //Is enemy still hit/in hit
    [HideInInspector] public bool isEnemiesStillHit = false;
    //Gold sum
    private int Gold = 0;

    [Header("All characters variables")]
    public List<Squad> squads;
    public List<Squad> events;
    public List<Squad> bosses;
    public Squad elder, baker, chest;


    [Space]
    [Space]
    [Header("Rewards")]
    [SerializeField] private Icon goldIcon;
    [SerializeField] private List<Icon> commonItems, rareItems, epicItems, legendaryItems, eventItems;
    private List<Icon> startItems;


    [Space]
    [Space]
    [Header("Start enemy spawn positions")]
    [SerializeField] private int startPositionx;
    [SerializeField] private int startPositiony;


    [Space]
    [Space]
    [Header("Initialisations")]
    [SerializeField] private EventHUD eventHUD;
    [SerializeField] private VisibleInventory visibleInventory;
    [SerializeField] private FightHUD fightHUD;
    [SerializeField] private Map map;
    [SerializeField] private Player player;
    [SerializeField] private Transform temp;
    [SerializeField] private CameraScript cam;
    [SerializeField] private DialogueStructure dialogueStructure;


    [Space]
    [Space]
    [Header("Sounds")]
    [SerializeField] private AudioSource poisonSound;


    [Space]
    [Space]
    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI turnText;
    private void Start()
    {
        startItems = SelectCharacterButton.curPlayer.startItems;
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
        dialogueStructure.Initialization(curSquad.dialogues);
    }
    public void Update()
    {
        if (start)
        {
            map.gameObject.SetActive(false);
            visibleInventory.checkPassive();
            player.FightStart();
            int st = dialogueStructure.Interaction();
            if (st == 101)
            {
                for (int i = 0; i < startItems.Count; i++)
                {
                    eventHUD.rewardItems.Add(startItems[i]);
                }
                eventHUD.Activation("Victory");
            }
            else if (st == 201)
            {
                eventHUD.Activation("Leaving");
            }
            else if (st == 202)
            {
                player.GetHeal(player.maxHP/4);
                eventHUD.Activation("Leaving");
            }
            else if (st == 203)
            {
                player.AddGold(-((player.maxHP - player.HP) * 3));
                player.GetHeal(player.maxHP - player.HP);
                eventHUD.Activation("Leaving");
            }
            else if (st == 204)
            {
                player.AddGold(-player.gold / 5);
                eventHUD.Activation("Leaving");
            }
            else if (st == 205)
            {
                curSquad.dialogues[5].text = "He stole " + player.gold/ 5 + " coins from me.";
            }
            else if (st == 301)
            {
                eventHUD.Activation("Leaving");
            }
            else if (st == 302)
            {
                eventHUD.rewardItems.Add(eventItems[0]);
                eventHUD.Activation("Victory");
            }
            else if (st == 303)
            {
                player.GetHit(player.maxHP / 4, false, EnemyAttack.Effect.none, "Vampire");
                if (player.HP > 0)
                {
                    eventHUD.Activation("Leaving");
                }
            }
            else if (st == 304)
            {
                player.GetHit(player.maxHP / 4, false, EnemyAttack.Effect.none, "Vampire");
                if (player.HP > 0)
                {
                    eventHUD.rewardItems.Add(eventItems[0]);
                    eventHUD.Activation("Victory");
                }
            }
            else if (st == 401)
            {
                RewardCalculation();
                eventHUD.Activation("Victory");
            }
            else if (st == 1)
            {
                fightHUD.VisibilityChange(true);
                curTurn = 1;
                turnText.text = "Turn " + curTurn.ToString();
                AllEnemiesPrepareHit();
                startTempChecking = true;
            }
            else if (st == 2)
            {
                //Back to map
                eventHUD.Activation("Running away");
            }
            else if (st == 3)
            {
                fightHUD.VisibilityChange(true);
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
            eventHUD.rewardItems.Add(gold);
            eventHUD.Activation("Victory");
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
                    curEnemy.GetHit(3 * curEnemy.poison, false);
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
        fightHUD.VisibilityChange(false);
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

    public void AddEnemies(List<Enemy> enemies, List<Vector3> positions, Vector3 pos)
    {
        for (int i = 0; i < positions.Count; i++)
        {
            Enemy curEnemy = Instantiate(enemies[i], positions[i] + pos, Quaternion.Euler(0, 0, 0), temp);

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
                eventHUD.rewardItems.Add(commonItems[Random.Range(0, commonItems.Count)]);
            }
        }
        else if (curSquad.difficult == 1)
        {
            int rand = Random.Range(0, 1);
            if (rand == 0)
            {
                eventHUD.rewardItems.Add(commonItems[Random.Range(0, commonItems.Count)]);
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
                eventHUD.rewardItems.Add(rareItems[Random.Range(0, rareItems.Count)]);
            }
            else
            {
                eventHUD.rewardItems.Add(commonItems[Random.Range(0, commonItems.Count)]);
            }
        }
        else if (curSquad.difficult == 3)
        {
            int rand = Random.Range(0, 5);
            if (rand == 0)
            {
                eventHUD.rewardItems.Add(rareItems[Random.Range(0, rareItems.Count)]);
            }
            else
            {
                eventHUD.rewardItems.Add(commonItems[Random.Range(0, commonItems.Count)]);
            }
        }
        else if (curSquad.difficult == 4)
        {
            eventHUD.rewardItems.Add(rareItems[Random.Range(0, rareItems.Count)]);
        }
        else if (curSquad.difficult == 5)
        {
            int rand = Random.Range(0, 9);
            if (rand == 0)
            {
                eventHUD.rewardItems.Add(epicItems[Random.Range(0, epicItems.Count)]);
            }
            else
            {
                eventHUD.rewardItems.Add(rareItems[Random.Range(0, rareItems.Count)]);
            }
        }
        else if (curSquad.difficult >= 6)
        {
            int rand = Random.Range(0, 5);
            if (rand == 0)
            {
                eventHUD.rewardItems.Add(epicItems[Random.Range(0, epicItems.Count)]);
            }
            else
            {
                eventHUD.rewardItems.Add(rareItems[Random.Range(0, rareItems.Count)]);
            }
        }
    }
}
