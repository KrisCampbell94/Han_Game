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
        Moving();

        AnimationCheck();
    }

    private void CheckPlayerPosition()
    {
        isPlayerClose = (playerTrackerLeft.GetComponent<PlayerEncounterScript>().isPlayerClose || playerTrackerRight.GetComponent<PlayerEncounterScript>().isPlayerClose);

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

    private void Moving()
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
    }
    private void AttackingFinish()
    {
        attacking = false;
    }
    private void AnimationCheck()
    {
        an.SetBool("Moving", moving);
        an.SetBool("Attacking", attacking);
    }
}
