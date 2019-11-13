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
    public bool isCloseAttacking = false;
    private BoxCollider2D closeAttackCollider;
    public bool isEnemyClose = false;
    public Transform enemyTracker;
    public Transform attackLocation;

    public bool isGravity = false;
    public float gravityRotationSpeed = 3.0f;
    private GameObject stage;
    public List<Vector2> stage_Distance = new List<Vector2>();
    private int iterationCount = -1;

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
        closeAttackCollider = gameObject.AddComponent(typeof(BoxCollider2D)) as BoxCollider2D;
        closeAttackCollider.enabled = false;
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
                //isRunning = true;
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

		isEnemyClose = enemyTracker.GetComponent<PlayerEncounterScript>().isEnemyClose;
        // Attacking Setup
        if (Input.GetButtonDown("Attack"))
        {
            // KUNAI Weapon Setup
            if (!isEnemyClose)
            {
                isAttacking = true;
                GameObject weaponA = KunaiPoolerScript.sharedInstance.GetPooledKunais();
                if (weaponA != null)
                {
                    KunaiScript weaponAScript = weaponA.GetComponent<KunaiScript>();
                    weaponAScript.isRight = (transform.eulerAngles.y == 0);
                    weaponA.transform.position = attackLocation.transform.position;
                    weaponA.SetActive(true);
                }
            }
            else
            {
                isCloseAttacking = true;
                CreateCloseAttackCollider();
            }
        }

        // Gravity Setup Part 1
        isGravity = Input.GetButton("Gravity");
        if (isGravity)
        {
            // Freeze the character in place; both X and Y axis
            rb.constraints = RigidbodyConstraints2D.FreezePosition;
            // If Player moves left or right
            if (Input.GetButtonDown("Horizontal") && horizontalMove > 0)
            {
                stage.transform.eulerAngles = new Vector3(0, 0, stage.transform.eulerAngles.z + 90);
            }
            else if (Input.GetButtonDown("Horizontal") && horizontalMove < 0)
            {
                stage.transform.eulerAngles = new Vector3(0, 0, stage.transform.eulerAngles.z - 90);
            }
        }
        else
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    void Update()
    {
		// Animation Setups
        anim.SetFloat("SpeedHorizontal", Mathf.Abs(horizontalMove));
        anim.SetFloat("SpeedVertical", rb.velocity.y);
        anim.SetBool("Grounded", grounded);
        anim.SetBool("Running", isRunning);
        anim.SetBool("Attacking", isAttacking);
        anim.SetBool("CloseAttacking", isCloseAttacking);
        anim.SetBool("Gravity", isGravity);
		
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
    }
    // For Animation Event
    void AttackingFinish()
    {
        isAttacking = false;
    }
    void CloseAttackingFinish()
    {
        closeAttackCollider.enabled = false;
        isCloseAttacking = false; 
    }
    void CreateCloseAttackCollider()
    {
        closeAttackCollider.enabled = true;
		closeAttackCollider.isTrigger = true;
        closeAttackCollider.offset = new Vector2(2, 2);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {

    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy" && !isCloseAttacking)
        {
            GetComponent<HitPoints>().SubtractHitPoints(4);
        }
        
    }
}
