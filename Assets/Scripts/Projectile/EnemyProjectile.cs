using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    float destroyTime = 5f; 

    public void Start () { 

        Destroy(gameObject, destroyTime); // Destroy after seconds
    }


    // Collision detection
    void OnCollisionEnter(Collision collision)
    {
        // Projectile collision with player
        if (collision.gameObject.tag == "Player")
        {
            PlayerStats PlayerStats = collision.gameObject.GetComponent<PlayerStats>();
            PlayerStats.hp -= 1;

            Destroy(gameObject); // Destroy self
        }
    }
}
