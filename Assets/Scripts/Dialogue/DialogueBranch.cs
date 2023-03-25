using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class DialogueBranch
{
    [TextArea(3, 5)]
    public string text;
    public string person;
    public string choice1text, choice2text, choice3text, choice4text;
    [HideInInspector]
    public bool isButton1Clickable = true, isButton2Clickable = true, isButton3Clickable = true, isButton4Clickable = true;
    public List<int> choice1DialogueBranch, choice2DialogueBranch, choice3DialogueBranch, choice4DialogueBranch;
    public DialogueStructure.statement Statement;
    public Sprite icon;

    public List<string> textToChars()
    {
        List<string> Textletters = new List<string>();
        foreach (char s in text)
        {
            Textletters.Add(s.ToString());
        }
        return Textletters;
    }
}
