using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squad : MonoBehaviour
{
    [System.Serializable]
    public class ListWrapper
    {
        public List<GameObject> myList;
    }
    public List<ListWrapper> enemies;
    public string squadName;
    [TextArea(2, 4)]
    public string description;
    [Range(0, 8)]
    public int difficult;
    public List<DialogueBranch> dialogues;
}
