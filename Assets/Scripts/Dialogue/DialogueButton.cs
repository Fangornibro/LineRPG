using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogueButton : MonoBehaviour, IButton
{
    public static DialogueStructure curInteractableItem;
    [HideInInspector]
    public Vector2 defpos;
    void Start()
    {
        defpos = transform.parent.GetComponent<RectTransform>().anchoredPosition;
    }
    void IButton.OnPointerClick()
    {
        if (curInteractableItem != null)
        {
            if (transform.name == "DialogueButton1")
            {
                if (curInteractableItem.curDialogueBranch.choice1dialoguebranch.Count != 0)
                {
                    curInteractableItem.DialogueSelection(curInteractableItem.dialogues[curInteractableItem.curDialogueBranch.choice1dialoguebranch[Random.Range(0, curInteractableItem.curDialogueBranch.choice1dialoguebranch.Count)]]);
                }
                else
                {
                    curInteractableItem.DialogueSelection(null);
                }
            }
            if (transform.name == "DialogueButton2")
            {
                if (curInteractableItem.curDialogueBranch.choice2dialoguebranch.Count != 0)
                {
                    curInteractableItem.DialogueSelection(curInteractableItem.dialogues[curInteractableItem.curDialogueBranch.choice2dialoguebranch[Random.Range(0, curInteractableItem.curDialogueBranch.choice2dialoguebranch.Count)]]);
                }
                else
                {
                    curInteractableItem.DialogueSelection(null);
                }
            }
            if (transform.name == "DialogueButton3")
            {
                if (curInteractableItem.curDialogueBranch.choice3dialoguebranch.Count != 0)
                {
                    curInteractableItem.DialogueSelection(curInteractableItem.dialogues[curInteractableItem.curDialogueBranch.choice3dialoguebranch[Random.Range(0, curInteractableItem.curDialogueBranch.choice3dialoguebranch.Count)]]);
                }
                else
                {
                    curInteractableItem.DialogueSelection(null);
                }
            }
            if (transform.name == "DialogueButton4")
            {
                if (curInteractableItem.curDialogueBranch.choice4dialoguebranch.Count != 0)
                {
                    curInteractableItem.DialogueSelection(curInteractableItem.dialogues[curInteractableItem.curDialogueBranch.choice4dialoguebranch[Random.Range(0, curInteractableItem.curDialogueBranch.choice4dialoguebranch.Count)]]);
                }
                else
                {
                    curInteractableItem.DialogueSelection(null);
                }
            }
        }
        curInteractableItem = null;
    }
}
