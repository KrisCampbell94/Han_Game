using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEncounterScript : MonoBehaviour
{
    public bool isPlayerClose = false;
    public bool isEnemyClose = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            isPlayerClose = true;
        }
        if(collider.gameObject.tag == "Enemy" || collider.gameObject.tag == "EnemyWeapon")
        {
            isEnemyClose = true;
        }

    }
    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            isPlayerClose = false;
        }
        if (collider.gameObject.tag == "Enemy" || collider.gameObject.tag == "EnemyWeapon")
        {
            isEnemyClose = false;
        }
    }
}
