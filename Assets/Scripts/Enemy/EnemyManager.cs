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

    //Recursive function that finds all glued together objects and causes damage to those as well
    public void DealDamageToAllAttached(float damage)
    {
        List<Rigidbody> exploredBodies = new List<Rigidbody>();
        DealDamageToAllAttached(exploredBodies, damage);
    }

    public void DealDamageToAllAttached(List<Rigidbody> exploredBodies, float damage)
    {
        
        if (exploredBodies.Contains(rb)) return; //If we've been here before, leave

        //If we haven't been here before, take damage
        TakeDamage(damage);
        exploredBodies.Add(rb);

        //Let's check if there are any other connected webs/enemies that need to take damage
        foreach (SpringJoint joint in attachedJoints)
        {
            WebProjectile webProjectile = joint.GetComponent<WebProjectile>();
            if (webProjectile != null)
            {
                webProjectile.DealDamageToAllAttached(exploredBodies, damage);
                break;
            }

            EnemyManager enemyManager = joint.GetComponent<EnemyManager>();
            if (enemyManager != null)
            {
                enemyManager.DealDamageToAllAttached(exploredBodies, damage);
            }
        }
    }

    public void TakeDamage(float damage)
    {
        enemyStats.TakeDamage(damage);
        
        //flash model white here
    }
}
