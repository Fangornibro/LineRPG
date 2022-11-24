using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class FightActivation : MonoBehaviour
{
    private FightManager fm;
    private Transform manaList;
    public Image manaPrefab;
    public List<Image> mana;
    public Sprite fullMana;
    private void Start()
    {
        fm = GameObject.Find("LevelDialogue").GetComponent<FightManager>();
        manaList = GameObject.Find("ManaList").GetComponent<Transform>();
    }
    void Update()
    {
        if (fm.startTempChecking && fm.curEventString == "FightIcon")
        {
            transform.Find("FightHud").GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        }
        else
        {
            transform.Find("FightHud").GetComponent<RectTransform>().anchoredPosition = new Vector3(-3000, -3000, 0);
        }
    }
    public void UpdateMana(int maxMana, int curMana)
    {
        for (int i = 0; i < manaList.childCount; i++)
        {
            GameObject.Destroy(manaList.GetChild(i).gameObject);
        }
        for (int i = 0; i < maxMana; i++)
        {
            Image curManaImg = GameObject.Instantiate(manaPrefab, Vector3.zero, Quaternion.Euler(0, 0, 0), manaList);
            curManaImg.GetComponent<RectTransform>().anchoredPosition = new Vector2(i * 150, 0);
            if (curMana > 0)
            {
                curManaImg.sprite = fullMana;
                curMana--;
            }
            mana.Add(curManaImg);
        }
    }
}
