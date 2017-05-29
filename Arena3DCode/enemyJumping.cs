using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyJumping : MonoBehaviour {

    public int force;

    private bool isOn;
    private bool canJump;
    private GameObject player;
    private Rigidbody rb;

	void Start () {
        isOn = true;
        canJump = true;
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	void FixedUpdate () {
        if (isOn)
        {
            if (canJump)
            {
                Jump();
            }
        }	
	}

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "floor")
        {
            canJump = true;
        }
    }

    private void Jump()
    {
        canJump = false;
        Vector3 jumpDirection = helper.Vector(transform.position, player.transform.position);
        Vector3 jumpDirectionVersor = helper.Versor(jumpDirection);
        jumpDirectionVersor.y += 1;
        jumpDirectionVersor = helper.Versor(jumpDirectionVersor);
        rb.AddForce(jumpDirectionVersor * force);
    }
}
