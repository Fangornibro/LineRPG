using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CancelSquadInfoButton : MonoBehaviour
{
    private SquadInfoPanel sq;
    private RoomSelector rs;

    void Start()
    {
        sq = GameObject.Find("SquadInfoPanel").GetComponent<SquadInfoPanel>();
        rs = GameObject.Find("AbilityOnCursor").GetComponent<RoomSelector>();
    }

    public void OnPointerClick()
    {
        rs.ChangeSelection(null);
        sq.isOpened = false;
    }
}

