using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileEmitter : MonoBehaviour
{
    [SerializeField] IProjectilePattern projectilePattern;
    [SerializeField] float shootFrequency = 1f;
    float shootTime = 0;

    void Start()
    {
        projectilePattern = GetComponent<IProjectilePattern>();
    }

    void Update()
    {
        shootTime += Time.deltaTime;
        if (shootTime >= shootFrequency)
        {
            Shoot(projectilePattern.GetProjectiles());
            shootTime -= shootFrequency;
        }
    }

    void Shoot(Vector3[] projectiles)
    {
        for (int i = 0; i < projectiles.Length; i++)
        {
            createProjectile(projectiles[i]);
        }
    }

    void createProjectile(Vector3 dir)
    {
        Debug.DrawRay(transform.position, dir * 4, Color.red, 1f);
    }
}
