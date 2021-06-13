using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileEmitter : MonoBehaviour
{
    [SerializeField] IProjectilePattern projectilePattern;
    [SerializeField] float shootFrequency = 1f;
    float shootTime = 0;
    public GameObject projectilePrefab;
    public float projectileSpeed = 250f;

    public AudioManager.AudioSound onFireSound;
    private AudioManager AudioManager;

    void Start()
    {
        projectilePattern = GetComponent<IProjectilePattern>();

        // Link the emitter to the AudioManager
        AudioManager = FindObjectOfType<AudioManager>();
        if (!AudioManager) Debug.LogError("ERROR: AudioManager could not be found on this object! Please add one!",this);
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

        // Create the new projectile
        GameObject projectile = UnityEngine.Object.Instantiate(projectilePrefab);
        projectile.name = "EnemyProjectile";
        projectile.transform.position = transform.position + dir; // Set pos
        dir = Vector3.Normalize(dir);
        projectile.GetComponent<Rigidbody>().AddRelativeForce(dir * projectileSpeed); // Set force in dir
        
        // Set projectile prefs
        EnemyProjectile EnemyProjectile = projectile.GetComponent<EnemyProjectile>();
        EnemyProjectile.direction = dir;
        EnemyProjectile.speed = projectileSpeed;

        // Play sound
        if (onFireSound.clip != null) AudioManager.PlaySound(onFireSound);
    }
}
