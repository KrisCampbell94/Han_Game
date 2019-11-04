using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KunaiScript : MonoBehaviour
{
    public bool isRight = true;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isRight)
        {
            transform.position = new Vector2(transform.position.x + (speed * Time.deltaTime), transform.position.y);
        }
        else
        {
            transform.eulerAngles = new Vector2(0, 180);
            transform.position = new Vector2(transform.position.x - (speed * Time.deltaTime), transform.position.y);
        }
    }
}
