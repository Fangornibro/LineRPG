using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class FightManager : MonoBehaviour
{
    //Is enemy still hit/in hit    And checking if all enemies Die
    [HideInInspector] public bool isEnemiesStillHit = false, startTempChecking = false;
    //Global difficult
    [HideInInspector] public int globalDifficult;

    [Header("All characters variables")]
    public List<Squad> squads;
    public List<Squad> events;
    public List<Squad> bosses;
    public List<Squad> chests;
    public Squad elder, baker, tutorial1;
    [HideInInspector] public Squad curSquad;
    [SerializeField] private List<Squad> cursedSquads;
    private List<Transform> curEnemies = new List<Transform>();


    private string locationName;


    [Space]
    [Space]
    [Header("Gold")]
    [SerializeField] private Item goldIcon;
    private int Gold = 0;


    [Space]
    [Space]
    [Header("Initialisations")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private BottomInventory bottomInventory;
    [SerializeField] private EventHUD eventHUD;
    [SerializeField] private Player player;
    [SerializeField] private Transform temp;
    [SerializeField] private CameraScript cameraScript;
    [SerializeField] private DialogueStructure dialogueStructure;
    [SerializeField] private GlobalDifficults globalDifficults;
    [SerializeField] private Map map;
    [SerializeField] private List<Transform> positionsForSpawning;
    [SerializeField] private ArrowScript arrow;



    [Space]
    [Space]
    [Header("Sounds")]
    [SerializeField] private AudioSource poisonSound;


    [Space]
    [Space]
    [Header("Time")]
    [SerializeField] private Image timeIcon;
    [SerializeField] private Sprite morning, evening;
    [SerializeField] private TextMeshProUGUI hourText;
    [SerializeField] private TextMeshProUGUI dayText;
    private int hour = 1, day = 1;
    private void Start()
    {
        if (SelectCharacterButton.isTutuorial)
        {
            cameraScript.actualCamera.orthographicSize = 15;
            RoomStart("Tutorial", tutorial1);
        }
    }
    public void RoomStart(string locationName, Squad curSquad)
    {
        this.curSquad = curSquad;
        this.locationName = locationName;
        //Camera
        cameraScript.isKinematic = true;
        if (curSquad.squadName == "Baker")
        {
            curSquad.dialogues[0].choice1text = "Yeah sure(Pay " + ((player.maxHP-player.HP) * 3) + " gold.)";
            if (player.gold < (player.maxHP - player.HP) * 3)
            {
                curSquad.dialogues[0].isButton1Clickable = false;
            }
            else
            {
                curSquad.dialogues[0].isButton1Clickable = true;
            }
        }
        else if (curSquad.dialogues[0].person == "Vampire")
        {
            curSquad.dialogues[0].choice2text = "Let the vampire drink your blood (" + player.maxHP/4 + " hp)";        
        }
        //Selection
        List<GameObject> selectedEnemies = curSquad.squadVariations[Random.Range(0, curSquad.squadVariations.Count)].enemies;
        //Spawn
        curEnemies = new List<Transform> { null, null, null, null, null, null };
        for (int i = 0; i < selectedEnemies.Count; i++)
        {
            if (selectedEnemies[i] != null)
            {
                curEnemies[i] = (Instantiate(selectedEnemies[i], Vector3.zero, Quaternion.Euler(0, 0, 0), temp).transform);
            }
        }

        EnemySetPosition(curEnemies);
        //Dialogue
        dialogueStructure.Initialization(curSquad.dialogues);
        StartFight();
    }

    public void SpawnSquad(Squad squad)
    {
        DestroyAllEnemies();
        RoomStart(locationName, squad);
    }

    public void StartFight()
    {
        gameManager.CurRoomVisible(true);
        bottomInventory.checkPassive();
        player.FightStart();
    }
    public void Update()
    {
        if (startTempChecking)
        {
            tempChecking();
        }
    }

    public void StatementUpdate(DialogueStructure.statement Statement)
    {
        switch (Statement)
        {
            case DialogueStructure.statement.Null:
                break;
            case DialogueStructure.statement.StartFight:
                gameManager.FightHUDVisible(true);
                AllEnemiesPrepareHit();
                startTempChecking = true;
                break;
            case DialogueStructure.statement.RunningAway:
                gameManager.EventHUDVisible(EventHUD.statement.RunningAway);
                break;
            case DialogueStructure.statement.EnemiesAttacksFirst:
                gameManager.FightHUDVisible(true);
                AllEnemiesPrepareHit();
                startTempChecking = true;
                player.NewTurn();
                StartCoroutine(AllEnemiesHit());
                break;
            case DialogueStructure.statement.Leaving:
                gameManager.EventHUDVisible(EventHUD.statement.Leaving);
                break;
            case DialogueStructure.statement.CHESTOpening:
                curEnemies[0].GetComponent<Animator>().SetBool("opening", true);
                startTempChecking = true;
                break;
            case DialogueStructure.statement.CHESTMimicSpawn:
                curEnemies[0].GetComponent<Animator>().SetBool("mimicSpawn", true);
                break;
            case DialogueStructure.statement.CHESTSomethingInTheBoxSpawn:
                curEnemies[0].GetComponent<Animator>().SetBool("somethingInTheBoxSpawn", true);
                break;
            case DialogueStructure.statement.DestroyEverybody:
                DestroyAllEnemies();
                startTempChecking = true;
                break;
            case DialogueStructure.statement.BAKERFreeHeal:
                player.GetHeal(player.maxHP / 4);
                gameManager.EventHUDVisible(EventHUD.statement.Leaving);
                break;
            case DialogueStructure.statement.BAKERPayForHeal:
                player.AddGold(-((player.maxHP - player.HP) * 3));
                player.GetHeal(player.maxHP - player.HP);
                gameManager.EventHUDVisible(EventHUD.statement.Leaving);
                break;
            case DialogueStructure.statement.BAKERSteal:
                if (player.gold == 0)
                {
                    curSquad.dialogues[5].text = "He tried to take coins from me, but found nothing.";
                }
                else
                {
                    curSquad.dialogues[5].text = "He took " + player.gold / 5 + " coins from me.";
                    player.AddGold(-player.gold / 5);
                }
                break;
            case DialogueStructure.statement.EVENTVampireGiveAndLeave:
                player.GetHit(player.maxHP / 4, false, false, false, true);
                if (player.HP > 0)
                {
                    gameManager.EventHUDVisible(EventHUD.statement.Leaving);
                }
                break;
            case DialogueStructure.statement.EVENTVampireGiveAndReward:
                player.GetHit(player.maxHP / 4, false, false, false, true);
                if (player.HP > 0)
                {
                    DestroyAllEnemies();
                    startTempChecking = true;
                }
                break;
            case DialogueStructure.statement.TUTORIALJustDatePanel:
                gameManager.TimePanelVisible(true);
                break;
            case DialogueStructure.statement.TUTORIALRewarAndEnemies:
                for (int i = 0; i < SelectCharacterButton.curPlayer.startItems.Count; i++)
                {
                    curSquad.numberOfRewards[i].possibleRewards[0].itemPool = SelectCharacterButton.curPlayer.startItems[i];
                }
                rewardCalculation();
                gameManager.EventHUDVisible(EventHUD.statement.FirstFight);
                break;
            case DialogueStructure.statement.TUTORIALRewardAndEnemiesWithGeneration:
                gameManager.JustMapVisible(true);
                map.StartRoomsSpawnCoroutine(true);
                for (int i = 0; i < SelectCharacterButton.curPlayer.startItems.Count; i++)
                {
                    curSquad.numberOfRewards[i].possibleRewards[0].itemPool = SelectCharacterButton.curPlayer.startItems[i];
                }
                rewardCalculation();
                gameManager.EventHUDVisible(EventHUD.statement.FirstFightWithGeneration);
                break;
            case DialogueStructure.statement.TUTORIALArrowDeactivation:
                arrow.gameObject.SetActive(false);
                break;
            case DialogueStructure.statement.TUTORIALArrowMovement1:
                arrow.SetArrow(-980, 530);
                break;
            case DialogueStructure.statement.TUTORIALArrowMovement2:
                arrow.SetArrow(-574, 530);
                break;
            case DialogueStructure.statement.TUTORIALArrowMovement3:
                arrow.SetArrow(-340, 530);
                break;
            case DialogueStructure.statement.TUTORIALArrowMovement4:
                gameManager.TimePanelVisible(true);
                arrow.SetArrow(0, 410);
                break;
            case DialogueStructure.statement.TUTORIALArrowMovement5:
                arrow.SetArrow(0, 520);
                break;
            case DialogueStructure.statement.TUTORIALArrowMovement6:
                arrow.SetArrow(-213, 350);
                break;
            case DialogueStructure.statement.TUTORIALArrowMovement7:
                arrow.SetArrow(-143, 350);
                break;
            case DialogueStructure.statement.TUTORIALArrowMovement8:
                arrow.SetArrow(-60, 350);
                break;
            case DialogueStructure.statement.TUTORIALSkip:
                gameManager.TimePanelVisible(true);
                gameManager.JustMapVisible(true);
                map.StartRoomsSpawnCoroutine(true);
                for (int i = 0; i < SelectCharacterButton.curPlayer.startItems.Count; i++)
                {
                    curSquad.numberOfRewards[i].possibleRewards[0].itemPool = SelectCharacterButton.curPlayer.startItems[i];
                }
                rewardCalculation();
                gameManager.EventHUDVisible(EventHUD.statement.Victory);
                break;
        }
    }

    private void EnemySetPosition(List<Transform> enemiesList)
    {
        for (int i = 0; i < enemiesList.Count; i++)
        {
            if (enemiesList[i] != null)
            {
                enemiesList[i].position = positionsForSpawning[i].position;
                try
                {
                    curEnemies[i].GetComponent<Enemy>().cell = positionsForSpawning[i];
                }
                catch
                {

                }
            }
        }
    }

    private void tempChecking()
    {
        if (temp.childCount == 0)
        {
            if (curSquad.isCursed)
            {
                globalDifficult--;
                if (globalDifficult < 0)
                {
                    globalDifficult = 0;
                }
                globalDifficults.UpdateDifficults(globalDifficult);
                gameManager.EventHUDVisible(EventHUD.statement.RunningAway);
            }
            startTempChecking = false;
            rewardCalculation();
            if (Gold != 0)
            {
                Item gold = goldIcon;
                gold.damage = Gold;
                Gold = 0;
                eventHUD.rewardItems.Add(gold);
            }
            gameManager.EventHUDVisible(EventHUD.statement.Victory);
        }
    }
    private void rewardCalculation()
    {
        int rand = Random.Range(0, 100);
        int lastPossibility = 0, currentPossibility = 0;
        eventHUD.rewardItems.Clear();
        for (int i = 0; i < curSquad.numberOfRewards.Count; i++)
        {
            for (int j = 0; j < curSquad.numberOfRewards[i].possibleRewards.Count; j++)
            {
                currentPossibility += curSquad.numberOfRewards[i].possibleRewards[j].possibility;
                if (rand <= currentPossibility && rand > lastPossibility)
                {
                    eventHUD.rewardItems.Add(curSquad.numberOfRewards[i].possibleRewards[j].itemPool.items[Random.Range(0, curSquad.numberOfRewards[i].possibleRewards[j].itemPool.items.Count)]);
                    break;
                }
                lastPossibility = currentPossibility;
            }
        }
    }
    public void NextTurn()
    {
        hour += 1;
        if (hour == 24)
        {
            hour= 0;
            day++;
            dayText.text = "Day:" + day;
            if (globalDifficult != 5)
            {
                globalDifficult++;
                globalDifficults.UpdateDifficults(globalDifficult);
                List<Squad> tempSquadList = new List<Squad>();
                foreach (Squad s in cursedSquads)
                {
                    if (s.difficult == globalDifficult)
                    {
                        tempSquadList.Add(s);
                    }
                }
                map.SetCursedRoom(tempSquadList[Random.Range(0 , tempSquadList.Count)]);
            }
            for (int i = 0; i < temp.childCount; i++)
            {
                temp.GetChild(i).GetComponent<Enemy>().AddHPForDifficult();
            }
        }
        if (hour >= 6 && hour < 18)
        {
            timeIcon.sprite = morning;
        }
        else
        {
            timeIcon.sprite = evening;
        }
        hourText.text = "Hour:" + hour;
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
                curEnemy.Hit(globalDifficult);
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
                if (curEnemy.isTerrifying)
                {
                    curEnemy.plusMissChance = 50;
                }
                else
                {
                    curEnemy.plusMissChance = 0;
                }
               
                if (curEnemy.poison > 0)
                {
                    poisonSound.Play();
                    curEnemy.GetHit(2 * curEnemy.poison, false, false, false, true);
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
        gameManager.FightHUDVisible(false);
        gameManager.MapVisible(true);
        DestroyAllEnemies();
    }

    private void DestroyAllEnemies()
    {
        for (int i = 0; i < temp.childCount; i++)
        {
            Destroy(temp.GetChild(i).gameObject);
        }
    }

    public void AddGold(int gold)
    {
        int r = Random.Range(-1, 1);
        Gold += gold + (r * gold /10) + r;
    }

    public void AddEnemies(List<Enemy> enemies, List<Vector3> positions)
    {
        for (int i = 0; i < positions.Count; i++)
        {
            Enemy curEnemy = Instantiate(enemies[i], positions[i], Quaternion.Euler(0, 0, 0), temp);

            curEnemy.nextAttack = curEnemy.attacks[Random.Range(1, curEnemy.attacks.Count)];

            curEnemy.transform.Find("AttackIcon").GetComponent<SpriteRenderer>().sprite = curEnemy.nextAttack.attackIcon;
            curEnemy.EffectUpdate();
        }
    }
}
