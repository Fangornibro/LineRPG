using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalDifficults : MonoBehaviour
{
    [SerializeField] private List<Image> difficults;
    [SerializeField] private Sprite fullGlobalDifficult, emptyGlobalDifficult;
    void Start()
    {
        UpdateDifficults(0);
    }

    // Update is called once per frame
    public void UpdateDifficults(int globalDifficult)
    {
        for (int i = 0; i < difficults.Count; i++)
        {
            if (i < globalDifficult)
            {
                difficults[i].sprite = fullGlobalDifficult;
            }
            else
            {
                difficults[i].sprite = emptyGlobalDifficult;
            }
        }
    }
}
