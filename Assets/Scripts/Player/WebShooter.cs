using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebShooter : MonoBehaviour
{
    public GameObject webProjectilePrefab;
    private int remainingBursts = 6;
    public int websPerBurst = 8;
    private int remainingWebs = 0;
    public float burstSpeed = 0.1f;
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
        if ( Input.GetMouseButtonDown(0) && remainingBursts > 0) 
        { 
            remainingWebs = websPerBurst;
        }

        if ( remainingWebs > 0  && timeSinceLastWeb > burstSpeed)
        {
            ShootWeb();
            remainingWebs -= 1;
            timeSinceLastWeb = 0f;
        }
        timeSinceLastWeb += Time.deltaTime;
    }

    private void ShootWeb()
    {
        Debug.DrawRay(transform.position, transform.right, Color.blue, 1f);

        Vector3 direction = transform.right;
        direction.x += Random.Range(-sprayRandomness, sprayRandomness);
        direction.z += Random.Range(-sprayRandomness, sprayRandomness);
        

        // Create the new projectile
        Vector3 spawnPos = transform.position + direction;
        GameObject projectile = Instantiate(webProjectilePrefab, spawnPos, transform.rotation);
        projectile.GetComponent<Rigidbody>().AddForce(direction * projectileSpeed); // Set force in dir
    }
}
