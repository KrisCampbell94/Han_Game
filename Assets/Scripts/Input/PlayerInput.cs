using UnityEngine;
using static EntityMover;
using static Gravity;

public class PlayerInput : MonoBehaviour
{
	private EventManager eventManager;
	private Gravity gravity;
    private EntityMover entityMover;
    private EntityRotator entityRotator;

    private EntityEncounter encounter; //~

    private Direction direction;
	private Movement movement;

    private bool attackTimer = false;
    private float timer = 1; //~
	
	// Start is called before the first frame update
	void Start() {
		eventManager = GetComponent<EventManager>();
		gravity = GetComponent<Gravity>();
        entityMover = GetComponent<EntityMover>();
        entityRotator = GetComponent<EntityRotator>();

        // This grabs the gameobject within the player called playerRange,
        //  takes the script of that object in order to check whether an enemy is close or not.
        encounter = GameObject.Find("playerRange").GetComponent<EntityEncounter>();//~

        // Current input affecting movement
        // May differ fron actual movement in case of obstacles, being in the air, etc
        movement = Movement.Stopped;

		// Current input affecting direction
		// Likely matches actual direction, since turning around is not hindered by obstacles
		direction = Direction.Right;
	}

	private void FixedUpdate() {
		// Get inputs
		float horizontalMove = Input.GetAxis("Horizontal");
		float verticalMove = Input.GetAxis("Vertical");
		bool gravityButtonDown = Input.GetButton("Gravity");
		bool jumpButton = Input.GetButton("Jump");
        bool attackButton = Input.GetButtonDown("Attack");//~

        // If holding gravity key (shift)
        if (gravityButtonDown) {
			// Do gravity change
            if (!entityRotator.orienting)
            {
                GravityInput(horizontalMove, verticalMove);
            }
        } else {
			// Otherwise, normal movement
			MovementInput(horizontalMove, verticalMove);
			JumpInput(jumpButton);
            if (!attackTimer)
            {
                AttackInput(attackButton);
            }
            else
            {
                timer += Time.deltaTime;
                int seconds = (int)timer % 60;
                if(seconds % 2 == 0)
                {
                    timer = 1;
                    attackTimer = false;
                }
            }
            
		}
	}

	private void MovementInput(float horizontalMove, float verticalMove) {
		// Check current movement
		switch (movement) {
			case Movement.Stopped:
				// If stopped and input says move
				if (horizontalMove != 0) {
					// Change to moving and call event
					movement = Movement.Walking;
					eventManager.InvokeEvent("Input_Walking");
				}
				break;
			case Movement.Walking:
				// If walking and input says stop
				if (horizontalMove == 0) {
					// Change to stopped and call event
					movement = Movement.Stopped;
					eventManager.InvokeEvent("Input_Stopping");
				}
				break;
			case Movement.Running:
				// If running and input says stop
				if (horizontalMove == 0) {
					// Change to stopped and call event
					movement = Movement.Stopped;
					eventManager.InvokeEvent("Input_Stopping");
				}
				break;
		}

		// Check current direction
		switch (direction) {
			case Direction.Left:
				// If facing left and movement says right
				if (horizontalMove > 0) {
					// Change to right and call event
					direction = Direction.Right;
					eventManager.InvokeEvent("Input_TurnRight");
				}
				break;
			case Direction.Right:
				// If facing right and movement says left
				if (horizontalMove < 0) {
					// Change to left and call event
					direction = Direction.Left;
					eventManager.InvokeEvent("Input_TurnLeft");
				}
				break;
		}
	}

	private void JumpInput(bool jumpButton) {
		if (jumpButton) {
			eventManager.InvokeEvent("Input_Jump");
		}
        else
        {
            eventManager.InvokeEvent("Output_Jump");
        }
	}
    private void AttackInput(bool attackButton) //~
    {
        if (attackButton)
        {
            if (encounter.isEnemyClose)
            {
                eventManager.InvokeEvent("Attacking_Close");
            }
            else
            {
                eventManager.InvokeEvent("Attacking_Range");
                attackTimer = true;
            }
        }
    }

	private void GravityInput(float horizontalMove, float verticalMove) {
		// If vertical change
		if (verticalMove != 0) {
			// Positive, change to north
			if (verticalMove > 0) {
				eventManager.InvokeEvent("Input_Gravity_Flip");
			}
		}
		// If horizontal change
		else if (horizontalMove != 0) {
			// Positive, change to east
			if (horizontalMove > 0) {
				eventManager.InvokeEvent("Input_Gravity_RotateRight");
			}
			// Negative, change to west
			else if (horizontalMove < 0) {
				eventManager.InvokeEvent("Input_Gravity_RotateLeft");
			}
		}
	}


}
