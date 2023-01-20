using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class DialogueBranch
{
    [Multiline(5)]
    public string text;
    public string person;
    public string choice1text, choice2text, choice3text, choice4text;
    [HideInInspector]
    public bool button1active = true, button2active = true, button3active = true, button4active = true;
    public List<int> choice1dialoguebranch, choice2dialoguebranch, choice3dialoguebranch, choice4dialoguebranch;
    public int eventNumber;
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
