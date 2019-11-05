using UnityEngine;

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
    }

    // Update is called once per frame
    void Update() {
        
    }

	private void FixedUpdate() {

	}
}
