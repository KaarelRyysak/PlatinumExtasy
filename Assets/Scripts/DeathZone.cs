using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{


    // Collision detection
    void OnTriggerEnter(Collider collider)
    {
        switch(collider.gameObject.tag){

            case("Enemy"):      // collision with enemy
                Debug.Log("ENEMY IN DEATH ZONE");
                Destroy(collider.transform.gameObject);
                break;
        }
    }
}
