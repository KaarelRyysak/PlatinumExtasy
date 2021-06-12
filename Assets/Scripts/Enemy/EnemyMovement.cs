using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    // Transform to follow
    Transform targetTransform;
    float speed = 2.5f;

    void Start()
    {
        targetTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void FixedUpdate()
    {
        if (Vector3.Distance(targetTransform.position, transform.position) < 2.0f) //Stop if I get within melee range
        {
            return;
        }
        Vector3 directionToTarget = Vector3.zero;
        directionToTarget.x = (targetTransform.position.x - transform.position.x);
        directionToTarget.z = (targetTransform.position.z - transform.position.z);
        directionToTarget = directionToTarget.normalized;
        //Debug.DrawRay(transform.position, directionToTarget * 3, Color.green);
        transform.Translate(directionToTarget * speed * Time.deltaTime, Space.World);
    }
}
