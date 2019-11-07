using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Gravity;

public class MouseInput : MonoBehaviour
{
	// How far a drag needs to be before it counts
	public float dragThreshold = 100;

	// Player object
	private GameObject player;
	private Gravity playerGravity;

	// Self components
	private EventManager eventManager;

	// Mouse down stored values
	private float mouseDownX = 0;
	private float mouseDownY = 0;

	// Start is called before the first frame update
	void Start() {
		player = GameObject.FindWithTag("Player");
		playerGravity = player.GetComponent<Gravity>();

		eventManager = GetComponent<EventManager>();
	}

	private void OnMouseDown() {
		// Store pos on mouse down
		mouseDownX = Input.mousePosition.x;
		mouseDownY = Input.mousePosition.y;
	}

	private void OnMouseUp() {
		// Get pos on mouse up
		float mouseUpX = Input.mousePosition.x;
		float mouseUpY = Input.mousePosition.y;

		// Compare with mousedown pos
		float diffX = mouseDownX - mouseUpX;
		float diffY = mouseDownY - mouseUpY;

		// Get player's gravity direction
		GravityDirection playerGravityDirection = playerGravity.gravityDirection;

		// If player is sideways, swap X and Y
		if (playerGravityDirection == GravityDirection.East || playerGravityDirection == GravityDirection.West) {
			float tempX = diffX;
			diffX = diffY;
			diffY = tempX;
		}

		// Get absolute distance for comparisions
		float absDiffX = Mathf.Abs(diffX);
		float absDiffY = Mathf.Abs(diffY);

		// Y is pulled more than X
		if (absDiffY > absDiffX) {
			// Y is pulled enough
			if (absDiffY >= dragThreshold) {
				// If player is upsidedown or West, invert Y input
				if (playerGravityDirection == GravityDirection.North || playerGravityDirection == GravityDirection.West) {
					diffY *= -1;
				}

				// Invoke gravity input event
				if (diffY > 0) {
					eventManager.InvokeEvent("Input_Gravity_South");
				} else {
					eventManager.InvokeEvent("Input_Gravity_North");
				}
			}
		} else { // X is pulled more than Y
				 // X is pulled enough
			if (absDiffX >= dragThreshold) {
				// If player is upside down or East, invert X input
				if (playerGravityDirection == GravityDirection.North || playerGravityDirection == GravityDirection.East) {
					diffX *= -1;
				}

				// Invoke gravity input event
				if (diffX > 0) {
					eventManager.InvokeEvent("Input_Gravity_West");
				} else {
					eventManager.InvokeEvent("Input_Gravity_East");
				}
			}
		}
	}


}
