using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
	// Ground check variables
	public Transform groundCheck;
	public LayerMask whatIsGround;
	public float groundRadius = 0.2f;

	public bool grounded { get; private set; }

	// Reference to other components
	private EventManager eventManager;

	// Start is called before the first frame update
	void Start() {
		eventManager = GetComponent<EventManager>();
	}

    // Update is called once per frame
    void Update() {
		// Check if grounded
		bool newGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);

		// If grounded changed, then triggere event
		if (grounded != newGrounded) {
			grounded = newGrounded;
			eventManager.InvokeEvent("Ground_" + grounded);
		}
	}
}
