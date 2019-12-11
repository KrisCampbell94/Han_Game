using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingDamageVisual : MonoBehaviour
{
    private TextMeshPro textMesh;
    private float disappearTimer = 1f;
    private Color textColor;

    // Start is called before the first frame update
    void Start()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
        textColor = textMesh.color;
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0, 1) * Time.deltaTime;
        transform.rotation = GameObject.FindWithTag("Player").transform.rotation;
        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0)
        {
            float disappearSpeed = 3f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;
            if (textColor.a <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
