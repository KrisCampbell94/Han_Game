﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
	public enum GravityDirection { North, East, South, West }

	public float gravityMagnitude = 9.8f;

	private EventManager eventManager;
	private Rigidbody2D rBody2D;

	// Current gravity vector
	private Vector2 gravityVector;

	// Start is called before the first frame update
	void Start() {
		eventManager = GetComponent<EventManager>();
		rBody2D = GetComponent<Rigidbody2D>();

		eventManager.AddListener("Input_Gravity_North", () => SetGravityDirection(GravityDirection.North));
		eventManager.AddListener("Input_Gravity_East", () => SetGravityDirection(GravityDirection.East));
		eventManager.AddListener("Input_Gravity_South", () => SetGravityDirection(GravityDirection.South));
		eventManager.AddListener("Input_Gravity_West", () => SetGravityDirection(GravityDirection.West));
	}

	private void FixedUpdate() {
		// Apply gravity in selected direction
		rBody2D.AddForce(gravityVector * gravityMagnitude);
	}

	public void SetGravityDirection(GravityDirection newGravityDirection) {
		// Set gravityDirectionVector based on gravityDirection
		switch (newGravityDirection) {
			case GravityDirection.North:
				gravityVector = Vector2.up;
				break;
			case GravityDirection.East:
				gravityVector = Vector2.right;
				break;
			case GravityDirection.South:
			default:
				gravityVector = Vector2.down;
				break;
			case GravityDirection.West:
				gravityVector = Vector2.left;
				break;
		}

		eventManager.InvokeEvent("Gravity_" + newGravityDirection);
	}
}
