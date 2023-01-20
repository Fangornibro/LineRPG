using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CancelSquadInfoButton : MonoBehaviour
{
    private SquadInfoPanel sq;

    void Start()
    {
        sq = GameObject.Find("SquadInfoPanel").GetComponent<SquadInfoPanel>();
    }

    public void OnPointerClick()
    {
        sq.isOpened = false;
    }
}

