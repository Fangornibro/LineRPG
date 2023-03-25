using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class CameraScript : MonoBehaviour
{

    [HideInInspector] public Camera actualCamera;

    [HideInInspector] public bool isKinematic = false;

    private static int sizeForCameraMovement = Screen.width / 20;

    private float maxPosX = 0, minPosX = 0, maxPosY = 0, minPosY = 0;

    private bool isApproximation = false;

    [Space]
    [Space]
    [Header("Initialisations")]
    [SerializeField] private Map map;
    private void Awake()
    {
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

    public void StartApproximationCoroutine(Vector3 targetPos, float maxApproximation, bool isRoomEvent)
    {
        StartCoroutine(Approximation(targetPos, maxApproximation, isRoomEvent));
    }

    private IEnumerator Approximation(Vector3 targetPos, float maxApproximation, bool isRoomEvent)
    {
        if (!isApproximation)
        {
            isKinematic = true;
            isApproximation = true;
            Vector3 startPosition = transform.position;
            targetPos = new Vector3(targetPos.x, targetPos.y, -15);
            Camera camera = GetComponent<Camera>();
            float startApproximation = camera.orthographicSize;
            for (float i = 0; i < 1; i += Time.deltaTime)
            {
                transform.position = Vector3.Lerp(startPosition, targetPos, EasingInverseSquared(i));
                camera.orthographicSize = Mathf.Lerp(startApproximation, maxApproximation, i);
                yield return null;
            }
            Camera.main.orthographicSize = 15;
            if (isRoomEvent)
            {
                map.NextRoom();
            }
            else
            {
                isKinematic = false;
            }
            isApproximation = false;
        }
        
    }
    private float EasingInverseSquared(float x)
    {
        return 1-(1-x)*(1-x);
    }
}
