﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaStar : MonoBehaviour
{
    public float speed;
    public bool FlipX = false;
    private SpriteRenderer sprite;
    
    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        sprite.flipX = FlipX;
        Movement();
    }
    private void Movement()
    {
        float speedDelta = speed * Time.deltaTime;
        switch (sprite.flipX)
        {
            case true:
                transform.position = new Vector2(transform.position.x - speedDelta, transform.position.y);
                break;
            case false:
                transform.position = new Vector2(transform.position.x + speedDelta, transform.position.y);
                break;
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!collision.GetComponent<HitBox>().Enabled)
            {
                collision.gameObject.GetComponent<HitPoints>().SubtractHitPoints(1);
            }
            gameObject.SetActive(false);
        }
    }
}
