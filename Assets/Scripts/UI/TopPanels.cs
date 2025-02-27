using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TopPanels : MonoBehaviour
{
    [SerializeField] private Transform manaList;
    [SerializeField] private Image emptyBall, hpBar;
    [SerializeField] private Sprite fullMana;
    [SerializeField] private TextMeshProUGUI hpText;

    //private Transform hpList;
    //[SerializeField] private Sprite hp100, hp99, hp75, hp50, hp25;
    private void Start()
    {
        //hpList = transform.Find("HPList").GetComponent<Transform>();
    }
    public void UpdateMana(int maxMana, int curMana)
    {
        for (int i = 0; i < manaList.childCount; i++)
        {
            Destroy(manaList.GetChild(i).gameObject);
        }
        for (int i = 0; i < maxMana; i++)
        {
            Image curManaImg = Instantiate(emptyBall, Vector3.zero, Quaternion.Euler(0, 0, 0), manaList);
            curManaImg.GetComponent<RectTransform>().anchoredPosition = new Vector2(i * 45, 0);
            curManaImg.transform.SetAsFirstSibling();
            if (curMana > 0)
            {
                curManaImg.sprite = fullMana;
                curMana--;
            }
        }
    }

    public void UpdateHP(int maxHP, int curHP)
    {
        hpBar.fillAmount = (float)curHP / (float)maxHP;
        hpText.SetText(curHP + "/" + maxHP);
    }

    //public void UpdateHP(int maxHP, int hpInOneBall, int curHP)
    //{
    //    for (int i = 0; i < hpList.childCount; i++)
    //    {
    //        Destroy(hpList.GetChild(i).gameObject);
    //    }
    //    for (int i = 1; i <= maxHP / hpInOneBall; i++)
    //    {
    //        Image curHpImg = Instantiate(emptyBall, Vector3.zero, Quaternion.Euler(0, 0, 0), hpList);
    //        curHpImg.transform.SetAsFirstSibling();
    //        curHpImg.GetComponent<RectTransform>().anchoredPosition = new Vector2((i - 1) * 45, 0);
    //        if (curHP <= ((i * hpInOneBall) - hpInOneBall))
    //        {

    //        }
    //        else if (curHP > ((i * hpInOneBall) - hpInOneBall) && curHP <= ((i * hpInOneBall) - ((hpInOneBall / 4) * 3)))
    //        {
    //            curHpImg.sprite = hp25;
    //        }
    //        else if (curHP > ((i * hpInOneBall) - ((hpInOneBall / 4) * 3)) && curHP <= ((i * hpInOneBall) - hpInOneBall / 2))
    //        {
    //            curHpImg.sprite = hp50;
    //        }
    //        else if (curHP > ((i * hpInOneBall) - hpInOneBall / 2) && curHP <= ((i * hpInOneBall) - (hpInOneBall / 4)))
    //        {
    //            curHpImg.sprite = hp75;
    //        }
    //        else if (curHP > ((i * hpInOneBall) - (hpInOneBall / 4)) && curHP < ((i * hpInOneBall)))
    //        {
    //            curHpImg.sprite = hp99;
    //        }
    //        else
    //        {
    //            curHpImg.sprite = hp100;
    //        }
    //    }
    //}
}
