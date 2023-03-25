using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    private bool isEndPos = true;
    private Vector2 endPos, StartPos;
    private RectTransform rectTransform;
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        gameObject.SetActive(false);
    }

    private IEnumerator Movement()
    {
        if (isEndPos)
        {
            Vector3 startPosition = transform.position;
            for (float i = 0; i < 1; i += Time.deltaTime)
            {
                transform.position = Vector3.Lerp(startPosition, StartPos, EasingInverseSquared(i));
                yield return null;
            }
            isEndPos = false;
            StartCoroutine(Movement());
        }
        else
        {
            Vector3 startPosition = transform.position;
            for (float i = 0; i < 1; i += Time.deltaTime)
            {
                transform.position = Vector3.Lerp(startPosition, endPos, EasingInverseSquared(i));
                yield return null;
            }
            isEndPos = true;
            StartCoroutine(Movement());
        }
    }
    public void SetArrow(int x, int y)
    {
        gameObject.SetActive(true);
        StopAllCoroutines();
        rectTransform.localPosition = new Vector2(x, y);
        endPos = transform.position;
        StartPos = endPos - Vector2.down * 20;
        isEndPos = true;
        StartCoroutine(Movement());
    }
    private float EasingInverseSquared(float x)
    {
        return Mathf.Sin((x * Mathf.PI) / 2);
    }
}
