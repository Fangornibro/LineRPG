using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomPanel : MonoBehaviour
{
    public List<GameObject> UI;
    public Cell cell1, cell2, cell3, cell4;
    private FightManager fm;

    private void Start()
    {
        fm = GameObject.Find("LevelDialogue").GetComponent<FightManager>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && cell1.icon != null)
        {
            cell1.icon.Use();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && cell2.icon != null)
        {
            cell2.icon.Use();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && cell3.icon != null)
        {
            cell3.icon.Use();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) && cell4.icon != null)
        {
            cell4.icon.Use();
        }

        if (DialogueStructure.isDialogueOpen || !fm.startTempChecking)
        {
            foreach (GameObject go in UI)
            {
                if (go.GetComponent<Cell>() != null)
                {
                    if (go.GetComponent<Cell>().icon != null)
                    {
                        go.GetComponent<Cell>().icon.gameObject.SetActive(false);
                    }
                }
                go.SetActive(false);
            }
        }
        else
        {
            foreach (GameObject go in UI)
            {
                if (go.GetComponent<Cell>() != null)
                {
                    if (go.GetComponent<Cell>().icon != null)
                    {
                        go.GetComponent<Cell>().icon.gameObject.SetActive(true);
                    }
                }
                go.SetActive(true);
            }
        }
    }
}
