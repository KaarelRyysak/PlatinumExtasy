using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<SpringJoint> attachedJoints = null; //List of joints that have been attached to the gameobject, refreshed every 10 secconds
    //before you use this, call "refreshAttachedJoints();" pls
    private float lastRefreshTime = 0f;
    private float refreshAttachedFrequency = 10f;
    private EnemyStats enemyStats;
    private Rigidbody rb;

    private MeshRenderer meshRenderer = null;

    private Color objectColor;
    public Color fadeColor = Color.white;
    public float fadeDuration = 5f;
    public float fadeStartTime = 0f;
    
    void Awake()
    {
        attachedJoints = new List<SpringJoint>();

        enemyStats = this.gameObject.GetComponent<EnemyStats>();
        rb = this.gameObject.GetComponent<Rigidbody>();

        //Find meshrenderer to change material color
        meshRenderer = this.gameObject.GetComponent<MeshRenderer>();
        objectColor = meshRenderer.material.color;
        meshRenderer.material.color = fadeColor;
        fadeStartTime = Time.time;
    }

    public void Update()
    {
        //If enough time has passed, check if all joints in the list are still attached
        if (Time.time > lastRefreshTime + refreshAttachedFrequency)
        {
            refreshAttachedJoints();
        }
        

        //Flash white when necessary
        if (Time.time < fadeDuration + fadeStartTime)
        {
            float percent = (Time.time - fadeStartTime) / fadeDuration;
            gameObject.GetComponent<Renderer>().material.color = Color.Lerp(fadeColor, objectColor, percent);
        }
    }
    public void refreshAttachedJoints()
    {
        List<SpringJoint> newList = new List<SpringJoint>(attachedJoints);
        foreach (SpringJoint joint in attachedJoints)
        {
            if (joint == null) newList.Remove(joint);
        }
        attachedJoints = newList;
    }

    //Call this when player gets hit by projectile
    //Recursive function that finds all glued together objects and causes damage to those as well
    public void DealDamageToAllAttached(float damage)
    {
        TakeDamage(damage);
        List<GameObject> attachedEnemies = GetAllAttachedEnemies();
        foreach (GameObject enemyObj in attachedEnemies)
        {
            EnemyManager enemyManager = enemyObj.gameObject.GetComponent<EnemyManager>();
            enemyManager.TakeDamage(damage);
        }
    }

    //Gets called on every enemy that is taking damage
    public void TakeDamage(float damage)
    {
        enemyStats.TakeDamage(damage);
        FlashWhite();
    }

    public void FlashWhite()
    {
        fadeStartTime = Time.time;
        meshRenderer.material.color = fadeColor;
    }

    //Gets every enemy attached to this gameobject, use this one Cade :)
    public List<GameObject> GetAllAttachedEnemies()
    {
        List<Rigidbody> exploredBodies = new List<Rigidbody>();
        List<GameObject> foundEnemies = new List<GameObject>();
        foundEnemies = GetAllAttachedEnemies(exploredBodies, foundEnemies);
        foundEnemies.Remove(this.gameObject);
        return foundEnemies;
    }

    //recursively get all attached enemies
    public List<GameObject> GetAllAttachedEnemies(List<Rigidbody> exploredBodies, List<GameObject> foundEnemies)
    {
        if (exploredBodies.Contains(rb)) return foundEnemies; //If we've been here before, leave

        //If we haven't been here before, let's add this to the attached enemies
        foundEnemies.Add(this.gameObject);
        exploredBodies.Add(this.rb);
        
        refreshAttachedJoints();
        foreach (SpringJoint joint in attachedJoints)
        {
            if (joint.gameObject.tag == "Enemy")
            {
                EnemyManager enemyManager = joint.gameObject.GetComponent<EnemyManager>();
                foundEnemies = enemyManager.GetAllAttachedEnemies(exploredBodies, foundEnemies);
            }
            if (joint.gameObject.tag == "Web")
            {
                WebProjectile enemyManager = joint.gameObject.GetComponent<WebProjectile>();
                foundEnemies = enemyManager.GetAllAttachedEnemies(exploredBodies, foundEnemies);
            }
        }

        return foundEnemies;
    }
}
