using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerScript : MonoBehaviour
{
	private GameObject player;
	
	public bool bounds = true;
	public Vector3 minCameraPos, maxCameraPos;
	
    // Start is called before the first frame update
    void Start() {
        player = GameObject.FindWithTag("Player");
    }

	void FixedUpdate(){
		// Update the position of the camera relative to the player
		Vector3 newPosition = player.transform.position;
		newPosition.z = transform.position.z;
		transform.position = newPosition;

		// Update rotation of camera relative to player
		Vector3 newRotation = transform.eulerAngles;
		newRotation.z = player.transform.eulerAngles.z;
		transform.eulerAngles = newRotation;
		
		// Boundaries for camera
		if(bounds){
			transform.position = new Vector3(
				Mathf.Clamp(transform.position.x, minCameraPos.x, maxCameraPos.x),
				Mathf.Clamp(transform.position.y, minCameraPos.y, maxCameraPos.y),
				Mathf.Clamp(transform.position.z, minCameraPos.z, maxCameraPos.z)
			);
		}
	}
}
