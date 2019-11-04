using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerScript : MonoBehaviour
{
	private GameObject getPlayer;
	
	public bool bounds = true;
	public Vector3 minCameraPos, maxCameraPos;
	
    // Start is called before the first frame update
    void Start()
    {
        getPlayer = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	void FixedUpdate(){
		// Update the position of the camera relative to the player
		transform.position = new Vector3(getPlayer.transform.position.x, getPlayer.transform.position.y, transform.position.z);
		
		// Boundaries setup
		if(bounds){
			transform.position = new Vector3(
				Mathf.Clamp(transform.position.x, minCameraPos.x, maxCameraPos.x),
				Mathf.Clamp(transform.position.y, minCameraPos.y, maxCameraPos.y),
				Mathf.Clamp(transform.position.z, minCameraPos.z, maxCameraPos.z)
			);
		}
	}
}
