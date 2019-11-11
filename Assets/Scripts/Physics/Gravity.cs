using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    public enum GravityDirection { North, East, South, West }

    public float gravityMagnitude = 9.8f;
    public bool autoFixRigidbody = true;

    private EventManager eventManager;
    private Rigidbody2D rBody2D;

    // Current gravity direction and vector
    public GravityDirection gravityDirection { get; private set; }
    public Vector2 gravityVector { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        eventManager = GetComponent<EventManager>();
        rBody2D = GetComponent<Rigidbody2D>();

        if (rBody2D.gravityScale != 0)
        {
            Debug.LogError("Rigidbody2D Gravity Scale should be 0 to use custom Gravity.");
        }

        // Set initial gravity
        SetGravityDirection(GravityDirection.South);

        // Change gravity direction based on input relative to world
        eventManager.AddListener("Input_Gravity_North", () => SetGravityDirection(GravityDirection.North));
        eventManager.AddListener("Input_Gravity_East", () => SetGravityDirection(GravityDirection.East));
        eventManager.AddListener("Input_Gravity_South", () => SetGravityDirection(GravityDirection.South));
        eventManager.AddListener("Input_Gravity_West", () => SetGravityDirection(GravityDirection.West));

        // Change gravity direction based on input relative to self
        eventManager.AddListener("Input_Gravity_Flip", FlipGravity);
        eventManager.AddListener("Input_Gravity_RotateLeft", RotateGravityLeft);
        eventManager.AddListener("Input_Gravity_RotateRight", RotateGravityRight);
    }

    private void FixedUpdate()
    {
        // Apply gravity in selected direction
        rBody2D.AddForce(gravityVector * gravityMagnitude);
    }

    // Flip gravity to opposite direction
    public void FlipGravity()
    {
        Debug.Log("start " + gravityDirection);

        switch (gravityDirection)
        {
            case GravityDirection.North:
                SetGravityDirection(GravityDirection.South);
                break;
            case GravityDirection.East:
                SetGravityDirection(GravityDirection.West);
                break;
            case GravityDirection.South:
                SetGravityDirection(GravityDirection.North);
                break;
            case GravityDirection.West:
                SetGravityDirection(GravityDirection.East);
                break;
        }
    }
    
    // Rotate gravity right relative to now
    public void RotateGravityLeft()
    {
        switch (gravityDirection)
        {
            case GravityDirection.North:
                SetGravityDirection(GravityDirection.East);
                break;
            case GravityDirection.East:
                SetGravityDirection(GravityDirection.South);
                break;
            case GravityDirection.South:
                SetGravityDirection(GravityDirection.West);
                break;
            case GravityDirection.West:
                SetGravityDirection(GravityDirection.North);
                break;
        }
    }

    // Rotate gravity left relative to now
    public void RotateGravityRight()
    {
        switch (gravityDirection)
        {
            case GravityDirection.North:
                SetGravityDirection(GravityDirection.West);
                break;
            case GravityDirection.East:
                SetGravityDirection(GravityDirection.North);
                break;
            case GravityDirection.South:
                SetGravityDirection(GravityDirection.East);
                break;
            case GravityDirection.West:
                SetGravityDirection(GravityDirection.South);
                break;
        }
    }

    // Set gravity relative to world
    public void SetGravityDirection(GravityDirection newGravityDirection)
    {
        // If already set to this direction, return
        if (gravityDirection == newGravityDirection)
        {
            return;
        }

        // Save new direction
        gravityDirection = newGravityDirection;

        // Set gravityDirectionVector based on gravityDirection
        switch (gravityDirection)
        {
            case GravityDirection.North:
                gravityVector = Vector2.up;
                break;
            case GravityDirection.East:
                gravityVector = Vector2.right;
                break;
            case GravityDirection.South:
                gravityVector = Vector2.down;
                break;
            case GravityDirection.West:
                gravityVector = Vector2.left;
                break;
        }

        // Clear existing velocity
        rBody2D.velocity = Vector3.zero;

        // Invoke event to indicate that gravity has changed
        eventManager.InvokeEvent("Gravity_" + gravityDirection);
    }
}
