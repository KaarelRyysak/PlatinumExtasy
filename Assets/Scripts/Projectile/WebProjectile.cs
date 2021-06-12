using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebProjectile : MonoBehaviour
{
    public List<SpringJoint> springJoints = null; //All currently connected joints (+1?)
    public float lifetime = 20f;
    private float spawntime = 0f;

    private Rigidbody rb;

    void Awake()
    {
        springJoints = new List<SpringJoint>();

        spawntime = Time.time;

        rb = this.gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //If lifetime is over, destroy self
        if ( Time.time > spawntime + lifetime )
        {
            Destroy(this.gameObject);
        }
    }


    void OnCollisionEnter(Collision collision)
    {
        if ( collision.rigidbody != null  && collision.gameObject.tag != "Player") 
        {
            foreach (ContactPoint contact in collision.contacts)
            {
                CreateSpringJoint(collision.rigidbody, contact.point);
            }
            
        }
    }

    void CreateSpringJoint(Rigidbody target, Vector3 contactPoint)
    {
        SpringJoint springJoint = this.gameObject.AddComponent<SpringJoint>();
        springJoints.Add(springJoint);
        
        springJoint.connectedBody = target;
        springJoint.anchor = new Vector3(0f, 0.5f, 0f);
        springJoint.connectedAnchor = contactPoint;
        springJoint.spring = 40f;
        springJoint.damper = 4f;
        springJoint.breakForce = 35f;

        EnemyManager enemyManager = target.gameObject.GetComponent<EnemyManager>();
        if (enemyManager != null)
        {
            enemyManager.attachedJoints.Add(springJoint);
        }
    }

    void OnJointBreak(float breakForce)
    {
        refreshSpringJoints();
        //if only I could find which joint broke ffs lol
    }

    void refreshSpringJoints()
    {
        //remove broken springjoint from springjoints list
        List<SpringJoint> newList = new List<SpringJoint>(springJoints);
        foreach (SpringJoint joint in springJoints)
        {
            if (joint == null) newList.Remove(joint);
        }
        springJoints = newList;
    }

    public void DealDamageToAllAttached(float damage)
    {
        List<Rigidbody> exploredBodies = new List<Rigidbody>();
        DealDamageToAllAttached(exploredBodies, damage);
    }

    public void DealDamageToAllAttached(List<Rigidbody> exploredBodies, float damage)
    {
        if (exploredBodies.Contains(rb)) return; //If we've been here before, leave

        //If we haven't been here before, take damage
        TakeDamage();
        exploredBodies.Add(rb);

        //Let's check if there are any other connected webs/enemies that need to take damage
        foreach (SpringJoint joint in springJoints)
        {
            WebProjectile webProjectile = joint.connectedBody.GetComponent<WebProjectile>();
            if (webProjectile != null)
            {
                webProjectile.DealDamageToAllAttached(exploredBodies, damage);
                break;
            }

            EnemyManager enemyManager = joint.connectedBody.GetComponent<EnemyManager>();
            if (enemyManager != null)
            {
                enemyManager.TakeDamage(damage);
            }
        }
    }

    public void TakeDamage()
    {
        //Make color flash white here
    }

    public List<GameObject> GetAllAttachedEnemies(List<Rigidbody> exploredBodies, List<GameObject> foundEnemies)
    {
        if (exploredBodies.Contains(rb)) return foundEnemies; //If we've been here before, leave

        //If we haven't been here before, let's add this to the attached enemies
        exploredBodies.Add(this.rb);
        
        refreshSpringJoints();
        foreach (SpringJoint joint in springJoints)
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
            if (joint.connectedBody.gameObject.tag == "Enemy")
            {
                EnemyManager enemyManager = joint.connectedBody.gameObject.GetComponent<EnemyManager>();
                foundEnemies = enemyManager.GetAllAttachedEnemies(exploredBodies, foundEnemies);
            }
            if (joint.connectedBody.gameObject.tag == "Web")
            {
                WebProjectile enemyManager = joint.connectedBody.gameObject.GetComponent<WebProjectile>();
                foundEnemies = enemyManager.GetAllAttachedEnemies(exploredBodies, foundEnemies);
            }
        }

        return foundEnemies;
    }
}
