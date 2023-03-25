using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToogleScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private AudioSource buttonDownSound, buttonUpSound;
    public void OnPointerDown(PointerEventData eventData)
    {
        buttonDownSound.Play();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonUpSound.Play();
    }
}
