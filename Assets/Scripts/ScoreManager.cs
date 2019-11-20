using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
	public int scorePerSecond = 1;
	public int maxScoreFromTime = 60 * 10;

	public Text scoreText;
	public Text timeText;

	public int score { get; private set; }
	public float timeSinceStart { get; private set; }

	private bool trackTime;
	private float timeStart;
    private float timeAtLastDecrement;

    public static ScoreManager sharedInstance;

    private void Awake()
    {
        if(sharedInstance == null)
        {
            sharedInstance = this;
            Debug.Log("Shared Instance");
        }
        else
        {
            Debug.LogError("Multiple Score Managers Found. Kill Kill Kill. Die Die Die.");
        }
    }

    // Start is called before the first frame update
    void Start() {
		StartTime();
        score = maxScoreFromTime;
	}

	// Update is called once per frame
	void Update() {
		if (trackTime) {
			UpdateTime();
        }
	}

	public void StartTime() {
		trackTime = true;
		timeStart = Time.time;
	}

	public void UpdateTime() {
		float timeNow = Time.time;
		timeSinceStart = timeNow - timeStart;

        float timeSinceLastDecrement = Time.time - timeAtLastDecrement;
        if (timeSinceLastDecrement >= 1)
        {
            score -= scorePerSecond;
            if (score <= 0)
            {
                score = 0;
            }
            timeAtLastDecrement = Time.time;
        }

        //score = maxScore - Mathf.RoundToInt(timeSinceStart * scorePerSecond);

		// TODO: Use events instead
		scoreText.text = score.ToString();
		timeText.text = Mathf.FloorToInt(timeSinceStart).ToString();
	}

	public void StopTime() {
		trackTime = false;
	}

    public void AddToScore(int scoreToAdd)
    {
        score += scoreToAdd;
    }
}
