using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowMovement : MonoBehaviour
{
    // Transform to follow
    Transform targetTransform;

    Rigidbody rb;
    float speed = 650f;

    void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        
        if (!playerObj) Debug.LogWarning("Warning: No player in scene.");
        else targetTransform = playerObj.transform;
    }

    void FixedUpdate()
    {
        if (targetTransform){
            if (Vector3.Distance(targetTransform.position, transform.position) < 2.0f) //Stop if I get within melee range
            {
                return;
            }
            Vector3 directionToTarget = Vector3.zero;
            directionToTarget.x = (targetTransform.position.x - transform.position.x);
            directionToTarget.z = (targetTransform.position.z - transform.position.z);
            directionToTarget = directionToTarget.normalized;
            //Debug.DrawRay(transform.position, directionToTarget * 3, Color.green);
            //transform.Translate(directionToTarget * speed * Time.deltaTime, Space.World);

            rb.AddForce(directionToTarget * speed * Time.deltaTime);
        }
    }
}
