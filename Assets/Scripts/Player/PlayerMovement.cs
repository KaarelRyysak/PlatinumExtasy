using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Vector2 inputVector = new Vector2(0, 0);
    float speed = 5f;
    Rigidbody rb = null;

    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    void Update()
    {
        inputVector.x = Input.GetAxis("Horizontal");
        inputVector.y = Input.GetAxis("Vertical");
        inputVector = inputVector.normalized;
        //print(inputVector.ToString());

        //Let's rotate towards the mouse :)
        Vector3 mouse_pos = Input.mousePosition;
        Vector3 object_pos = Camera.main.WorldToScreenPoint(transform.position);
        mouse_pos.x = mouse_pos.x - object_pos.x;
        mouse_pos.y = mouse_pos.y - object_pos.y;
        float angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, -angle, 0));

        //This fixes perpetual motion on collision
        rb.velocity = Vector3.zero;
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
