using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DialogueStructure
{
    public TextMeshProUGUI text = GameObject.Find("Text").GetComponent<TextMeshProUGUI>(), person = GameObject.Find("Person").GetComponent<TextMeshProUGUI>();
    public GameObject faceIcon = GameObject.Find("DialogueHudFaceIcon"), button1 = GameObject.Find("DialogueButton1").transform.GetChild(0).gameObject, button2 = GameObject.Find("DialogueButton2").transform.GetChild(0).gameObject, button3 = GameObject.Find("DialogueButton3").transform.GetChild(0).gameObject, button4 = GameObject.Find("DialogueButton4").transform.GetChild(0).gameObject;
    public static bool isDialogueOpen = false;
    int ichar = 0;
    public bool epressed = false, stringEnded = false, initialization = true;
    string Text = "";
    float delayBetweenLetters = 0.1f, startdelayBetweenLetters = 0.1f;
    public AudioSource textSound = GameObject.Find("textSound").GetComponent<AudioSource>();
    public List<DialogueBranch> dialogues;
    public DialogueBranch curDialogueBranch;
    public int Statement = 0;
    private FightManager ld = GameObject.Find("FightManager").GetComponent<FightManager>();
    //For double click
    private float clicked = 0;
    private float clicktime = 0;
    private float clickdelay = 1f;
    public DialogueStructure(List<DialogueBranch> Dialogues)
    {
        dialogues = Dialogues;
    }

    public int Interaction()
    {
        if (initialization)
        {
            button1.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector3(-1000, -1000, 0);
            button2.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector3(-1000, -1000, 0);
            button3.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector3(-1000, -1000, 0);
            button4.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector3(-1000, -1000, 0);
            curDialogueBranch = dialogues[0];
            initialization = false;
            epressed = true;
        }
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
                button1.transform.parent.GetComponent<RectTransform>().anchoredPosition = button1.GetComponent<DialogueButton>().defpos;
                button1.GetComponentInChildren<TextMeshProUGUI>().text = curDialogueBranch.choice1text;
                if (curDialogueBranch.button1active)
                {
                    button1.transform.parent.GetComponent<ButtonScript>().activation();
                }
                else
                {
                    button1.transform.parent.GetComponent<ButtonScript>().deactivation();
                }
            }
            if (curDialogueBranch.choice2text != "")
            {
                button2.transform.parent.GetComponent<RectTransform>().anchoredPosition = button2.GetComponent<DialogueButton>().defpos;
                button2.GetComponentInChildren<TextMeshProUGUI>().text = curDialogueBranch.choice2text;
                if (curDialogueBranch.button2active)
                {
                    button2.transform.parent.GetComponent<ButtonScript>().activation();
                }
                else
                {
                    button2.transform.parent.GetComponent<ButtonScript>().deactivation();
                }
            }
            if (curDialogueBranch.choice3text != "")
            {
                button3.transform.parent.GetComponent<RectTransform>().anchoredPosition = button3.GetComponent<DialogueButton>().defpos;
                button3.GetComponentInChildren<TextMeshProUGUI>().text = curDialogueBranch.choice3text;
                if (curDialogueBranch.button3active)
                {
                    button3.transform.parent.GetComponent<ButtonScript>().activation();
                }
                else
                {
                    button3.transform.parent.GetComponent<ButtonScript>().deactivation();
                }
            }
            if (curDialogueBranch.choice4text != "")
            {
                button4.transform.parent.GetComponent<RectTransform>().anchoredPosition = button4.GetComponent<DialogueButton>().defpos;
                button4.GetComponentInChildren<TextMeshProUGUI>().text = curDialogueBranch.choice4text;
                if (curDialogueBranch.button4active)
                {
                    button4.transform.parent.GetComponent<ButtonScript>().activation();
                }
                else
                {
                    button4.transform.parent.GetComponent<ButtonScript>().deactivation();
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
                initialization = true;
                ld.start = false;
            }
            else
            {
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
            button1.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector3(-1000, -1000, 0);
            button2.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector3(-1000, -1000, 0);
            button3.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector3(-1000, -1000, 0);
            button4.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector3(-1000, -1000, 0);
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

