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
            Destroy(this.gameObject);
        }
    }


    void Start(){
        lastHealth = health;
    }

    void Update(){

        if (lastHealth!=health){
            lastHealth = health;
            if (health == 0) FindObjectOfType<AudioManager>().PlaySound(onHitSound);
            else FindObjectOfType<AudioManager>().PlaySound(onDestroySound);
        }
    }
}
