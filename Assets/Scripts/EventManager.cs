using System;
using System.Collections.Generic;
using UnityEngine;

// Based on
// https://stackoverflow.com/questions/42034245/unity-eventmanager-with-delegate-instead-of-unityevent

public class EventManager : MonoBehaviour {
	public bool debug = true;

	// Holds reference to all events
	private Dictionary<string, Action> events = new Dictionary<string, Action>();

	// Add listener to event
	public void AddListener(string eventName, Action listener) {
		// If event doesn't exist, create a new one
		if (!events.ContainsKey(eventName)) {
			events.Add(eventName, listener);
		} else { // If event does exist, add listener to event
			events[eventName] += listener;
		}
	}

	// Remove listener from event
	public void RemoveListener(string eventName, Action listener) {
		// If event exists, remove listener from it
		if (events.ContainsKey(eventName)) {
			events[eventName] -= listener;
		}
	}

	// Invoke all associated listeners
	public void InvokeEvent(string eventName) {
        if(eventName == "Output_Jump") //~
        {
            // Added due to multiple logs containing Output_Jump
            return;
        }
		if (debug) {
			Debug.Log("Invoked " + eventName);
		}

		// If event exists, invoke all listeners
		if (events.ContainsKey(eventName)) {
			events[eventName].Invoke();
		}
	}

	// Get all registered events
	public Dictionary<string, Action>.KeyCollection GetRegisteredEvents() {
		return events.Keys;
	}
}
