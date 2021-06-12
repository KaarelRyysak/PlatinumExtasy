using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    
    [SerializeField] public GameObject objectToFollow;
    Vector3 offset = new Vector3(0, 0, 0);
    void Start()
    {
        // Get the offset of the object I'm linked to
        offset = transform.position - objectToFollow.transform.position;
    }

    void Update()
    {
        transform.position = objectToFollow.transform.position + offset;
    }
}
