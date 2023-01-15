using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NextTurnButton : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    private FightManager fm;
    private RectTransform rt;
    [SerializeField]
    private Sprite down, up;
    private void Start()
    {
        fm = GameObject.Find("LevelDialogue").GetComponent<FightManager>();
        rt = GetComponent<RectTransform>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!fm.isEnemiesStillHit && !fm.IsAllEnemiesStillInHit())
        {
            fm.NextTurn();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        rt.sizeDelta = new Vector2(376, 162);
        GetComponent<UnityEngine.UI.Image>().sprite = down;
        transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        rt.sizeDelta = new Vector2(376, 189);
        GetComponent<UnityEngine.UI.Image>().sprite = up;
        transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 18, 0);
    }
}
