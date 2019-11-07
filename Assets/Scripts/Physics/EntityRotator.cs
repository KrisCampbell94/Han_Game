using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Gravity;

public class EntityRotator : MonoBehaviour
{
    public bool orienting { get; private set; } // Enabled when rotating, disabled when done

    private EventManager eventManager;
    
    // Rotation Lerping
    public float timeToRotate = 0.5f; // Seconds
    private float startTime; // Time when started rotating
    private Vector3 startAngle; // Angle when started rotating
    private float destAngleZ; // Angle to rotate to

    // Start is called before the first frame update
    void Start()
    {
        eventManager = GetComponent<EventManager>();

        // Add listeners to react to gravity change
        eventManager.AddListener("Gravity_North", () => RotateSelfToGravity(GravityDirection.North));
        eventManager.AddListener("Gravity_East", () => RotateSelfToGravity(GravityDirection.East));
        eventManager.AddListener("Gravity_South", () => RotateSelfToGravity(GravityDirection.South));
        eventManager.AddListener("Gravity_West", () => RotateSelfToGravity(GravityDirection.West));
    }
    
    private void FixedUpdate()
    {
        if (orienting)
        {
            UpdateOrientation();
        }
    }

    // Updates

    // TODO: Take current orientation into account?
    private void UpdateOrientation()
    {
        // Get new angle for this update
        Vector3 newAngle = startAngle;
        float rotateDelta = (Time.time - startTime) / timeToRotate;
        newAngle.z = Mathf.Lerp(startAngle.z, destAngleZ, rotateDelta);

        // Set transform to new angle
        transform.eulerAngles = newAngle;

        // If angle has reached 360, reset the dest to 0
        if (newAngle.z == 360)
        {
            destAngleZ = 0;
        }
        else if (newAngle.z == -90) // If angle reached -90, reset dest to 270
        {
            destAngleZ = 270;
        }

        // If reached destination rotation
        if (transform.eulerAngles.z == destAngleZ)
        {
            // Disable orienting state
            orienting = false;
            eventManager.InvokeEvent("Rotator_Orienting_" + orienting);
        }
    }

    // Events

    private void RotateSelfToGravity(GravityDirection gravityDirection)
    {
        // Store current time and angle
        startTime = Time.time;
        startAngle = transform.eulerAngles;

        // Depending on direction, set
        // New entity angle
        // Whether movement is on x or y
        // Whether movement direction is flipped or not
        // Whether jump direction is flipped or not
        switch (gravityDirection)
        {
            case GravityDirection.North:
                destAngleZ = 180;
                break;
            case GravityDirection.East:
                destAngleZ = 90;
                break;
            case GravityDirection.South:
            default:
                destAngleZ = 0;
                // Smart rotate
                if (startAngle.z == 270)
                {
                    destAngleZ = 360;
                }
                break;
            case GravityDirection.West:
                destAngleZ = 270;
                // Smart rotate
                if (startAngle.z == 0)
                {
                    destAngleZ = -90;
                }
                break;
        }
        
        // Enable orienting state
        orienting = true;
        eventManager.InvokeEvent("Rotator_Orienting_" + orienting);
    }
}
