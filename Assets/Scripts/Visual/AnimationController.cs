using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
	private EventManager eventManager;
	private Animator animator;
	private SpriteRenderer spriteRenderer;

	// Start is called before the first frame update
	void Start() {
		eventManager = GetComponent<EventManager>();
		animator = GetComponent<Animator>();
		spriteRenderer = GetComponent<SpriteRenderer>();

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

		eventManager.AddListener("Input_TurnLeft", () => { spriteRenderer.flipX = true; });
		eventManager.AddListener("Input_TurnRight", () => { spriteRenderer.flipX = false; });

		eventManager.AddListener("Ground_True", () => {
			SetAnimatorVariable("Grounded", true);
			SetAnimatorVariable("Gravity", false);
		});
		eventManager.AddListener("Ground_False", () => { SetAnimatorVariable("Grounded", false); });

		eventManager.AddListener("Mover_Jumping", () => { SetAnimatorVariable("Jumping", true); });

        eventManager.AddListener("Attacking_Range",() => { SetAnimatorVariable("Attacking", true); }); //~

        eventManager.AddListener("Attacking_Close", () =>{ SetAnimatorVariable("CloseAttacking",true); }); //~

        eventManager.AddListener("Rotator_Orienting_True", () => SetAnimatorVariable("Gravity", true));
		eventManager.AddListener("Rotator_Orienting_False", () => SetAnimatorVariable("Gravity", false));
	}

	private void SetAnimatorVariable(string name, bool value) {
		animator.SetBool(name, value);
	}

    private void AttackingFinish() //~
    {
        SetAnimatorVariable("Attacking", false);
    }
    private void CloseAttackingFinish() //~
    {
        SetAnimatorVariable("CloseAttacking", false);
        GetComponent<HitBox>().Enabled = false;
    }
}
