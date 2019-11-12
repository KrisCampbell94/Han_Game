using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KunaiScript : MonoBehaviour
{
    public bool isRight = true; // Don't Delete YET
    public bool onMoveX = true;

    public enum TurningAndGravity { isRightMoveX, isLeftMoveX, isRightMoveY, isLeftMoveY }
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
        float speedDelta = speed * Time.deltaTime * directionFlip;
        switch (tAndG)
        {
            // WORK ON THE ROTATION OF THE KUNAI
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
