using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraScript : MonoBehaviour
{
    private Map map;
    public Camera actualCamera;

    [HideInInspector]
    public bool isKinematic = false;
    private float time = 0;
    private Vector3 startCameraPos;
    private static int sizeForCameraMovement = Screen.width / 20;

    bool onlyOne = true;

    private float maxPosX = 0, minPosX = 0, maxPosY = 0, minPosY = 0;
    private void Awake()
    {
        map = GameObject.Find("Map").GetComponent<Map>();
        actualCamera = GetComponent<Camera>();
    }
    private void Update()
    {
        if (!isKinematic)
        {
            if (Input.mousePosition.x >= Screen.width - sizeForCameraMovement && actualCamera.ScreenToWorldPoint(new Vector3(Screen.mainWindowPosition.x, Screen.mainWindowPosition.y)).x <= maxPosX)
            {
                transform.position = Vector3.MoveTowards(transform.position, transform.position + Vector3.right, (Time.deltaTime / 2) * (Input.mousePosition.x - Screen.width + sizeForCameraMovement));
            }
            else if (Input.mousePosition.x <= sizeForCameraMovement && actualCamera.ScreenToWorldPoint(new Vector3(Screen.mainWindowPosition.x, Screen.mainWindowPosition.y)).x >= minPosX)
            {
                transform.position = Vector3.MoveTowards(transform.position, transform.position + Vector3.left, (Time.deltaTime / 2) * (sizeForCameraMovement - Input.mousePosition.x));
            }

            if (Input.mousePosition.y >= Screen.height - sizeForCameraMovement && actualCamera.ScreenToWorldPoint(new Vector3(Screen.mainWindowPosition.x, Screen.mainWindowPosition.y)).y <= maxPosY)
            {
                transform.position = Vector3.MoveTowards(transform.position, transform.position + Vector3.up, (Time.deltaTime / 2) * (Input.mousePosition.y - Screen.height + sizeForCameraMovement));
            }
            else if (Input.mousePosition.y <= sizeForCameraMovement && actualCamera.ScreenToWorldPoint(new Vector3(Screen.mainWindowPosition.x, Screen.mainWindowPosition.y)).y >= minPosY)
            {
                transform.position = Vector3.MoveTowards(transform.position, transform.position + Vector3.down, (Time.deltaTime / 2) * (sizeForCameraMovement - Input.mousePosition.y));
            }
        }
    }

    public void coordinatesReceived(float maxPosX, float minPosX, float maxPosY, float minPosY)
    {
        this.maxPosX = maxPosX;
        this.minPosX = minPosX;
        this.maxPosY = maxPosY;
        this.minPosY = minPosY;
    }

    public bool Approximation(Vector3 targetPos, float maxApproximation, bool isRoomEvent)
    {
        if (onlyOne)
        {
            startCameraPos = Camera.main.transform.position;
            time = 0;
            onlyOne = false;
        }
        time += Time.deltaTime;
        transform.position = Vector3.Lerp(startCameraPos, targetPos, time / (isRoomEvent ? 1 : 1.5f));
        transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, -15);
        GetComponent<Camera>().orthographicSize -= EasingInverseSquared(time / 3);
        if (Camera.main.orthographicSize <= maxApproximation)
        {
            Camera.main.orthographicSize = 15;
            isKinematic = false;
            if (isRoomEvent)
            {
                map.NextRoom();
            }
            onlyOne = true;
            return false;
        }
        else
        {
            isKinematic = true;
        }
        return true;
    }

    private float EasingInverseSquared(float x)
    {
        return x * x;
    }
}
