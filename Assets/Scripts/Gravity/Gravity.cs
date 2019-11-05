using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
	public enum GravityDirection { North, East, South, West }

	public float gravityMagnitude = 9.8f;
	public float timeToChangeGravity = 0.5f; // Seconds

	private EventManager eventManager;

	// Gravity direction relative to world
	private GravityDirection gravityDirection;
	// Same as gravityDirection but in vector form
	private Vector2 gravityDirectionVector;
	// Temp starting vector when changing gravity
	private Vector2 gravityStartVector;

	// Current gravity vector, will slowly change towards gravityDirectionVector
	private Vector2 gravityVector;

	// Start is called before the first frame update
	void Start() {
		eventManager = GetComponent<EventManager>();

		gravityDirection = GravityDirection.South;
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	private void FixedUpdate() {
		if (gravityVector != gravityDirectionVector) {
			float clamp = (Time.fixedDeltaTime / timeToChangeGravity);
			Vector2.Lerp(gravityStartVector, gravityDirectionVector, );


		}

		// Apply gravity in selected direction
		GetComponent<Rigidbody2D>().AddForce(gravityVector * gravityMagnitude);
	}


	public void ChangeGravity(GravityDirection newDirection) {
		gravityDirection = newDirection;

		// Store current gravityDirectionVector for gradual change
		gravityStartVector = gravityVector;

		// Set gravityDirectionVector based on gravityDirection
		switch (gravityDirection) {
			case GravityDirection.North:
				gravityDirectionVector = Vector2.up;
				break;
			case GravityDirection.East:
				gravityDirectionVector = Vector2.right;
				break;
			case GravityDirection.South:
			default:
				gravityDirectionVector = Vector2.down;
				break;
			case GravityDirection.West:
				gravityDirectionVector = Vector2.left;
				break;
		}

		eventManager.InvokeEvent("Mover_Gravity" + newDirection);
	}
}
