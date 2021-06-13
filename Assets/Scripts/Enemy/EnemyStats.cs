using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public float health = 4f;
    public float scoreAwardedOnDeath = 4f;

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
}
