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
    private RectTransform rt;
    [SerializeField] private RectTransform txt;
    [SerializeField] private UnityEngine.UI.Image img;
    [SerializeField] private Sprite down, up;
    [SerializeField] private AudioSource buttonDownSound, buttonUpSound;

    private bool isActive = true;

    public enum ButtonSize { small, medium }
    public ButtonSize buttonSize;
    private void Start()
    {
        rt = transform.GetChild(0).GetComponent<RectTransform>();
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
