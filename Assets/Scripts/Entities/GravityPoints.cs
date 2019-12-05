using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityPoints : MonoBehaviour
{
	private EventManager eventManager;

    public GameObject gravityFlame;

    public static readonly int SMALL_USE = 2;
    public static readonly int LARGE_USE = 4;

	public float maxGravityPoints = 4;
    public float replenishInterval = 0.5f; // Increase points ever x seconds 
    public float replenishAmount = 0.5f; // Increase x points every interval

    public float gravityPoints { get; private set; }

    private float previousPoints;

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
        FlamePosition();
        FlameSizeUpdate();
        
        if (replenishTimer <= 0 && gravityPoints < maxGravityPoints) {
            gravityPoints += replenishAmount;
            replenishTimer = replenishInterval;
            
        }
        
        replenishTimer -= Time.fixedDeltaTime;
        previousPoints = gravityPoints;
    }
    void FlamePosition()
    {
        eventManager.AddListener("Input_TurnLeft", () => {
            gravityFlame.transform.localPosition = new Vector2(1, 2.49f);
        });
        eventManager.AddListener("Input_TurnRight", () => {
            gravityFlame.transform.localPosition = new Vector2(-1, 2.49f);
        });
    }
    void FlameSizeUpdate()
    {
        Vector3 newScale = new Vector3(0, 0),
            currentScale = gravityFlame.transform.localScale;
        SpriteRenderer flameRender = gravityFlame.GetComponent<SpriteRenderer>();

        ParticleSystem.MainModule particle = gravityFlame.transform.Find("Particle System").GetComponent<ParticleSystem>().main;
        ParticleSystem.EmissionModule particleEmission = gravityFlame.transform.Find("Particle System").GetComponent<ParticleSystem>().emission;

        float alpha = 1.0f;
        if(gravityPoints == maxGravityPoints)
        {
            newScale = new Vector3(0.5f, 1);
        }
        else if(gravityPoints < maxGravityPoints && gravityPoints >= (maxGravityPoints * 0.9f))
        {
            newScale = new Vector3(0.45f, 0.9f);
            alpha = 0.9f;
            
        }
        else if (gravityPoints < (maxGravityPoints * 0.9f) && gravityPoints >= (maxGravityPoints * 0.8f))
        {
            newScale = new Vector3(0.4f, 0.8f);
            alpha = 0.8f;
        }
        else if (gravityPoints < (maxGravityPoints * 0.8f) && gravityPoints >= (maxGravityPoints * 0.7f))
        {
            newScale = new Vector3(0.35f, 0.7f);
            alpha = 0.7f;
        }
        else if (gravityPoints < (maxGravityPoints * 0.7f) && gravityPoints >= (maxGravityPoints * 0.6f))
        {
            newScale = new Vector3(0.3f, 0.6f);
            alpha = 0.6f;
        }
        else if (gravityPoints < (maxGravityPoints * 0.6f) && gravityPoints >= (maxGravityPoints * 0.5f))
        {
            newScale = new Vector3(0.25f, 0.5f);
            alpha = 0.5f;
        }
        else if (gravityPoints < (maxGravityPoints * 0.5f) && gravityPoints >= (maxGravityPoints * 0.4f))
        {
            newScale = new Vector3(0.2f, 0.4f);
            alpha = 0.4f;
        }
        else if (gravityPoints < (maxGravityPoints * 0.4f) && gravityPoints >= (maxGravityPoints * 0.3f))
        {
            newScale = new Vector3(0.15f, 0.3f);
            alpha = 0.3f;
        }
        else if (gravityPoints < (maxGravityPoints * 0.3f) && gravityPoints >= (maxGravityPoints * 0.2f))
        {
            newScale = new Vector3(0.1f, 0.2f);
            alpha = 0.2f;
        }
        else if (gravityPoints < (maxGravityPoints * 0.2f) && gravityPoints >= (maxGravityPoints * 0.1f))
        {
            newScale = new Vector3(0.05f, 0.1f);
            alpha = 0.1f;
        }
        else if (gravityPoints < (maxGravityPoints * 0.1f))
        {
            newScale = new Vector3(0, 0);
            alpha = 0.0f;
        }
        // Particle System Edits
        particle.startSize = (0.55f * alpha);
        particleEmission.rateOverTime = new ParticleSystem.MinMaxCurve(20 * alpha);

        // Debug.Log(gravityPoints);
        gravityFlame.transform.localScale = Vector3.Lerp(currentScale, newScale,1 * Time.deltaTime);
        flameRender.color = new Color(flameRender.color.r, flameRender.color.g, flameRender.color.b, alpha);
        
    }
}
