using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
	private EventManager eventManager;
	private Animator animator;
	private EntityMover entityMover;

	// Start is called before the first frame update
	void Start() {
		eventManager = GetComponent<EventManager>();
		animator = GetComponent<Animator>();

		eventManager.AddListener("Input_Stopping", () => {
			SetAnimatorVariable("Walking", false);
			SetAnimatorVariable("Running", false);
		});

		eventManager.AddListener("Input_Walking", () => {
			SetAnimatorVariable("Walking", true);
			SetAnimatorVariable("Running", false);
		});

		eventManager.AddListener("Input_Running", () => {
			SetAnimatorVariable("Walking", false);
			SetAnimatorVariable("Running", true);
		});

		eventManager.AddListener("Ground_True", () => {
			SetAnimatorVariable("Grounded", true);
			SetAnimatorVariable("Gravity", false);
		});
		eventManager.AddListener("Ground_False", () => { SetAnimatorVariable("Grounded", false); });

		eventManager.AddListener("Mover_Jumping", () => { SetAnimatorVariable("Jumping", true); });

		eventManager.AddListener("Mover_Orienting_True", () => SetAnimatorVariable("Gravity", true));
		eventManager.AddListener("Mover_Orienting_False", () => SetAnimatorVariable("Gravity", false));
	}

	private void SetAnimatorVariable(string name, bool value) {
		animator.SetBool(name, value);
	}
}
