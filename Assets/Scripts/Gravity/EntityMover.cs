using UnityEngine;
using static Gravity;

public class EntityMover : MonoBehaviour
{
	public enum Direction { Left, Right }
	public enum Movement { Stopped, Walking, Running }

	private EventManager eventManager;

	// Current movement direction relative to self
	private Direction direction;

	// Current movement state
	private Movement movement;

	// Current grounded state
	private bool grounded;

	// Start is called before the first frame update
	void Start() {
		eventManager = GetComponent<EventManager>();

		direction = Direction.Right;
		movement = Movement.Stopped;

		eventManager.AddListener("Gravity_North", () => RotateSelfToGravity(GravityDirection.North));
		eventManager.AddListener("Gravity_East", () => RotateSelfToGravity(GravityDirection.East));
		eventManager.AddListener("Gravity_South", () => RotateSelfToGravity(GravityDirection.South));
		eventManager.AddListener("Gravity_West", () => RotateSelfToGravity(GravityDirection.West));
	}

    // Update is called once per frame
    void Update() {
        
    }

	private void FixedUpdate() {

	}

	private void RotateSelfToGravity(GravityDirection gravityDirection) {
		Debug.Log(gravityDirection);

		// Lerp

		// Also make camera follow
	}
}
