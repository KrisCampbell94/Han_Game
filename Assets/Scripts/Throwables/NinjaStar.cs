using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaStar : MonoBehaviour
{
    public float speed;
    public bool FlipX = false;
    private SpriteRenderer sprite;

    private GameObject getPlayer;
    private Vector2 playerPosition, moveToPlayer;

    // Start is called before the first frame update
    void Start()
    {
        getPlayer = GameObject.Find("Han_Player");
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        SetPlayerPosition();
        sprite.flipX = FlipX;
        FollowMovement();
    }
    public void SetPlayerPosition()
    {
        playerPosition = getPlayer.transform.position;
        Debug.Log(playerPosition);
    }
    public void FollowingSetup()
    {
        Vector3 directionToPlayer = (Vector2)transform.position - playerPosition;
        moveToPlayer = directionToPlayer;
        float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
        GetComponent<Rigidbody2D>().rotation = angle;
    }
    private void FollowMovement()
    {
        Vector2 speedDelta = moveToPlayer * speed * Time.deltaTime;
        transform.position = new Vector2(transform.position.x, transform.position.y) - speedDelta;
    }
    private void StraightMovement()
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
