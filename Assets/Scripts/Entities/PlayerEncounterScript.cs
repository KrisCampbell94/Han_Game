﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEncounterScript : MonoBehaviour
{
    public bool isPlayerClose = false;
    public bool isEnemyClose = false;
    public bool closeToWall = false;

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
        if(collider.gameObject.layer == 8)
        {
            closeToWall = true;
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
        if (collider.gameObject.layer == 8)
        {
            closeToWall = false;
        }
    }
}
