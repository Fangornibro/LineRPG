using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public Transform invent;
    [HideInInspector]
    public bool isMenuOpen = false;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !invent.GetComponent<Inventory>().isInventOpen && !DialogueStructure.isDialogueOpen)
        {
            Pause.pauseOn = !Pause.pauseOn;
            isMenuOpen = !isMenuOpen;
        }
    }
}