﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Ninja : MonoBehaviour
{
    public bool isPlayerClose = false;
    public Transform attackLocation;
    private float timer = 1;
    public Transform playerTrackerLeft;
    public Transform playerTrackerRight;

    private bool jumpFlip = true;
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
        else // Make the Ninja Jump
        {
            timer += Time.deltaTime;
            int seconds = (int)timer % 60;
            if(seconds == 5)
            {
                
                if (jumpFlip)
                    rb.velocity = new Vector2(-10f, 15f);
                else
                    rb.velocity = new Vector2(10f, 15f);
                jumpFlip = !jumpFlip;
                sr.flipX = !jumpFlip;

                timer = 1;
            }
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
