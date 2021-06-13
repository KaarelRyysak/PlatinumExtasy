using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebProjectile : MonoBehaviour
{
    public List<SpringJoint> springJoints = null; //All currently connected joints (+1?)
    public float lifetime = 20f;
    private float spawntime = 0f;

    private Rigidbody rb;

    private MeshRenderer meshRenderer = null;
    private Color objectColor;
    public Color fadeColor = Color.white;
    public float fadeDuration = 5f;
    public float fadeStartTime = 0f;

    void Awake()
    {
        springJoints = new List<SpringJoint>();

        spawntime = Time.time;

        rb = this.gameObject.GetComponent<Rigidbody>();

        //Find meshrenderer to change material color
        meshRenderer = this.gameObject.GetComponent<MeshRenderer>();
        objectColor = meshRenderer.material.color;
        meshRenderer.material.color = fadeColor;
        fadeStartTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        //If lifetime is over, destroy self
        if ( Time.time > spawntime + lifetime )
        {
            Destroy(this.gameObject);
        }

        //Flash white when necessary
        if (Time.time < fadeDuration + fadeStartTime)
        {
            float percent = (Time.time - fadeStartTime) / fadeDuration;
            gameObject.GetComponent<Renderer>().material.color = Color.Lerp(fadeColor, objectColor, percent);
        }
    }

    public void FlashWhite()
    {
        fadeStartTime = Time.time;
        meshRenderer.material.color = fadeColor;
    }

    void OnCollisionEnter(Collision collision)
    {
        if ( collision.rigidbody != null && collision.gameObject.tag != "Player") 
        {
            foreach (ContactPoint contact in collision.contacts)
            {
                CreateSpringJoint(collision.rigidbody, contact.point);
            }
            
        }
        else if (collision.gameObject.tag == "Environment")
        {
            foreach (ContactPoint contact in collision.contacts)
            {
                AttachToEnvironment(contact.point);
            }
        }
    }

    void AttachToEnvironment(Vector3 contactPoint)
    {
        SpringJoint springJoint = this.gameObject.AddComponent<SpringJoint>();
        springJoints.Add(springJoint);
        
        springJoint.anchor = new Vector3(0f, 0.5f, 0f);
        springJoint.connectedAnchor = contactPoint;
        springJoint.spring = 40f;
        springJoint.damper = 4f;
        springJoint.breakForce = 50f;
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
        springJoint.breakForce = 50f;

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
        TakeDamage();

        List<GameObject> attachedEnemies = GetAllAttachedEnemies();
        foreach (GameObject enemyObj in attachedEnemies)
        {
            EnemyManager enemyManager = enemyObj.gameObject.GetComponent<EnemyManager>();
            enemyManager.TakeDamage(damage);
        }
    }

    public void TakeDamage()
    {
        FlashWhite();
    }

    public List<GameObject> GetAllAttachedEnemies()
    {
        List<Rigidbody> exploredBodies = new List<Rigidbody>();
        List<GameObject> foundEnemies = new List<GameObject>();
        foundEnemies = GetAllAttachedEnemies(exploredBodies, foundEnemies);
        return foundEnemies;
    }
    public List<GameObject> GetAllAttachedEnemies(List<Rigidbody> exploredBodies, List<GameObject> foundEnemies)
    {
        if (exploredBodies.Contains(rb)) return foundEnemies; //If we've been here before, leave

        //If we haven't been here before, let's add this to the attached enemies
        exploredBodies.Add(this.rb);
        
        refreshSpringJoints();
        List<SpringJoint> newSpringJoints = new List<SpringJoint>(springJoints);
        foreach (SpringJoint joint in springJoints)
        {
            if (joint == null)
            {
                newSpringJoints.Remove(joint);
                continue;
            }
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
            if (joint.connectedBody == null)
            {
                newSpringJoints.Remove(joint);
                continue;
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
