using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Samurai : MonoBehaviour
{
    public bool isPlayerClose = false;
    public Transform playerTrackerLeft;
    public Transform playerTrackerRight;
    public float speed;

    public bool moving = false, attacking = false;

    private bool attackingTimer = false;
    private float timer = 1;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator an;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        an = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckPlayerPosition();
        AnimationCheck();

        if (attackingTimer)
        {
            timer += Time.deltaTime;
            int seconds = (int)timer % 60;
            if(seconds % 4 == 0)
            {
                timer = 1;
                attackingTimer = false;
            }
        }

        MovingOrAttacking();
    }

    private void CheckPlayerPosition()
    {
        isPlayerClose = (playerTrackerLeft.GetComponent<EntityEncounter>().isPlayerClose || playerTrackerRight.GetComponent<EntityEncounter>().isPlayerClose);

        if (playerTrackerLeft.GetComponent<EntityEncounter>().isPlayerClose ||
            playerTrackerRight.GetComponent<EntityEncounter>().closeToWall)
        {
            sr.flipX = true;
        }
        else if (playerTrackerRight.GetComponent<EntityEncounter>().isPlayerClose ||
            playerTrackerLeft.GetComponent<EntityEncounter>().closeToWall)
        {
            sr.flipX = false;
        }

        switch (isPlayerClose)
        {
            case true:
                if (attackingTimer)
                    attacking = false;
                else
                    attacking = true;
                moving = false;
                break;
            case false:
                moving = true;
                attacking = false;
                break;
        }
    }

    private void MovingOrAttacking()
    {
        if (moving)
        {
            float speedDelta = speed * Time.deltaTime;
            // Going Left
            if (sr.flipX)
            {
                transform.position = new Vector2(transform.position.x - speedDelta, transform.position.y);
            }
            else
            {
                transform.position = new Vector2(transform.position.x + speedDelta, transform.position.y);
            }
        }
        if (attacking)
        {
            attackingTimer = true;
        }
    }
    private void AttackingFinish()
    {
        attacking = false;
        GetComponent<HitBox>().Enabled = false;
    }
    private void AnimationCheck()
    {
        an.SetBool("Moving", moving);
        an.SetBool("Attacking", attacking);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if (collision.GetComponent<HitBox>().Enabled)
            {
                GetComponent<HitPoints>().SubtractHitPoints(5);
            }
            else
            {
                if (GetComponent<HitBox>().Enabled)
                {
                    collision.GetComponent<HitPoints>().SubtractHitPoints(4);
                }
                else
                {
                    collision.GetComponent<HitPoints>().SubtractHitPoints(2);
                }
            }
        }    
    }
}
