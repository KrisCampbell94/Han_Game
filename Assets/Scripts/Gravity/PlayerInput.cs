using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInput : MonoBehaviour
{
	public enum Direction { Left, Right }
	public enum Movement { Stopped, Walking, Running }

	private Direction direction;
	private Movement movement;

    // Start is called before the first frame update
    void Start()
    {
		movement = Movement.Stopped;
		direction = Direction.Right;

		AddEventDebug("Stopping");
		AddEventDebug("Walking");
		AddEventDebug("Running");

		AddEventDebug("TurnLeft");
		AddEventDebug("TurnRight");
	}

	// Update is called once per frame
	void Update()
    {

    }

	private void FixedUpdate() {
		float horizontalMove = Input.GetAxis("Horizontal");

		// Check current movement
		switch (movement) {
			case Movement.Stopped:
				// If stopped and input says move
				if (horizontalMove != 0) {
					// Change to moving and call event
					movement = Movement.Walking;
					EventManager.InvokeEvent("Walking");
				}
				break;
			case Movement.Walking:
				// If walking and input says stop
				if (horizontalMove == 0) {
					// Change to stopped and call event
					movement = Movement.Stopped;
					EventManager.InvokeEvent("Stopping");
				}
				break;
			case Movement.Running:
				// If running and input says stop
				if (horizontalMove == 0) {
					// Change to stopped and call event
					movement = Movement.Stopped;
					EventManager.InvokeEvent("Stopping");
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
					EventManager.InvokeEvent("TurnRight");
				}
				break;
			case Direction.Right:
				// If facing right and movement says left
				if (horizontalMove < 0) {
					// Change to left and call event
					direction = Direction.Left;
					EventManager.InvokeEvent("TurnLeft");
				}
				break;
		}
	}

	private void AddEventDebug(string eventName) {
		EventManager.AddListener(eventName, () => { Debug.Log(eventName); });
	}
}
