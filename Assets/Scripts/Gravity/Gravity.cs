using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
	public enum GravityDirection { North, East, South, West }

	public float gravityMagnitude = 9.8f;

	private EventManager eventManager;
	private Rigidbody2D rigidbody2D;

	// Current gravity vector
	private Vector2 gravityVector;

	// Start is called before the first frame update
	void Start() {
		eventManager = GetComponent<EventManager>();
		rigidbody2D = GetComponent<Rigidbody2D>();
	}

	private void FixedUpdate() {
		// Apply gravity in selected direction
		rigidbody2D.AddForce(gravityVector * gravityMagnitude);
	}

	public void SetGravityDirection(GravityDirection newGravityDirection) {
		// Set gravityDirectionVector based on gravityDirection
		switch (newGravityDirection) {
			case GravityDirection.North:
				gravityVector = Vector2.up;
				break;
			case GravityDirection.East:
				gravityVector = Vector2.right;
				break;
			case GravityDirection.South:
			default:
				gravityVector = Vector2.down;
				break;
			case GravityDirection.West:
				gravityVector = Vector2.left;
				break;
		}

		eventManager.InvokeEvent("Gravity_" + newGravityDirection);
	}
}
