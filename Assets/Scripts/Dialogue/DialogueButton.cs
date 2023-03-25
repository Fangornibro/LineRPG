using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogueButton : MonoBehaviour, IButton
{
    [SerializeField] private DialogueStructure dialogueStructure;
    [HideInInspector] public Vector2 defpos;
    void Start()
    {
        defpos = transform.parent.GetComponent<RectTransform>().anchoredPosition;
    }
    void IButton.OnPointerClick()
    {
        dialogueStructure.clickTime = 0.1f;
        if (transform.name == "DialogueButton1")
        {
            SelectDialogue(dialogueStructure.curDialogueBranch.choice1DialogueBranch);
        }
        else if (transform.name == "DialogueButton2")
        {
            SelectDialogue(dialogueStructure.curDialogueBranch.choice2DialogueBranch);
        }
        else if (transform.name == "DialogueButton3")
        {
            SelectDialogue(dialogueStructure.curDialogueBranch.choice3DialogueBranch);
        }
        else if (transform.name == "DialogueButton4")
        {
            SelectDialogue(dialogueStructure.curDialogueBranch.choice4DialogueBranch);
        }
    }

    private void SelectDialogue(List<int> dialogueBranch)
    {
        if (dialogueBranch.Count != 0)
        {
            dialogueStructure.DialogueSelection(dialogueStructure.dialogues[dialogueBranch[Random.Range(0, dialogueBranch.Count)]]);
        }
        else
        {
            dialogueStructure.DialogueSelection(null);
        }
    }
}
