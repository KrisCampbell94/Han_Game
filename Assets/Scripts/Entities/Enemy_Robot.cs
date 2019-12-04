using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Robot : MonoBehaviour
{
    public float speed;
    private float timer = 1;

    public bool isPlayerClose = false, moving = false, attacking = false;
    public Transform attackLocation, playerTrackerLeft, playerTrackerRight;
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


        MovingOrAttacking();
    }

    private void CheckPlayerPosition()
    {
        isPlayerClose = (playerTrackerLeft.GetComponent<EntityEncounter>().isPlayerClose || playerTrackerRight.GetComponent<EntityEncounter>().isPlayerClose);

        if (playerTrackerLeft.GetComponent<EntityEncounter>().isPlayerClose ||
            (playerTrackerRight.GetComponent<EntityEncounter>().closeToWall || playerTrackerRight.GetComponent<EntityEncounter>().isEnemyClose))
        {
            sr.flipX = false;
        }
        else if (playerTrackerRight.GetComponent<EntityEncounter>().isPlayerClose ||
            (playerTrackerLeft.GetComponent<EntityEncounter>().closeToWall || playerTrackerLeft.GetComponent<EntityEncounter>().isEnemyClose))
        {
            sr.flipX = true;
        }

        switch (isPlayerClose)
        {
            case true:
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
            // Going Right
            if (!sr.flipX)
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
            timer += Time.deltaTime;
            int quarterSecond = (int)timer % 15;
            if (quarterSecond % 2 == 0)
            {
                StartAttacking();
                timer = 1;
            }

        }
    }
    private void AnimationCheck()
    {
        an.SetBool("Moving", moving);
        an.SetBool("Attacking", attacking);
    }
    void StartAttacking()
    {
        GameObject weapon = BulletFirePooler.sharedInstance.GetPooledBulletFires();
        if (weapon != null)
        {
            BulletFire weaponScript = weapon.GetComponent<BulletFire>();
            weaponScript.FlipX = !sr.flipX;

            weapon.transform.position = attackLocation.transform.position;
            weapon.SetActive(true);

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
