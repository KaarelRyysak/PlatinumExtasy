using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunProjectile : MonoBehaviour
{
    public Vector3 direction;   // Direction of the projectile
    public float speed = 1f;    // Speed of the projectile
    
    float destroyTime = 10f;    // Time after which the object will be destroyed 
    
    private Vector3 velocity;
    private float scale = 1f;           // Current scale of the projectile
    private float scaleDiminish = 0.5f; // Point after which increases in scale have diminishing returns 
    private float damage = 1f; 
    private Rigidbody rb;

    public void Awake(){
        rb = GetComponent<Rigidbody>();
        velocity = rb.velocity; // Get initial velocity
    }

    public void Start () { 

        Destroy(gameObject, destroyTime);           // Destroy after seconds
        scale = gameObject.transform.localScale.x;   // Get projectile scale

    }


    // Collision detection
    void OnTriggerEnter(Collider collider)
    {
        switch(collider.gameObject.tag){
            case("Player"):     // collision with player
                DestroySelf();
                break;

            case("Enemy"):      // collision with enemy
                DamageEnemy(collider);
                break;

            case("Environment"):// collision with environment
                DestroySelf();
                break;
        }

        // Collision with another projectile
        if(collider.gameObject.name == "EnemyProjectile")
        {
            //ignore collision..?
        }

    }

    void DamageEnemy(Collider collider){
        EnemyManager enemyManager = collider.GetComponent<EnemyManager>();
        enemyManager.DealDamageToAllAttached(damage);
        DestroySelf();
    }

    void SetNewSize() {
        gameObject.transform.localScale = new Vector3(scale,scale,scale); // Add scale to this object
    }

    // Destroy self
    void DestroySelf(){
        Destroy(gameObject);
    }
}
