using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{

	public Text scoreText;
	public enum ScoreType { Time }

	public ScoreType scoreType = ScoreType.Time;
	public int scorePerSecond = 1;
	public int maxScore = 60 * 10;

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
			scoreText.Text = timeSinceStart.ToString();
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
		
	}

	public void StopTime() {
		UpdateTime();
		trackTime = false;
	}
}
