using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public float health = 4f;
    private float lastHealth = 4f;
    public float scoreAwardedOnDeath = 400f;


    public AudioManager.AudioSound onHitSound;
    public AudioManager.AudioSound onDestroySound;

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0f)
        {
            GameManager gameManager = FindObjectOfType<GameManager>();
            gameManager.stats.score += scoreAwardedOnDeath;
            WebShooter webShooter = FindObjectOfType<WebShooter>();
            webShooter.AddMoreWebBursts(1);
            StartCoroutine(ScaleDownAndDestroySelf(0.15f));
        }
    }


    void Start(){
        lastHealth = health;
    }

    void Update(){

        if (lastHealth!=health){
            lastHealth = health;
            if (health == 0) FindObjectOfType<AudioManager>().PlaySound(onDestroySound);
            else FindObjectOfType<AudioManager>().PlaySound(onHitSound);
        }
    }

    IEnumerator ScaleDownAndDestroySelf(float time)
    {
        Vector3 originalScale = this.transform.localScale;
        Vector3 destinationScale = new Vector3(0f, 0f, 0f);
        
        float currentTime = 0.0f;
        
        while (currentTime <= time)
        {
        this.transform.localScale = Vector3.Lerp(originalScale, destinationScale, currentTime / time);
        currentTime += Time.deltaTime;
        yield return null;
        } ;
        
        Destroy(gameObject);
    }
}
