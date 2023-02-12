using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
interface IButton
{
    void OnPointerClick();
}

public class ButtonScript : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    private RectTransform rt, txt;
    private UnityEngine.UI.Image img;
    [SerializeField]
    private Sprite down, up;
    private AudioSource buttonDownSound, buttonUpSound;

    private bool isActive = true;

    public enum ButtonSize { small, medium }
    public ButtonSize buttonSize;
    private void Start()
    {
        rt = transform.GetChild(0).GetComponent<RectTransform>();
        try
        {
            txt = transform.GetChild(0).GetChild(0).GetComponent<RectTransform>();
        }
        catch
        {
            txt = null;
        }
        img = transform.GetChild(0).GetComponent<UnityEngine.UI.Image>();
        buttonDownSound = GameObject.Find("buttonDownSound").GetComponent<AudioSource>();
        buttonUpSound = GameObject.Find("buttonUpSound").GetComponent<AudioSource>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left && isActive)
        {
            rt.GetComponent<IButton>().OnPointerClick();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isActive)
        {
            buttonDownSound.Play();
            img.sprite = down;
            img.color = new Color(0.7843137f, 0.7843137f, 0.7843137f, 1);
            if (buttonSize == ButtonSize.small)
            {
                rt.sizeDelta -= new Vector2(0, 9);
                if (txt != null)
                {
                    txt.anchoredPosition -= new Vector2(0, 9);
                }
            }
            else if (buttonSize == ButtonSize.medium)
            {
                rt.sizeDelta -= new Vector2(0, 18);
                if (txt != null)
                {
                    txt.anchoredPosition -= new Vector2(0, 18);
                }
            }
        }        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isActive)
        {
            buttonUpSound.Play();
            img.sprite = up;
            img.color = Color.white;
            if (buttonSize == ButtonSize.small)
            {
                rt.sizeDelta += new Vector2(0, 9);
                if (txt != null)
                {
                    txt.anchoredPosition += new Vector2(0, 9);
                }
            }
            else if (buttonSize == ButtonSize.medium)
            {
                rt.sizeDelta += new Vector2(0, 18);
                if (txt != null)
                {
                    txt.anchoredPosition += new Vector2(0, 18);
                }
            }
        }
    }


    public void deactivation()
    {
        isActive = false;
        img.color = new Color(0.7843137f, 0.7843137f, 0.7843137f, 1f);
    }

    public void activation()
    {
        isActive = true;
        img.color = Color.white;
    }
}
