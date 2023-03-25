using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squad : MonoBehaviour
{
    [System.Serializable]
    public class ListWrapper
    {
        public List<GameObject> enemies;
    }
    [System.Serializable]
    public class NumberOfRewards 
    {
        public List<PosibleRewards> possibleRewards;
    }
    [System.Serializable]
    public class PosibleRewards
    {
        public ItemPool itemPool;
        public int possibility;
    }

    public string squadName;
    [TextArea(2, 4)]
    public string description;
    [Range(0, 8)]
    public int difficult;
    public List<NumberOfRewards> numberOfRewards;
    public List<ListWrapper> squadVariations;
    public List<DialogueBranch> dialogues;
    [HideInInspector] public bool isCursed = false;
}
