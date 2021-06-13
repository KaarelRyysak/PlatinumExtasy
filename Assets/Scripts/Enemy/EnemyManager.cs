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
    
    void Awake()
    {
        attachedJoints = new List<SpringJoint>();

        enemyStats = this.gameObject.GetComponent<EnemyStats>();
        rb = this.gameObject.GetComponent<Rigidbody>();
    }

    public void Update()
    {
        //If enough time has passed, check if all joints in the list are still attached
        if (Time.time > lastRefreshTime + refreshAttachedFrequency)
        {
            refreshAttachedJoints();
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
        
        //flash model white here
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
