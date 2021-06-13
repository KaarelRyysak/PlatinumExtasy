using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShooter : MonoBehaviour
{
    public GameObject gunProjectilePrefab;
    public float shootSpeed = 0.2f;
    private float timeSinceLastWeb = 999f;
    public float sprayRandomness = 0.3f;
    public float projectileSpeed;
    private float remainingBlobs = 6f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if ( Input.GetMouseButton(0) && timeSinceLastWeb > shootSpeed)
        {
            ShootGun();
            timeSinceLastWeb = 0f;
        }
        timeSinceLastWeb += Time.deltaTime;
    }

    private void ShootGun()
    {
        Debug.DrawRay(transform.position, transform.right, Color.blue, 1f);

        Vector3 direction = transform.right;
        direction.x += Random.Range(-sprayRandomness, sprayRandomness);
        direction.z += Random.Range(-sprayRandomness, sprayRandomness);
        

        // Create the new projectile
        Vector3 spawnPos = transform.position + direction;
        GameObject projectile = Instantiate(gunProjectilePrefab, spawnPos, transform.rotation);
        direction = Vector3.Normalize(direction);
        projectile.GetComponent<Rigidbody>().AddForce(direction * projectileSpeed); // Set force in dir
        
        // Set projectile prefs
        GunProjectile GunProjectile = projectile.GetComponent<GunProjectile>();
        GunProjectile.direction = direction;
        GunProjectile.speed = projectileSpeed;
    }
}
