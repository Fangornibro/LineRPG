using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    private float dissapearTimer = 0.3f;
    [HideInInspector]
    public Color textColor;
    private TextMeshPro text;
    private void Start()
    {
        text = GetComponent<TextMeshPro>();
    }
    private void Update()
    {
        text.color = textColor;
        float moveYSpeed = 20f;
        transform.position += new Vector3(0, moveYSpeed) * Time.deltaTime;
        dissapearTimer -= Time.deltaTime;
        if (dissapearTimer <= 0)
        {
            textColor.a -= 3 * Time.deltaTime;
            transform.localScale -= Vector3.one * Time.deltaTime;
            if (textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
