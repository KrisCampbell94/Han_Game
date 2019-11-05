using UnityEngine;
using static EntityMover;
using static Gravity;

public class PlayerInput : MonoBehaviour
{
	private EventManager eventManager;

	private Direction direction;
	private Movement movement;

    // Start is called before the first frame update
    void Start() {
		eventManager = GetComponent<EventManager>();

		// Current input affecting movement
		// May differ fron actual movement in case of obstacles, being in the air, etc
		movement = Movement.Stopped;

		// Current input affecting direction
		// Likely matches actual direction, since turning around is not hindered by obstacles
		direction = Direction.Right;

		AddEventDebug("Input_Stopping");
		AddEventDebug("Input_Walking");
		AddEventDebug("Input_Running");

		AddEventDebug("Input_TurnLeft");
		AddEventDebug("Input_TurnRight");

		AddEventDebug("Mover_GravityEast");
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
					GetComponent<Gravity>().ChangeGravity(GravityDirection.East);
				}
				break;
			case Direction.Right:
				// If facing right and movement says left
				if (horizontalMove < 0) {
					// Change to left and call event
					direction = Direction.Left;
					eventManager.InvokeEvent("Input_TurnLeft");
					GetComponent<Gravity>().ChangeGravity(GravityDirection.West);
				}
				break;
		}
	}

	private void AddEventDebug(string eventName) {
		eventManager.AddListener(eventName, () => { Debug.Log(eventName); });
	}
}
