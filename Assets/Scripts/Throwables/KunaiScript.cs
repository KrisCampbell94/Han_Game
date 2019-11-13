using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KunaiScript : MonoBehaviour
{
    public bool isRight = true; // Don't Delete YET
    public bool onMoveX = true;

    // In relation to the player's direction and gravity movement
    public enum TurningAndGravity { isRightMoveX, isLeftMoveX, isRightMoveY, isLeftMoveY }
    // Note: To decrease enumerate values, 
    //  directionFlip is in relation to the EntityMover.directionFlip
    //  In context, 4 values could be 8.
    public float directionFlip = 1, speed;

    public TurningAndGravity turningAndGravity;

    // Start is called before the first frame update
    void Start()
    {
        turningAndGravity = TurningAndGravity.isRightMoveX;
    }

    // Update is called once per frame
    void Update()
    {
        Movement(turningAndGravity);
    }
    private void Movement(TurningAndGravity tAndG)
    {
        // Set the speed delta based on the speed, time and direction flip
        float speedDelta = speed * Time.deltaTime * directionFlip;
        switch (tAndG)
        {
            // Change the flipX value,
            //  increase/decrease the x/y position using the speed delta
            case TurningAndGravity.isRightMoveX:
                GetComponent<SpriteRenderer>().flipX = false;
                transform.position = new Vector2(transform.position.x + speedDelta, transform.position.y);
                break;
            case TurningAndGravity.isRightMoveY:
                GetComponent<SpriteRenderer>().flipX = false;
                transform.position = new Vector2(transform.position.x, transform.position.y + speedDelta);
                break;
            case TurningAndGravity.isLeftMoveX:
                GetComponent<SpriteRenderer>().flipX = true;
                transform.position = new Vector2(transform.position.x - speedDelta, transform.position.y);
                break;
            case TurningAndGravity.isLeftMoveY:
                GetComponent<SpriteRenderer>().flipX = true;
                transform.position = new Vector2(transform.position.x, transform.position.y - speedDelta);
                break;
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(gameObject.tag == "EnemyWeapon" && collision.gameObject.tag == "Player")
        {
            if (!collision.gameObject.GetComponent<PlayerControllerScript>().isCloseAttacking)
            {
                collision.gameObject.GetComponent<HitPoints>().SubtractHitPoints(1);
            }
            gameObject.SetActive(false);
        }
        if(gameObject.tag == "PlayerWeapon" && collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<HitPoints>().SubtractHitPoints(3);
            gameObject.SetActive(false);
        }
    }
}
