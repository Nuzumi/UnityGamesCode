using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovementScript : MonoBehaviour {

    public float speed;

    private Rigidbody rb;
    private Vector3 velocity;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        velocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * speed;
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + velocity * Time.deltaTime);
    }

    Vector3 getPoint()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Vector3 point =  ray.origin + (ray.direction * 18);

        return new Vector3(point.x, 5, point.z);
    }

}
