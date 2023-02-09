using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SocialPlatforms;

public class StartFightButton : MonoBehaviour
{
    private SquadInfoPanel sip;
    private Inventory inventory;
    private CameraScript cam;

    private bool isStartedAnim = false;
    [HideInInspector]
    public Vector3 roomPos;

    private void Start()
    {
        cam = GameObject.Find("Main Camera").GetComponent<CameraScript>();
        sip = GameObject.Find("SquadInfoPanel").GetComponent<SquadInfoPanel>();
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
    }
    public void OnPointerClick()
    {
        isStartedAnim = true;
        sip.isOpened= false;
        inventory.isInventOpen= false;
    }

    private void Update()
    {
        if (isStartedAnim)
        {
            isStartedAnim = cam.Approximation(roomPos, 1, true);
        }
    }
}
