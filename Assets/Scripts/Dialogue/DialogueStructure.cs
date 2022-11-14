using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using static Room;

public class DialogueStructure
{
    public TextMeshProUGUI text = GameObject.Find("Text").GetComponent<TextMeshProUGUI>(), person = GameObject.Find("Person").GetComponent<TextMeshProUGUI>();
    public GameObject faceIcon = GameObject.Find("DialogueHudFaceIcon"), button1 = GameObject.Find("DialogueButton1"), button2 = GameObject.Find("DialogueButton2"), button3 = GameObject.Find("DialogueButton3"), button4 = GameObject.Find("DialogueButton4");
    public static bool isDialogueOpen = false;
    int ichar = 0;
    public bool epressed = false, stringEnded = false, initialization = true;
    string Text = "";
    float delayBetweenLetters = 0.1f, startdelayBetweenLetters = 0.1f;
    public AudioSource textSound = GameObject.Find("textSound").GetComponent<AudioSource>();
    public DialogueBranch dialogue, curDialogueBranch;
    public int Statement = 0;
    private LevelDialogue ld = GameObject.Find("LevelDialogue").GetComponent<LevelDialogue>();
    public DialogueStructure(DialogueBranch Dialogue)
    {
        dialogue = Dialogue;
    }

    public int Interaction()
    {
        if (initialization)
        {
            button1.GetComponent<RectTransform>().anchoredPosition = new Vector3(-1000, -1000, 0);
            button2.GetComponent<RectTransform>().anchoredPosition = new Vector3(-1000, -1000, 0);
            button3.GetComponent<RectTransform>().anchoredPosition = new Vector3(-1000, -1000, 0);
            button4.GetComponent<RectTransform>().anchoredPosition = new Vector3(-1000, -1000, 0);
            curDialogueBranch = dialogue;
            initialization = false;
            epressed = true;
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
            if (curDialogueBranch.choice1text != null)
            {
                button1.GetComponent<RectTransform>().anchoredPosition = new Vector3(-12, 290, 0);
                button1.GetComponentInChildren<TextMeshProUGUI>().text = curDialogueBranch.choice1text;
            }
            if (curDialogueBranch.choice2text != null)
            {
                button2.GetComponent<RectTransform>().anchoredPosition = new Vector3(-12, 200, 0);
                button2.GetComponentInChildren<TextMeshProUGUI>().text = curDialogueBranch.choice2text;
            }
            if (curDialogueBranch.choice3text != null)
            {
                button3.GetComponent<RectTransform>().anchoredPosition = new Vector3(-12, 110, 0);
                button3.GetComponentInChildren<TextMeshProUGUI>().text = curDialogueBranch.choice3text;
            }
            if (curDialogueBranch.choice4text != null)
            {
                button4.GetComponent<RectTransform>().anchoredPosition = new Vector3(-12, 20, 0);
                button4.GetComponentInChildren<TextMeshProUGUI>().text = curDialogueBranch.choice4text;
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
                text.fontSize = curDialogueBranch.fontSize;
                text.SetText(Text);
            }
        }
        return (Statement);
    }
    public void DialogueSelection(DialogueBranch nextDialogueBranch)
    {
        if (stringEnded)
        {
            Statement = curDialogueBranch.nextStatement;
            button1.GetComponent<RectTransform>().anchoredPosition = new Vector3(-1000, -1000, 0);
            button2.GetComponent<RectTransform>().anchoredPosition = new Vector3(-1000, -1000, 0);
            button3.GetComponent<RectTransform>().anchoredPosition = new Vector3(-1000, -1000, 0);
            button4.GetComponent<RectTransform>().anchoredPosition = new Vector3(-1000, -1000, 0);
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

