using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebShooter : MonoBehaviour
{
    public GameObject webProjectilePrefab;
    private int remainingBursts = 6;
    private int maximumBursts = 6;
    public int shotsPerBurst = 8;
    private int remainingPellets = 0;
    public float burstSpeed = 0.1f;
    private float timeSinceLastWeb = 999f;
    public float sprayRandomness = 0.3f;
    public float projectileSpeed;
    private float remainingBlobs = 6f;

    public AudioManager.AudioSound onFireSound;
    private AudioManager AudioManager;

    // Start is called before the first frame update
    void Start()
    {
        // Link the emitter to the AudioManager
        AudioManager = FindObjectOfType<AudioManager>();
        if (!AudioManager) Debug.LogError("ERROR: AudioManager could not be found on this object! Please add one!",this);
    }

    // Update is called once per frame
    void Update()
    {
        if ( Input.GetMouseButtonDown(1) && remainingBursts > 0) 
        { 
            remainingPellets = shotsPerBurst;
            remainingBursts -= 1;
            //Add this to UI too

            // Play sound
            if (onFireSound.clip != null) AudioManager.PlaySound(onFireSound);
        }

        if ( remainingPellets > 0  && timeSinceLastWeb > burstSpeed)
        {
            ShootWeb();
            remainingPellets -= 1;
            timeSinceLastWeb = 0f;
        }
        timeSinceLastWeb += Time.deltaTime;
    }

    public void AddMoreWebBursts(int count)
    {
        if (remainingBursts + count > maximumBursts)
        {
            remainingBursts = maximumBursts;
        }
        else
        {
            remainingBursts += count;
        }

        //Refresh UI elements..?
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
