using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FocusButton : MonoBehaviour, IButton
{
    [SerializeField] private CameraScript cam;
    [SerializeField] private Map map;
    void IButton.OnPointerClick()
    {
        if (map.room != null)
        {
            cam.StartApproximationCoroutine(map.room.transform.position, 15, false);
        }
    }

}
