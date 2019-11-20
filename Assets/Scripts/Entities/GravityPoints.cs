using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityPoints : MonoBehaviour
{
	private EventManager eventManager;

    public static readonly int SMALL_USE = 1;
    public static readonly int LARGE_USE = 2;

	public int maxGravityPoints = 10;
    public int replenishInterval = 3; // Increase points ever x seconds 
    public int replenishAmount = 1; // Increase x points every interval

    public int gravityPoints { get; private set; }

    private float replenishTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        eventManager = GetComponent<EventManager>();
        eventManager.AddListener("GravityPoints_SmallUse", () => {
            gravityPoints -= SMALL_USE;
            replenishTimer = replenishInterval;
        });
        eventManager.AddListener("GravityPoints_LargeUse", () => {
            gravityPoints -= LARGE_USE;
            replenishTimer = replenishInterval;
        });

        gravityPoints = maxGravityPoints;
    }

    void FixedUpdate()
    {
        if (replenishTimer <= 0 && gravityPoints < maxGravityPoints) {
            gravityPoints += replenishAmount;
            replenishTimer = replenishInterval;
        }
        
        replenishTimer -= Time.fixedDeltaTime;
    }
}
