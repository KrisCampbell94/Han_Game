using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityPoints : MonoBehaviour
{
	private EventManager eventManager;

    public static readonly int SMALL_USE = 1;
    public static readonly int LARGE_USE = 1;

	public int maxGravityPoints = 10;
    public int replenishInterval = 1; // Increase points ever x seconds 
    public int replenishAmount = 1; // Increase x points every interval

    public int gravityPoints { get; private set; }

    private float timeAtLastReplenish = 0;

    // Start is called before the first frame update
    void Start()
    {
		eventManager = GetComponent<EventManager>();
        eventManager.AddListener("GravityPoints_SmallUse", () => gravityPoints -= SMALL_USE);
        eventManager.AddListener("GravityPoints_LargeUse", () => gravityPoints -= LARGE_USE);

        gravityPoints = maxGravityPoints;
    }

    void FixedUpdate()
    {
        float timeSinceLastReplenish = Time.time - timeAtLastReplenish;
        if (timeSinceLastReplenish >= replenishInterval && gravityPoints <= maxGravityPoints) {
            gravityPoints += replenishAmount;
            timeAtLastReplenish = Time.time;
        }
    }
}
