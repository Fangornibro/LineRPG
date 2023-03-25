using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static Room;

public class DialogueStructure : MonoBehaviour
{
    private int ichar = 0;
    [HideInInspector] public bool stringEnded = false;
    private string Text = "";
    private float delayBetweenLetters = 0.1f;
    
    [HideInInspector] public List<DialogueBranch> dialogues;
    [HideInInspector] public DialogueBranch curDialogueBranch;


    //Player sprite
    private Sprite playerSprite;


    [Header("Initialisations")]
    public GameObject faceIcon;
    public TextMeshProUGUI text, person;
    public ButtonScript dialogueButton1, dialogueButton2, dialogueButton3, dialogueButton4;
    [SerializeField] private TextMeshProUGUI dialogueButton1Text, dialogueButton2Text, dialogueButton3Text, dialogueButton4Text;
    [SerializeField] private FightManager fightManager;
    [SerializeField] private GameManager uiManager;


    [Space]
    [Space]
    [Header("Sounds")]
    [SerializeField] private AudioSource textSound;


    //For click
    [HideInInspector] public float clickTime = 0.1f;


    public enum statement { Null, StartFight, RunningAway, EnemiesAttacksFirst, Leaving, DestroyEverybody, NextSquad, 
        CHESTOpening, BAKERFreeHeal, BAKERPayForHeal, BAKERSteal, EVENTVampireGiveAndReward, EVENTVampireGiveAndLeave, 
        TUTORIALJustDatePanel, TUTORIALRewarAndEnemies, TUTORIALArrowDeactivation, TUTORIALArrowMovement1, TUTORIALArrowMovement2, 
        TUTORIALArrowMovement3, TUTORIALArrowMovement4, TUTORIALArrowMovement5, TUTORIALRewardAndEnemiesWithGeneration, 
        TUTORIALArrowMovement6, TUTORIALArrowMovement7, TUTORIALArrowMovement8, CHESTMimicSpawn, CHESTSomethingInTheBoxSpawn,
        TUTORIALSkip
    };
    private statement Statement;


    private void Start()
    {
        playerSprite = SelectCharacterButton.curPlayer.GetComponent<SpriteRenderer>().sprite;
        DectivateAllButtons();
    }
    private void Update()
    {
        if (clickTime > 0)
        {
            clickTime -= Time.deltaTime;
        }
        else
        {
            if (!stringEnded && Input.GetKeyDown(KeyCode.Mouse0))
            {
                delayBetweenLetters = 0;
            }
        }
        if (!stringEnded && Input.GetKeyDown(KeyCode.Space))
        {
            delayBetweenLetters = 0;
        }
    }
    public void Initialization(List<DialogueBranch> dialogues)
    {
        uiManager.DialogueStructureVisible(true);
        this.dialogues = dialogues;
        curDialogueBranch = dialogues[0];
        Interaction();
    }
    private void ActivateButton(ButtonScript button, TextMeshProUGUI buttonText, string choiceText, bool isButtonClickable)
    {
        if (choiceText != "")
        {
            button.gameObject.SetActive(true);
            buttonText.text = choiceText;
            if (isButtonClickable)
            {
                button.activation();
            }
            else
            {
                button.deactivation();
            }
        }
    }

    private void ActivateAllButtons()
    {
        ActivateButton(dialogueButton1, dialogueButton1Text, curDialogueBranch.choice1text, curDialogueBranch.isButton1Clickable);
        ActivateButton(dialogueButton2, dialogueButton2Text, curDialogueBranch.choice2text, curDialogueBranch.isButton2Clickable);
        ActivateButton(dialogueButton3, dialogueButton3Text, curDialogueBranch.choice3text, curDialogueBranch.isButton3Clickable);
        ActivateButton(dialogueButton4, dialogueButton4Text, curDialogueBranch.choice4text, curDialogueBranch.isButton4Clickable);
    }

    private void DectivateAllButtons()
    {
        dialogueButton1.gameObject.SetActive(false);
        dialogueButton2.gameObject.SetActive(false);
        dialogueButton3.gameObject.SetActive(false);
        dialogueButton4.gameObject.SetActive(false);
    }

    public void Interaction()
    {
        Sprite curSrite;
        if (curDialogueBranch.icon == null)
        {
            curSrite = playerSprite;
        }
        else
        {
            curSrite = curDialogueBranch.icon;
        }
        faceIcon.GetComponent<Image>().sprite = curSrite;
        float iconWidth = faceIcon.GetComponent<RectTransform>().sizeDelta.x / curSrite.rect.width;
        float iconHeight = faceIcon.GetComponent<RectTransform>().sizeDelta.y / curSrite.rect.height;
        faceIcon.GetComponent<RectTransform>().sizeDelta = new Vector2(curSrite.rect.width * Mathf.Min(iconWidth, iconHeight), curSrite.rect.height * Mathf.Min(iconWidth, iconHeight));
        person.SetText(curDialogueBranch.person);

        StartCoroutine(characterPrint());
    }

    private IEnumerator characterPrint()
    {
        while (ichar < curDialogueBranch.textToChars().Count)
        {
            Text += curDialogueBranch.textToChars()[ichar];
            text.SetText(Text);
            if (curDialogueBranch.textToChars()[ichar] != " ")
            {
                textSound.Play();
            }
            ichar++;
            if (delayBetweenLetters == 0 && ichar > curDialogueBranch.textToChars().Count)
            {
                Text += curDialogueBranch.textToChars()[ichar];
                ichar++;
            }
            yield return new WaitForSeconds(delayBetweenLetters);
        }
        delayBetweenLetters = 0.1f;
        stringEnded = true;
        ActivateAllButtons();

    }
    public void DialogueSelection(DialogueBranch nextDialogueBranch)
    {
        Statement = curDialogueBranch.Statement;
        curDialogueBranch = nextDialogueBranch;

        text.SetText("");
        person.SetText("");
        Text = "";
        ichar = 0;
        stringEnded = false;
        DectivateAllButtons();
        faceIcon.GetComponent<RectTransform>().sizeDelta = new Vector2(380, 380);
        if (curDialogueBranch == null)
        {
            uiManager.DialogueStructureVisible(false);
            fightManager.StartFight();
        }
        else
        {
            Interaction();
        }
        fightManager.StatementUpdate(Statement);
    }
}

