using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_BatNinja : MonoBehaviour
{
    public bool isPlayerClose = false;
    public Transform attackLocation;
    private float timer = 1;
    public Transform playerTrackerLeft;
    public Transform playerTrackerRight;
    public float speed;
    private GameObject getPlayer;
    private Vector2 playerPosition, moveToPlayer;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator an;

    // Start is called before the first frame update
    void Start()
    {
        getPlayer = GameObject.Find("Han_Player");
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        an = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        isPlayerClose = (playerTrackerLeft.GetComponent<EntityEncounter>().isPlayerClose || playerTrackerRight.GetComponent<EntityEncounter>().isPlayerClose);
        if (isPlayerClose)
        {
            if (playerTrackerLeft.GetComponent<EntityEncounter>().isPlayerClose)
            {
                sr.flipX = true;
            }
            else if (playerTrackerRight.GetComponent<EntityEncounter>().isPlayerClose)
            {
                sr.flipX = false;
            }
            timer += Time.deltaTime;
            int seconds = (int)timer % 60;

            // Every 2 seconds
            if (seconds % 3 == 0)
            {
                StartAttacking();
                timer = 1;
            }
        }
        else
        {
            ArielMovement();
        }
    }

    void StartAttacking()
    {
        GameObject weapon = NinjaStarPooler.sharedInstance.GetPooledNinjaStars();
        if (weapon != null)
        {
            NinjaStar weaponScript = weapon.GetComponent<NinjaStar>();
            weaponScript.FlipX = sr.flipX;

            weapon.transform.position = attackLocation.transform.position;
            weaponScript.FollowingSetup();
            weapon.SetActive(true);

        }
    }

    void ArielMovement()
    {
        playerPosition = getPlayer.transform.position;

        Vector3 directionToPlayer = (Vector2)transform.position - playerPosition;
        // Positives means face left, Negatives means face right
        sr.flipX = (directionToPlayer.x > 0);
        moveToPlayer = directionToPlayer;

        Vector2 speedDelta = moveToPlayer * speed * Time.deltaTime;
        
        timer += Time.deltaTime;
        int seconds = (int)timer % 60;
        if(seconds % 8 < 4)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y) - speedDelta;
        }
        else if (seconds % 8 > 3 && seconds % 8 < 7)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y);
        }
        else
        {
            timer = 1;
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (collision.GetComponent<HitBox>().Enabled)
            {
                GetComponent<HitPoints>().SubtractHitPoints(5);
            }
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {

    }
}
