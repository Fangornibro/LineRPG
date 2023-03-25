using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnPanel : MonoBehaviour
{
    [SerializeField] private GameObject dayText, hourText, timeIcon;
    public void textShowUP()
    {
        dayText.SetActive(true);
        hourText.SetActive(true);
        timeIcon.SetActive(true);
    }
    public void textHideUP()
    {
        dayText.SetActive(false);
        hourText.SetActive(false);
        timeIcon.SetActive(false);
    }
}
