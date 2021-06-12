using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public Vector3 direction;   // Direction of the projectile
    public float speed = 1f;    // Speed of the projectile
    
    float destroyTime = 10f;    // Time after which the object will be destroyed 
    
    Vector3 velocity;

    float scale = 1f;           // Current scale of the projectile
    float scaleDiminish = 0.5f; // Point after which increases in scale have diminishing returns 

    float damage = 1f; 
    
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
                DamagePlayer(collider);
                break;

            case("Enemy"):      // collision with enemy
                DestroySelf();
                break;

            case("Environment"):// collision with environment
                DestroySelf();
                break;
        }

        // Collision with another projectile
        if(collider.gameObject.name == "EnemyProjectile")
            CombineProjectile(collider);

    }

    void CombineProjectile(Collider collider){
        EnemyProjectile other = collider.gameObject.GetComponent<EnemyProjectile>();
        
        // If scale match - Combine based on index
        if (other.scale == scale && collider.transform.GetSiblingIndex() > transform.GetSiblingIndex())
            CombineProjectileAndDestroyOther(collider, other);    // Destroy based on hierarchy
        
        else if (other.scale == scale && collider.transform.GetSiblingIndex() < transform.GetSiblingIndex())
            DestroySelf();
        
        else if (other.scale < scale)
            CombineProjectileAndDestroyOther(collider, other);    // Destroy the smaller object
        
        else
            DestroySelf();
    }

    void CombineProjectileAndDestroyOther(Collider collider, EnemyProjectile other){

        // Combine velocity
        rb.velocity = new Vector3(0f, 0f, 0f);          // Zero out
        speed = (speed + other.speed) * 0.5f;           // Combined speed
        direction = Vector3.Normalize((direction+other.direction)*0.5f);   // Combined vector direction
        rb.AddRelativeForce(direction * speed);         // Set new force with combined direction & speed

        // Add damage
        damage += other.damage;

        // Add scale
        if (scale>scaleDiminish) scale += other.scale*.15f;
        else scale+= other.scale;

        other.DestroySelf();
        SetNewSize();
    }


    void DamagePlayer(Collider collider){
        PlayerStats PlayerStats = collider.gameObject.GetComponent<PlayerStats>();
        PlayerStats.hp -= 1;
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
