﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_NinjaScript : MonoBehaviour
{
    public bool isPlayerClose = false;
    public Transform attackLocation;
    private float timer = 0.0f;
    public Transform playerTrackerLeft;
    public Transform playerTrackerRight;

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        isPlayerClose = (playerTrackerLeft.GetComponent<PlayerEncounterScript>().isPlayerClose || playerTrackerRight.GetComponent<PlayerEncounterScript>().isPlayerClose);
        if (isPlayerClose)
        {
            if (playerTrackerLeft.GetComponent<PlayerEncounterScript>().isPlayerClose)
            {
                sr.flipX = true;
            }
            else if (playerTrackerRight.GetComponent<PlayerEncounterScript>().isPlayerClose)
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
    }

    void StartAttacking()
    {
        GameObject weapon = NinjaStarPoolerScript.sharedInstance.GetPooledNinjaStars();
        if (weapon != null)
        {
            KunaiScript weaponScript = weapon.GetComponent<KunaiScript>();
            weaponScript.isRight = (!sr.flipX);
            weapon.transform.position = attackLocation.transform.position;
            weapon.SetActive(true);
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
		if (collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.GetComponent<PlayerControllerScript>().isCloseAttacking)
            {
                GetComponent<HitPointScript>().SubtractHitPoints(5);
            }
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
}