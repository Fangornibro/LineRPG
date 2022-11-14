using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueBranch
{
    public string text, person;

    public string choice1text, choice2text, choice3text, choice4text;
    public DialogueBranch choice1dialoguebranch, choice2dialoguebranch, choice3dialoguebranch, choice4dialoguebranch;
    public int nextStatement, fontSize = 30;
    public Sprite icon;

    public DialogueBranch(string Person, string Text, Sprite Icon, string Choice1text, string Choice2text, string Choice3text, string Choice4text, DialogueBranch Choice1dialoguebranch, DialogueBranch Choice2dialoguebranch, DialogueBranch Choice3dialoguebranch, DialogueBranch Choice4dialoguebranch, int NextStatement)
    {
        person = Person;
        text = Text;
        icon = Icon;
        choice1text = Choice1text;  
        choice2text = Choice2text;
        choice3text = Choice3text;
        choice4text = Choice4text;
        choice1dialoguebranch = Choice1dialoguebranch;
        choice2dialoguebranch = Choice2dialoguebranch;
        choice3dialoguebranch = Choice3dialoguebranch;
        choice4dialoguebranch = Choice4dialoguebranch;
        nextStatement = NextStatement;
    }

    public DialogueBranch(string Person, string Text, Sprite Icon, string Choice1text, string Choice2text, string Choice3text, string Choice4text, DialogueBranch Choice1dialoguebranch, DialogueBranch Choice2dialoguebranch, DialogueBranch Choice3dialoguebranch, DialogueBranch Choice4dialoguebranch, int NextStatement, int FontSize)
    {
        person = Person;
        text = Text;
        icon = Icon;
        choice1text = Choice1text;
        choice2text = Choice2text;
        choice3text = Choice3text;
        choice4text = Choice4text;
        choice1dialoguebranch = Choice1dialoguebranch;
        choice2dialoguebranch = Choice2dialoguebranch;
        choice3dialoguebranch = Choice3dialoguebranch;
        choice4dialoguebranch = Choice4dialoguebranch;
        nextStatement = NextStatement;
        fontSize = FontSize;
    }

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
