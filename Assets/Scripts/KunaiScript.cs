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
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(gameObject.tag == "EnemyWeapon" && collision.gameObject.tag == "Player")
        {
            if (!collision.gameObject.GetComponent<PlayerControllerScript>().isCloseAttacking)
            {
                collision.gameObject.GetComponent<HitPointScript>().SubtractHitPoints(1);
            }
            gameObject.SetActive(false);
        }
        if(gameObject.tag == "PlayerWeapon" && collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<HitPointScript>().SubtractHitPoints(3);
            gameObject.SetActive(false);
        }
    }
}
