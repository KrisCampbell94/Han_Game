﻿using UnityEngine;
using static Gravity;

public class EntityMover : MonoBehaviour
{
	public enum Direction { Left, Right }
	public enum Movement { Stopped, Walking, Running }

	public float movementSpeed = 6.0f;
	public float runningMultiplier = 2;
	public float jumpForce = 8;
    
    private EventManager eventManager;
	private Rigidbody2D rBody2D;
	private GroundChecker groundChecker;
    private EntityRotator entityRotator;

	// Current movement direction relative to self
	private Direction direction;

	// Current movement state
	private Movement movement;

	// Movement
	float movementMultiplier = 0; // 0 for stop, 1 for move, runningMultiplier for run
	float directionMultiplier = 1; // 1 for right, -1 for left
	float directionFlipper = 1; // 1 for normal (Gravity South or East), -1 for flipped (Gravity North or West)
	float jumpFlipper = 1;
	bool moveOnX = true;
    
	// Start is called before the first frame update
	void Start() {
		eventManager = GetComponent<EventManager>();
		rBody2D = GetComponent<Rigidbody2D>();
		groundChecker = GetComponent<GroundChecker>();
        entityRotator = GetComponent<EntityRotator>();

        // Initial direction and movement
        SetDirection(Direction.Right);
		SetMovement(Movement.Stopped);

		// Add listeners to react to input
		eventManager.AddListener("Input_TurnLeft", () => SetDirection(Direction.Left));
		eventManager.AddListener("Input_TurnRight", () => SetDirection(Direction.Right));
		//
		eventManager.AddListener("Input_Stopping", () => SetMovement(Movement.Stopped));
		eventManager.AddListener("Input_Walking", () => SetMovement(Movement.Walking));
		eventManager.AddListener("Input_Running", () => SetMovement(Movement.Running));
		//
		eventManager.AddListener("Input_Jump", Jump);

		// Add listeners to react to gravity change
		eventManager.AddListener("Gravity_North", () => RotateMovementToGravity(GravityDirection.North));
		eventManager.AddListener("Gravity_East", () => RotateMovementToGravity(GravityDirection.East));
		eventManager.AddListener("Gravity_South", () => RotateMovementToGravity(GravityDirection.South));
		eventManager.AddListener("Gravity_West", () => RotateMovementToGravity(GravityDirection.West));
	}

	private void FixedUpdate() {
        // If no rotator, just update movement
        // If has rotator, make sure it's not orienting
        if (entityRotator == null || !entityRotator.orienting) {
			UpdateMovement();
		}
	}

	// Updates
    
	private void UpdateMovement() {
		float moveDelta = movementSpeed * movementMultiplier * directionMultiplier * directionFlipper;

		// Update velocity
		if (moveOnX) {
			rBody2D.velocity = new Vector2(moveDelta, rBody2D.velocity.y);
		} else {
			rBody2D.velocity = new Vector2(rBody2D.velocity.x, moveDelta);
		}
	}

	// Events

	private void SetDirection(Direction newDirection) {
		direction = newDirection;

		// Set velocity direction
		switch (direction) {
			case Direction.Left:
				directionMultiplier = -1;
				break;
			case Direction.Right:
				directionMultiplier = 1;
				break;
		}
	}

	private void SetMovement(Movement newMovement) {
		movement = newMovement;

		// Set velocity multiplier
		switch (movement) {
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

	private void Jump() {
		// If on ground,
		if (groundChecker.grounded) {
			// Set jump velocity
			float jumpDelta = jumpForce * jumpFlipper;

			// Jump opposing to gravity direction
			if (moveOnX) {
				rBody2D.velocity = new Vector2(rBody2D.velocity.x, jumpDelta);
			} else {
				rBody2D.velocity = new Vector2(jumpDelta, rBody2D.velocity.y);
			}
		}
	}

	private void RotateMovementToGravity(GravityDirection gravityDirection) {
		// Depending on direction, set
		// Whether movement is on x or y
		// Whether movement direction is flipped or not
		// Whether jump direction is flipped or not
		switch (gravityDirection) {
			case GravityDirection.North:
				moveOnX = true;
				directionFlipper = -1;
				jumpFlipper = -1;
				break;
			case GravityDirection.East:
				moveOnX = false;
				directionFlipper = 1;
				jumpFlipper = -1;
				break;
			case GravityDirection.South:
			default:
                moveOnX = true;
				directionFlipper = 1;
				jumpFlipper = 1;
				break;
			case GravityDirection.West:
                moveOnX = false;
				directionFlipper = -1;
				jumpFlipper = 1;
				break;
		}
	}
}
