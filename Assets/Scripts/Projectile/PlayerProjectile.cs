using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public Vector3 direction;   // Direction of the projectile
    public float speed = 1f;    // Speed of the projectile
    
    float destroyTime = 10f;    // Time after which the object will be destroyed 
    
    private Vector3 velocity;
    private float damage = 1f; 
    private Rigidbody rb;

    public void Awake(){
        rb = GetComponent<Rigidbody>();
        velocity = rb.velocity; // Get initial velocity
    }

    public void Start () { 

        Destroy(gameObject, destroyTime);           // Destroy after seconds
    }


    // Collision detection
    void OnTriggerEnter(Collider collider)
    {
        switch(collider.gameObject.tag){
            case("Player"):     // collision with player
                DestroySelf();
                break;

            case("Enemy"):      // collision with enemy
                collider.gameObject.GetComponent<EnemyManager>().DealDamageToAllAttached(damage);
                break;

            case("Environment"):// collision with environment
                DestroySelf();
                break;
        }

        // Collision with another projectile
        if(collider.gameObject.name == "EnemyProjectile")
        {
            //TODO: ignore collision (?)
        }

    }


    void DamagePlayer(Collider collider){
        GameManager GameManager = FindObjectOfType<GameManager>();
        GameManager.stats.hp -= (int)damage;
        DestroySelf();
    }

    // Destroy self
    void DestroySelf(){
        Destroy(gameObject);
    }
}
