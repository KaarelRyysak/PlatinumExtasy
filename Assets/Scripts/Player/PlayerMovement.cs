using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Vector2 inputVector = new Vector2(0, 0);
    float speed = 5f;

    void Start()
    {
        
    }

    void Update()
    {
        inputVector.x = Input.GetAxis("Horizontal");
        inputVector.y = Input.GetAxis("Vertical");
        inputVector = inputVector.normalized;
        //print(inputVector.ToString());
    }

    private void FixedUpdate()
    {
        // I remembered transform.translate exists after I wrote this.
        Vector3 newPosition = transform.position;
        newPosition.x += inputVector.x * speed * Time.deltaTime;
        newPosition.z += inputVector.y * speed * Time.deltaTime;

        transform.position = newPosition;

    }
}
