using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
	public enum ScoreType { Time }

	public ScoreType scoreType = ScoreType.Time;
	public int scorePerSecond = 1;
	public int maxScore = 60 * 10;

	public Text scoreText;
	public Text timeText;

	public int score { get; private set; }
	public float timeSinceStart { get; private set; }

	private bool trackTime;
	private float timeStart;

	// Start is called before the first frame update
	void Start() {
		StartTime();
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

		score = maxScore - Mathf.RoundToInt(timeSinceStart * scorePerSecond);
		if (score <= 0) {
			score = 0;
		}

		// TODO: Use events instead
		scoreText.text = score.ToString();
		timeText.text = Mathf.RoundToInt(timeSinceStart).ToString();
	}

	public void StopTime() {
		trackTime = false;
	}
}
