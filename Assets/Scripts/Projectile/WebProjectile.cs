using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebProjectile : MonoBehaviour
{
    // Start is called before the first frame update
    public List<SpringJoint> springJoints = null;
    public float lifetime = 20f;
    private float spawntime = 0f;

    void Awake()
    {
        springJoints = new List<SpringJoint>();

        spawntime = Time.time;
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
    }

    void OnJointBreak()
    {
        foreach (SpringJoint joint in springJoints)
        {
            if (joint == null) springJoints.Remove(joint);
        }
    }

    public void DealDamageToAllAttached()
    {
        List<Rigidbody> exploredBodies = new List<Rigidbody>();
        DealDamageToAllAttached(exploredBodies);
    }

    public void DealDamageToAllAttached(List<Rigidbody> exploredBodies)
    {
        foreach (SpringJoint joint in springJoints)
        {
            WebProjectile webProjectile = joint.connectedBody.GetComponent<WebProjectile>();
            if (webProjectile != null)
            {
                webProjectile.DealDamageToAllAttached(exploredBodies);
            }
        }
    }

    public void TakeDamage()
    {
        //Make color flash white here
    }
}
