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
        if ( Time.time > spawntime + lifetime )
        {
            Destroy(this.gameObject);
        }
    }


    void OnCollisionEnter(Collision collision)
    {
        if ( collision.rigidbody != null  && collision.gameObject.tag != "Player"  && collision.gameObject.tag != "Web") 
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
        springJoint.breakForce = 50f;
    }

}
