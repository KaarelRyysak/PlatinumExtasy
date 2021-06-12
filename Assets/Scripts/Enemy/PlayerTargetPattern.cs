using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargetPattern : MonoBehaviour, IProjectilePattern
{
    Transform playerTransform;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public Vector3[] GetProjectiles()
    {
        if (playerTransform != null)
        {
            Vector3 directionToPlayer = new Vector3();
            directionToPlayer.x = (playerTransform.position.x - transform.position.x);
            directionToPlayer.z = (playerTransform.position.z - transform.position.z);
            directionToPlayer = directionToPlayer.normalized;
            return new Vector3[] { directionToPlayer};
        }
        // If no player object
        return new Vector3[] {new Vector3(1, 0, 0) };
    }
}
