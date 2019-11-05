using System;
using System.Collections.Generic;
using UnityEngine;

// Based on
// https://stackoverflow.com/questions/42034245/unity-eventmanager-with-delegate-instead-of-unityevent
public class EventManager : ScriptableObject {
	// Holds reference to all events
	private Dictionary<string, Action> events;

	// Singleton
	private static EventManager eventManager;

	// Get static singleton instance
	public static EventManager Instance {
		get {
			// If does not exist, initialize
			if (!eventManager) {
				eventManager = new EventManager();
				eventManager.Init();
			}

			return eventManager;
		}
	}

	// Init events dictionary
	void Init() {
		if (events == null) {
			events = new Dictionary<string, Action>();
		}
	}

	// Add listener to event
	public static void AddListener(string eventName, Action listener) {
		// If event doesn't exist, create a new one
		if (!Instance.events.ContainsKey(eventName)) {
			Instance.events.Add(eventName, listener);
		} else { // If event does exist, add listener to event
			Instance.events[eventName] += listener;
		}
	}

	// Remove listener from event
	public static void RemoveListener(string eventName, Action listener) {
		// If there's no eventManager, there's no events
		if (eventManager == null) return;

		// If event exists, remove listener from it
		if (Instance.events.ContainsKey(eventName)) {
			Instance.events[eventName] -= listener;
		}
	}

	// Invoke all associated listeners
	public static void InvokeEvent(string eventName) {
		// If event exists, invoke all listeners
		if (Instance.events.ContainsKey(eventName)) {
			Instance.events[eventName].Invoke();
		}
	}

	// Get all registered events
	public static Dictionary<string, Action>.KeyCollection GetRegisteredEvents() {
		return Instance.events.Keys;
	}
}
