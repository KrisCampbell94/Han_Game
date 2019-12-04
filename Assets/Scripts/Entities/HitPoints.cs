using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HitPoints : MonoBehaviour
{
    public int hitPoints;
	public int maxHitPoints = 10;
    public int pointsOnKill = 20;
	private SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
		hitPoints = maxHitPoints;
    }

    // Update is called once per frame
    void Update()
    {
		
		// Hit Point Setup
		if(hitPoints < Mathf.RoundToInt(0.2f * maxHitPoints) && 
			hitPoints >= 0){
			sr.color = new Color(0.5f, 0, 0);
		}
		else if(hitPoints < Mathf.RoundToInt(0.4f * maxHitPoints) && 
			hitPoints >= Mathf.RoundToInt(0.2f * maxHitPoints)){
			sr.color = new Color(0.6f, 0.2f, 0.2f);
		}
		else if(hitPoints < Mathf.RoundToInt(0.6f * maxHitPoints) && 
		hitPoints >= Mathf.RoundToInt(0.4f * maxHitPoints)){
			sr.color = new Color(0.7f, 0.4f, 0.4f); 
		}
		else if(hitPoints < Mathf.RoundToInt(0.8f * maxHitPoints) && 
			hitPoints >= Mathf.RoundToInt(0.6f * maxHitPoints)){
			sr.color = new Color(0.8f, 0.6f, 0.6f);
		}
		else if(hitPoints < maxHitPoints && 
			hitPoints >= Mathf.RoundToInt(0.8f * maxHitPoints)){
			sr.color = new Color(0.9f, 0.8f, 0.8f);
		}
		else if(hitPoints == maxHitPoints){
			sr.color = new Color(1, 1, 1);
		} 
		
		if(hitPoints < 1){
			if (tag=="Player") {
                GameOverScript.SceneToLoad = SceneManager.GetActiveScene().name;

                SceneManager.LoadScene("Lose");
			} else
            {
                Debug.Log("ADDING SCORE");
                ScoreManager.sharedInstance.AddToScore(pointsOnKill);
                gameObject.SetActive(false);
			}
		}
    }
    public void AddHitPoints(int pointsToAdd)
    {
        if((hitPoints + pointsToAdd) > maxHitPoints)
        {
            hitPoints = maxHitPoints;
        }
        else
        {
            hitPoints += pointsToAdd;
        }
    }

    public void SubtractHitPoints(int pointsToSub)
    {
        if((hitPoints - pointsToSub) < 0)
        {
            hitPoints = 0;
        }
        else
        {
            hitPoints -= pointsToSub;
        }
    }
}
