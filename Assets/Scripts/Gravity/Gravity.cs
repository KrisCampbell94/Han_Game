using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
	public enum GravityDirection { North, East, South, West }

	public float gravityMagnitude = 9.8f;

	private EventManager eventManager;
	private Rigidbody2D rBody2D;

	// Current gravity direction and vector
	public GravityDirection gravityDirection { get; private set; }
	public Vector2 gravityVector { get; private set; }

	// Start is called before the first frame update
	void Start() {
		eventManager = GetComponent<EventManager>();
		rBody2D = GetComponent<Rigidbody2D>();

		// Set initial gravity
		SetGravityDirection(GravityDirection.South);

		// Change gravity direction based on input events
		eventManager.AddListener("Input_Gravity_North", () => SetGravityDirection(GravityDirection.North));
		eventManager.AddListener("Input_Gravity_East", () => SetGravityDirection(GravityDirection.East));
		eventManager.AddListener("Input_Gravity_South", () => SetGravityDirection(GravityDirection.South));
		eventManager.AddListener("Input_Gravity_West", () => SetGravityDirection(GravityDirection.West));
	}

	private void FixedUpdate() {
		// Apply gravity in selected direction
		rBody2D.AddForce(gravityVector * gravityMagnitude);
	}

	public void SetGravityDirection(GravityDirection newGravityDirection) {
		// If already set to this direction, return
		if (gravityDirection == newGravityDirection) {
			return;
		}

		// Save new direction
		gravityDirection = newGravityDirection;

		// Set gravityDirectionVector based on gravityDirection
		switch (gravityDirection) {
			case GravityDirection.North:
				gravityVector = Vector2.up;
				break;
			case GravityDirection.East:
				gravityVector = Vector2.right;
				break;
			case GravityDirection.South:
				gravityVector = Vector2.down;
				break;
			case GravityDirection.West:
				gravityVector = Vector2.left;
				break;
		}

		// Invoke event to indicate that gravity has changed
		eventManager.InvokeEvent("Gravity_" + gravityDirection);
	}
}
