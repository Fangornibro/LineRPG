using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueActivation : MonoBehaviour
{
    [Header("Initialisations")]
    [SerializeField] private DialogueStructure dialogueStructure;
    void Update()
    {
        if (dialogueStructure.isDialogueOpen)
        {
            transform.Find("DialogueHud").GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
        }
        else
        {
            transform.Find("DialogueHud").GetComponent<RectTransform>().anchoredPosition = new Vector3(-2000, -2000, 0);
        }
    }
}
