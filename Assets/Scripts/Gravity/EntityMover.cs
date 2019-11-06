﻿using UnityEngine;
using static Gravity;

public class EntityMover : MonoBehaviour
{
    public enum Direction { Left, Right }
    public enum Movement { Stopped, Walking, Running }

    public float movementSpeed = 6.0f;
    public float runningMultiplier = 2;

    private EventManager eventManager;
    private Rigidbody2D rBody2D;

    // Current movement direction relative to self
    private Direction direction;

    // Current movement state
    private Movement movement;

    // Movement
    float movementMultiplier = 0; // 0 for stop, 1 for move, runningMultiplier for run
    float directionMultiplier = 1; // 1 for right, -1 for left
    float directionFlipper = 1; // 1 for normal (Gravity South or East), -1 for flipped (Gravity North or West)
    bool moveOnX = true;

    // Current grounded state
    private bool grounded;

    // Rotation Lerping
    public float timeToRotate = 0.5f; // Seconds
    private float startTime;
    private Vector3 startAngle;
    private float destAngleZ;

    // Start is called before the first frame update
    void Start()
    {
        eventManager = GetComponent<EventManager>();
        rBody2D = GetComponent<Rigidbody2D>();

        // Initial direction and movement
        SetDirection(Direction.Right);
        SetMovement(Movement.Stopped);

        // Add listeners to react to input
        eventManager.AddListener("Input_TurnLeft", () => SetDirection(Direction.Left));
        eventManager.AddListener("Input_TurnRight", () => SetDirection(Direction.Right));
        eventManager.AddListener("Input_Stopping", () => SetMovement(Movement.Stopped));
        eventManager.AddListener("Input_Walking", () => SetMovement(Movement.Walking));
        eventManager.AddListener("Input_Running", () => SetMovement(Movement.Running));

        // Add listeners to react to gravity change
        eventManager.AddListener("Gravity_North", () => RotateSelfToGravity(GravityDirection.North));
        eventManager.AddListener("Gravity_East", () => RotateSelfToGravity(GravityDirection.East));
        eventManager.AddListener("Gravity_South", () => RotateSelfToGravity(GravityDirection.South));
        eventManager.AddListener("Gravity_West", () => RotateSelfToGravity(GravityDirection.West));
    }

    private void FixedUpdate()
    {
        Debug.Log(transform.eulerAngles.z + " " + destAngleZ);

        // If not orientated right
        if (transform.eulerAngles.z != destAngleZ)
        {
            UpdateOrientation();
        }
        else
        {
            UpdateMovement();
        }
    }

    // Updates

    // TODO: Take current orientation into account?
    private void UpdateOrientation()
    {
        // Get new angle for this update
        Vector3 newAngle = startAngle;
        float rotateDelta = (Time.time - startTime) / timeToRotate;
        newAngle.z = Mathf.Lerp(startAngle.z, destAngleZ, rotateDelta);
        
        // Set transform to new angle
        transform.eulerAngles = newAngle;
    }

    private void UpdateMovement()
    {
        float moveDelta = movementSpeed * movementMultiplier * directionMultiplier * directionFlipper;

        // Update velocity
        if (moveOnX)
        {
            rBody2D.velocity = new Vector2(moveDelta, rBody2D.velocity.y);
        }
        else
        {
            rBody2D.velocity = new Vector2(rBody2D.velocity.x, moveDelta);
        }
    }

    // Events

    private void SetDirection(Direction newDirection)
    {
        direction = newDirection;

        // Set velocity direction
        switch (direction)
        {
            case Direction.Left:
                directionMultiplier = -1;
                break;
            case Direction.Right:
                directionMultiplier = 1;
                break;
        }
    }

    private void SetMovement(Movement newMovement)
    {
        movement = newMovement;

        // Set velocity multiplier
        switch (movement)
        {
            case Movement.Stopped:
                movementMultiplier = 0;
                break;
            case Movement.Walking:
                movementMultiplier = 1;
                break;
            case Movement.Running:
                movementMultiplier = runningMultiplier;
                break;
        }
    }

    private void RotateSelfToGravity(GravityDirection gravityDirection)
    {
        // Store current time and angle
        startTime = Time.time;
        startAngle = transform.eulerAngles;

        // Get new angle based on direction of gravity
        switch (gravityDirection)
        {
            case GravityDirection.North:
                destAngleZ = 180;
                moveOnX = true;
                directionFlipper = -1;
                break;
            case GravityDirection.East:
                destAngleZ = 90;
                moveOnX = false;
                directionFlipper = 1;
                break;
            case GravityDirection.South:
            default:
                destAngleZ = 0;
                moveOnX = true;
                directionFlipper = 1;
                break;
            case GravityDirection.West:
                destAngleZ = 270;
                moveOnX = false;
                directionFlipper = -1;
                break;
        }
    }
}
