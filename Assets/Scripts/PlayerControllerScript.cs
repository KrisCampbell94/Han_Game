using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerScript : MonoBehaviour
{
    public bool grounded = false;
    public Transform groundCheck;
    public float groundRadius = 0.2f;
    public LayerMask whatIsGround;
    public float maxJump = 16.0f;

    public float movementSpeed = 6.0f;
    private float horizontalMove;
    private int horizontalTapCount = 0;
    private float horizontalTapCooler = 0.5f;
    public bool isRunning = false;

    public bool isAttacking = false;
    public Transform attackLocation;

    public bool isGravity = false;
    public float gravityRotationSpeed = 3.0f;
    private GameObject stage;
    public List<Vector2> stage_Distance = new List<Vector2>();
    private int iterationCount = -1;

    public int hitPoints = 10;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        stage = GameObject.FindWithTag("Rotatable");
        foreach (Transform child in stage.transform)
        {
            stage_Distance.Add(stage.transform.position - child.position);
            //Debug.Log(rotatable.transform.position - child.position);
        }

    }

    void FixedUpdate()
    {
        // Grounded Setup
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
        // Falling Setup + Jumping Setup Part 1
        if (!grounded && !Input.GetButton("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y - 0.5f);
        }

        // Movement Setup
        horizontalMove = Input.GetAxis("Horizontal");
        //	Multi-Tap System
        if (Input.GetButtonDown("Horizontal"))
        {
            if (horizontalTapCooler > 0f && horizontalTapCount == 2)
            {
                // Double Tap Running
                isRunning = true;
            }
            else
            {
                horizontalTapCooler = 0.5f;
                horizontalTapCount++;
            }
        }
        if (horizontalTapCooler > 0f)
        {
            horizontalTapCooler -= 0.5f * Time.deltaTime;
            if (horizontalTapCooler < 0f)
            {
                horizontalTapCooler = 0f;
            }
        }
        else
        {
            horizontalTapCount = 0;
        }
        // The Effects of the Movement Setup as well as the Multi-Tap System
        if (!isGravity)
        {
            if (!isRunning)
            {
                rb.velocity = new Vector2(horizontalMove * movementSpeed, rb.velocity.y);
            }
            else if (isRunning && horizontalMove != 0)
            {
                rb.velocity = new Vector2((horizontalMove * 4) * movementSpeed, rb.velocity.y);
            }
            else if (isRunning && horizontalMove == 0)
            {
                isRunning = false;
            }
        }
        // Character Rotation
        if (horizontalMove < 0 && transform.rotation.y != 180)
        {
            transform.eulerAngles = new Vector2(0, 180);
        }
        else if (horizontalMove > 0 && transform.rotation.y != 0)
        {
            transform.eulerAngles = new Vector2(0, 0);
        }

        // Attacking Setup
        if (Input.GetButtonDown("Attack"))
        {
            isAttacking = true;
            // KUNAI Weapon Setup
            GameObject weaponA = ObjectPoolerScript.sharedInstance.GetPooledObject();
            if(weaponA != null)
            {
                KunaiScript weaponAScript = weaponA.GetComponent<KunaiScript>();
                weaponAScript.isRight = (transform.eulerAngles.y == 0);
                weaponA.transform.position = attackLocation.transform.position;
                weaponA.SetActive(true);
            }
            // NOTES:
            //  There's another Box Collider 2D which is gonna be whenever an enemy is close, he'll attack with the sword instead of his kunai
        }

        // Gravity Setup Part 1
        isGravity = Input.GetButton("Gravity");
        if (isGravity)
        {
            // Freeze the character in place; both X and Y axis
            rb.constraints = RigidbodyConstraints2D.FreezePosition;
            // If Player moves left or right
            if (horizontalMove != 0)
            {
                stage.transform.Rotate(
                    stage.transform.rotation.x,
                    stage.transform.rotation.y, 
                    Mathf.RoundToInt(stage.transform.rotation.z + (horizontalMove * gravityRotationSpeed)), 
                    Space.World);
            }
        }
        else
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        // Hit Point Setup
        switch (hitPoints)
        {
            case 0:
                sr.color = new Color(0.5f, 0, 0);
                break;
            case 1:
            case 2:
                sr.color = new Color(0.6f, 0.2f, 0.2f);
                break;
            case 3:
            case 4:
                sr.color = new Color(0.7f, 0.4f, 0.4f); 
                break;
            case 5:
            case 6:
                sr.color = new Color(0.8f, 0.6f, 0.6f);
                break;
            case 7:
            case 8:
                sr.color = new Color(0.9f, 0.8f, 0.8f);
                break;
            case 9:
            case 10:
                sr.color = new Color(1, 1, 1);
                break;
        }
    }

    void Update()
    {
        // Jumping Setup Part 2
        if (Input.GetButtonDown("Jump") && grounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, maxJump);
        }

        // Move the entire Stage along with character
        //  AKA Gravity Setup Part 2 
        iterationCount++;
        stage.transform.position = transform.position;
        for (int i = 0; i < stage.transform.childCount; i++)
        {
            if (!isGravity)
            {
                Vector2 testPosition = ((Vector2)transform.position - stage_Distance[i]);
                if (iterationCount % 2 == 1)
                {
                    stage_Distance[i] = testPosition;
                    testPosition = ((Vector2)transform.position - stage_Distance[i]);
                }
                else if (iterationCount % 2 == 0 && iterationCount > 0)
                {
                    stage_Distance[i] = testPosition;
                    testPosition = ((Vector2)transform.position - stage_Distance[i]);
                }
                stage.transform.GetChild(i).position = testPosition;
                stage_Distance[i] = testPosition;
            }
            else
            {
                stage_Distance[i] = stage.transform.GetChild(i).position;
            }
        }

        // Animation Setups
        anim.SetFloat("SpeedHorizontal", Mathf.Abs(horizontalMove));
        anim.SetFloat("SpeedVertical", rb.velocity.y);
        anim.SetBool("Grounded", grounded);
        anim.SetBool("Running", isRunning);
        anim.SetBool("Attacking", isAttacking);
        anim.SetBool("Gravity", isGravity);
    }
    // For Animation Event
    void AttackingFinish()
    {
        isAttacking = false;
    }
}
