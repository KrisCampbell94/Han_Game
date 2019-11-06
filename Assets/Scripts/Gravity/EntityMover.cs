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

	// Rotation Lerping
	public float timeToRotate = 0.5f; // Seconds
	private float startTime;
	private Vector3 startAngle;
	private float destAngleZ;

	// Start is called before the first frame update
	void Start() {
		eventManager = GetComponent<EventManager>();

        // Initial direction and movement
		direction = Direction.Right;
		movement = Movement.Stopped;

        // Add listeners to react to gravity change
		eventManager.AddListener("Gravity_North", () => RotateSelfToGravity(GravityDirection.North));
		eventManager.AddListener("Gravity_East", () => RotateSelfToGravity(GravityDirection.East));
		eventManager.AddListener("Gravity_South", () => RotateSelfToGravity(GravityDirection.South));
		eventManager.AddListener("Gravity_West", () => RotateSelfToGravity(GravityDirection.West));
	}
    
	private void FixedUpdate() {
		// If not orientated right
		if (transform.eulerAngles.z != destAngleZ) {
            // Get new angle for this update
			Vector3 newAngle = startAngle;
            float rotateDelta = (Time.time - startTime) / timeToRotate;
            newAngle.z = Mathf.Lerp(startAngle.z, destAngleZ, rotateDelta);

            // If angle is -90, set it to 270 instead
            if (newAngle.z == -90 && rotateDelta >= 1) {
				newAngle.z = 270;
			}

            // Set transform to new angle
			transform.eulerAngles = newAngle;
		}
	}

	private void RotateSelfToGravity(GravityDirection gravityDirection) {
		// Store current time and angle
		startTime = Time.time;
		startAngle = transform.eulerAngles;

        // Get new angle based on direction of gravity
		switch (gravityDirection) {
			case GravityDirection.North:
				destAngleZ = 180;
				break;
			case GravityDirection.East:
				destAngleZ = 90;
				break;
			case GravityDirection.South:
			default:
				destAngleZ = 0;
				break;
			case GravityDirection.West:
				destAngleZ = -90;
				break;
		}
	}
}
