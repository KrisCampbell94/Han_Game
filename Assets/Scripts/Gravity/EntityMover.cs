using UnityEngine;
using static Gravity;

public class EntityMover : MonoBehaviour
{
    public enum Direction { Left, Right }
    public enum Movement { Stopped, Walking, Running }

    public float movementSpeed = 6.0f;
    public float runningMultiplier = 2;

    private EventManager eventManager;
    private Rigidbody2D rBody2D;

    // Current movement direction relative to self
    private Direction direction;

    // Current movement state
    private Movement movement;

    // Movement
    private Vector2 velocity;

    // Current grounded state
    private bool grounded;

    // Rotation Lerping
    public float timeToRotate = 0.5f; // Seconds
    private float startTime;
    private Vector3 startAngle;
    private float destAngleZ;

    // Start is called before the first frame update
    void Start()
    {
        eventManager = GetComponent<EventManager>();
        rBody2D = GetComponent<Rigidbody2D>();

        // Initial direction and movement
        SetDirection(Direction.Right);
        SetMovement(Movement.Stopped);

        // Add listeners to react to input
        eventManager.AddListener("Input_TurnLeft", () => SetDirection(Direction.Left));
        eventManager.AddListener("Input_TurnRight", () => SetDirection(Direction.Right));
        eventManager.AddListener("Input_Stopping", () => SetMovement(Movement.Stopped));
        eventManager.AddListener("Input_Walking", () => SetMovement(Movement.Walking));
        eventManager.AddListener("Input_Running", () => SetMovement(Movement.Running));

        // Add listeners to react to gravity change
        eventManager.AddListener("Gravity_North", () => RotateSelfToGravity(GravityDirection.North));
        eventManager.AddListener("Gravity_East", () => RotateSelfToGravity(GravityDirection.East));
        eventManager.AddListener("Gravity_South", () => RotateSelfToGravity(GravityDirection.South));
        eventManager.AddListener("Gravity_West", () => RotateSelfToGravity(GravityDirection.West));
    }

    private void FixedUpdate()
    {
        // If not orientated right
        if (transform.eulerAngles.z != destAngleZ)
        {
            UpdateOrientation();
        }
        else
        {
            UpdateMovement();
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

        // If angle is -90, set it to 270 instead
        if (newAngle.z == -90 && rotateDelta >= 1)
        {
            newAngle.z = 270;
        }

        // Set transform to new angle
        transform.eulerAngles = newAngle;
    }

    private void UpdateMovement()
    {
        // Update velocity
        rBody2D.velocity = new Vector2(movementSpeed * movementMultiplier * directionMultiplier, rBody2D.velocity.y);
    }

    // Events

    private void SetDirection(Direction newDirection)
    {
        direction = newDirection;

        float directionMultiplier = 0;

        // Set velocity direction
        switch (direction)
        {
            case Direction.Left:
                directionMultiplier = -1;
                break;
            case Direction.Right:
                directionMultiplier = 1;
                break;
        }
    }

    private void SetMovement(Movement newMovement)
    {
        movement = newMovement;

        float movementMultiplier = 0;

        // Set velocity multiplier
        switch (movement)
        {
            case Movement.Stopped:
                movementMultiplier = 0;
                break;
            case Movement.Walking:
                movementMultiplier = 1;
                break;
            case Movement.Running:
                movementMultiplier = runningMultiplier;
                break;
        }
    }

    private void RotateSelfToGravity(GravityDirection gravityDirection)
    {
        // Store current time and angle
        startTime = Time.time;
        startAngle = transform.eulerAngles;

        // Get new angle based on direction of gravity
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
                break;
            case GravityDirection.West:
                destAngleZ = -90;
                break;
        }
    }
}
