using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundsChecker : MonoBehaviour
{
    private Vector3 playerPosition;
    private bool playerInBounds;
    // Start is called before the first frame update
    void Start()
    {
        playerPosition = Vector3.zero;
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerInBounds = true;
            playerPosition = collision.gameObject.transform.position;
        } 
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerInBounds = false;
            collision.gameObject.transform.position = playerPosition;
        }
        if (collision.gameObject.tag == "Enemy")
        {
            collision.GetComponent<HitPoints>().hitPoints = 0;
        }

    }
}
