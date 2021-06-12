using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    
    [SerializeField] public GameObject objectToFollow;
    public Vector3 offset = new Vector3(0, 0, 0);

    void OnEnable() {
        if (objectToFollow) SetTarget(objectToFollow);
    }

    public void SetTarget(GameObject target){

        objectToFollow = target;

        // Get the offset of the object I'm linked to
        offset = transform.position - objectToFollow.transform.position;
    }

    void Update()
    {
        if (objectToFollow) transform.position = objectToFollow.transform.position + offset;
    }
}
