using UnityEngine;
using static EntityMover;
using static Gravity;

public class PlayerInput : MonoBehaviour
{
    private EventManager eventManager;
    private Gravity gravity;

    private Direction direction;
    private Movement movement;
    private Jumping jumping;

    // Start is called before the first frame update
    void Start()
    {
        eventManager = GetComponent<EventManager>();
        gravity = GetComponent<Gravity>();

        // Current input affecting movement
        // May differ fron actual movement in case of obstacles, being in the air, etc
        movement = Movement.Stopped;

        // Current input affecting direction
        // Likely matches actual direction, since turning around is not hindered by obstacles
        direction = Direction.Right;


        // Current input affecting jumping
        jumping = Jumping.No;
    }

    private void FixedUpdate()
    {
        // Get inputs
        float horizontalMove = Input.GetAxis("Horizontal");
        float verticalMove = Input.GetAxis("Vertical");
        bool gravityButtonDown = Input.GetButton("Gravity");
        bool jumpButton = Input.GetButtonDown("Jump");

        // If holding gravity key (shift)
        if (gravityButtonDown)
        {
            // Do gravity change
            GravityInput(horizontalMove, verticalMove);
        }
        else
        {
            // Otherwise, normal movement
            MovementInput(horizontalMove, verticalMove);
            JumpInput(jumpButton);
        }
    }

    private void MovementInput(float horizontalMove, float verticalMove)
    {
        // Check current movement
        switch (movement)
        {
            case Movement.Stopped:
                // If stopped and input says move
                if (horizontalMove != 0)
                {
                    // Change to moving and call event
                    movement = Movement.Walking;
                    eventManager.InvokeEvent("Input_Walking");
                }
                break;
            case Movement.Walking:
                // If walking and input says stop
                if (horizontalMove == 0)
                {
                    // Change to stopped and call event
                    movement = Movement.Stopped;
                    eventManager.InvokeEvent("Input_Stopping");
                }
                break;
            case Movement.Running:
                // If running and input says stop
                if (horizontalMove == 0)
                {
                    // Change to stopped and call event
                    movement = Movement.Stopped;
                    eventManager.InvokeEvent("Input_Stopping");
                }
                break;
        }

        // Check current direction
        switch (direction)
        {
            case Direction.Left:
                // If facing left and movement says right
                if (horizontalMove > 0)
                {
                    // Change to right and call event
                    direction = Direction.Right;
                    eventManager.InvokeEvent("Input_TurnRight");
                }
                break;
            case Direction.Right:
                // If facing right and movement says left
                if (horizontalMove < 0)
                {
                    // Change to left and call event
                    direction = Direction.Left;
                    eventManager.InvokeEvent("Input_TurnLeft");
                }
                break;
        }
    }

    private void JumpInput(bool jumpButton)
    {
        switch (jumping)
        {
            case Jumping.No:
                if (jumpButton)
                {
                    jumping = Jumping.Yes;
                    eventManager.InvokeEvent("Input_JumpUp");
                }
                break;
            case Jumping.Yes:
                if (!jumpButton)
                {
                    jumping = Jumping.No;
                    eventManager.InvokeEvent("Input_FallDown");
                }
                break;
        }
    }

    private void GravityInput(float horizontalMove, float verticalMove)
    {
        // If vertical change
        if (verticalMove != 0)
        {
            // Positive, change to north
            if (verticalMove > 0 && gravity.gravityDirection != GravityDirection.North)
            {
                eventManager.InvokeEvent("Input_Gravity_North");
            }
            // Negative, change to south
            else if (verticalMove < 0 && gravity.gravityDirection != GravityDirection.South)
            {
                eventManager.InvokeEvent("Input_Gravity_South");
            }
        }
        // If horizontal change
        else if (horizontalMove != 0)
        {
            // Positive, change to east
            if (horizontalMove > 0 && gravity.gravityDirection != GravityDirection.East)
            {
                eventManager.InvokeEvent("Input_Gravity_East");
            }
            // Negative, change to west
            else if (horizontalMove < 0 && gravity.gravityDirection != GravityDirection.West)
            {
                eventManager.InvokeEvent("Input_Gravity_West");
            }
        }
    }


}
