using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DialogueStructure : MonoBehaviour
{
    [HideInInspector] public bool isDialogueOpen = false;
    private int ichar = 0;
    [HideInInspector] public bool epressed = false, stringEnded = false;
    private string Text = "";
    private float delayBetweenLetters = 0.1f, startdelayBetweenLetters = 0.1f;
    
    [HideInInspector] public List<DialogueBranch> dialogues;
    [HideInInspector] public DialogueBranch curDialogueBranch;
    [HideInInspector] public int Statement = 0;


    //Player sprite
    private Sprite playerSprite;


    [Header("Initialisations")]
    public GameObject faceIcon;
    public TextMeshProUGUI text, person;
    public RectTransform button1, button2, button3, button4;
    [SerializeField] private FightManager fightManager;


    [Space]
    [Space]
    [Header("Sounds")]
    [SerializeField] private AudioSource textSound;


    //For double click
    private float clicked = 0;
    private float clicktime = 0;
    private float clickdelay = 1f;
    private void Start()
    {
        playerSprite = SelectCharacterButton.curPlayer.GetComponent<SpriteRenderer>().sprite;
    }
    public void Initialization(List<DialogueBranch> dialogues)
    {
        button1.anchoredPosition = new Vector3(-1000, -1000, 0);
        button2.anchoredPosition = new Vector3(-1000, -1000, 0);
        button3.anchoredPosition = new Vector3(-1000, -1000, 0);
        button4.anchoredPosition = new Vector3(-1000, -1000, 0);

        Statement = 0;
        this.dialogues = dialogues;
        curDialogueBranch = dialogues[0];
        epressed = true;
    }

    public int Interaction()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            clicked++;
            if (clicked == 1) clicktime = Time.time;

            if (clicked > 1 && Time.time - clicktime < clickdelay)
            {
                clicked = 0;
                clicktime = 0;
                if (!stringEnded)
                {
                    if (epressed)
                    {
                        startdelayBetweenLetters = 0;
                    }
                }
            }
            else if (clicked > 2 || Time.time - clicktime > 1) clicked = 0;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!stringEnded)
            {
                if (epressed)
                {
                    startdelayBetweenLetters = 0;
                }
            }
        }
        if (stringEnded)
        {
            DialogueButton.curInteractableItem = this;
            if (curDialogueBranch.choice1text != "")
            {
                button1.anchoredPosition = button1.GetChild(0).GetComponent<DialogueButton>().defpos;
                button1.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = curDialogueBranch.choice1text;
                if (curDialogueBranch.button1active)
                {
                    button1.GetComponent<ButtonScript>().activation();
                }
                else
                {
                    button1.GetComponent<ButtonScript>().deactivation();
                }
            }
            if (curDialogueBranch.choice2text != "")
            {
                button2.anchoredPosition = button2.GetChild(0).GetComponent<DialogueButton>().defpos;
                button2.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = curDialogueBranch.choice2text;
                if (curDialogueBranch.button2active)
                {
                    button2.GetComponent<ButtonScript>().activation();
                }
                else
                {
                    button2.GetComponent<ButtonScript>().deactivation();
                }
            }
            if (curDialogueBranch.choice3text != "")
            {
                button3.anchoredPosition = button3.GetChild(0).GetComponent<DialogueButton>().defpos;
                button3.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = curDialogueBranch.choice3text;
                if (curDialogueBranch.button3active)
                {
                    button3.GetComponent<ButtonScript>().activation();
                }
                else
                {
                    button3.GetComponent<ButtonScript>().deactivation();
                }
            }
            if (curDialogueBranch.choice4text != "")
            {
                button4.anchoredPosition = button4.GetChild(0).GetComponent<DialogueButton>().defpos;
                button4.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = curDialogueBranch.choice4text;
                if (curDialogueBranch.button4active)
                {
                    button4.GetComponent<ButtonScript>().activation();
                }
                else
                {
                    button4.GetComponent<ButtonScript>().deactivation();
                }
            }
        }
        if (epressed)
        {
            isDialogueOpen = true;
            if (curDialogueBranch == null)
            {
                text.SetText("");
                person.SetText("");
                Text = "";
                ichar = 0;
                stringEnded = false;
                epressed = false;
                isDialogueOpen = false;
                fightManager.start = false;
            }
            else
            {
                if (curDialogueBranch.icon == null)
                {
                    curDialogueBranch.icon = playerSprite;
                }
                faceIcon.GetComponent<Image>().sprite = curDialogueBranch.icon;
                float iconWidth = faceIcon.GetComponent<RectTransform>().sizeDelta.x / curDialogueBranch.icon.rect.width;
                float iconHeight = faceIcon.GetComponent<RectTransform>().sizeDelta.y / curDialogueBranch.icon.rect.height;
                faceIcon.GetComponent<RectTransform>().sizeDelta = new Vector2 (curDialogueBranch.icon.rect.width * Mathf.Min(iconWidth, iconHeight), curDialogueBranch.icon.rect.height * Mathf.Min(iconWidth, iconHeight));
                person.SetText(curDialogueBranch.person);
                if (ichar < curDialogueBranch.textToChars().Count)
                {
                    if (delayBetweenLetters <= 0)
                    {
                        Text += curDialogueBranch.textToChars()[ichar];
                        if (curDialogueBranch.textToChars()[ichar] != " ")
                        {
                            textSound.Play();
                        }
                        delayBetweenLetters = startdelayBetweenLetters;
                        ichar++;
                    }
                    else
                    {
                        delayBetweenLetters -= Time.deltaTime;
                    }
                }
                else
                {
                    startdelayBetweenLetters = 0.1f;
                    epressed = false;
                    stringEnded = true;
                }
                text.SetText(Text);
            }
        }
        return (Statement);
    }
    public void DialogueSelection(DialogueBranch nextDialogueBranch)
    {
        if (stringEnded)
        {
            Statement = curDialogueBranch.eventNumber;
            button1.anchoredPosition = new Vector3(-1000, -1000, 0);
            button2.anchoredPosition = new Vector3(-1000, -1000, 0);
            button3.anchoredPosition = new Vector3(-1000, -1000, 0);
            button4.anchoredPosition = new Vector3(-1000, -1000, 0);
            faceIcon.GetComponent<RectTransform>().sizeDelta = new Vector2(380, 380);
            curDialogueBranch = nextDialogueBranch;
            text.SetText("");
            person.SetText("");
            Text = "";
            ichar = 0;
            stringEnded = false;
            epressed = true;
        }
    }
}

