using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basicSchhoBall : MonoBehaviour {

    public GameObject ball;
    public int force;

    private bool isShooting;
    private bool isBall;
    private bool addForce;
    private bool isGravityBall;
    private GameObject spawnedBall;
    private Rigidbody ballsRigidbody;

    private void Start()
    {
        isShooting = false;
        isBall = false;
        addForce = false;
        isGravityBall = false;
    }

    void Update () {
        if (Input.GetButtonDown("Fire1"))
        {
            isShooting = true;
            isBall = true;
        }

        if (Input.GetButtonUp("Fire1"))
        {
            if (isGravityBall)
            {
                isGravityBall = false;
            }
            else
            {
                addForce = true;
            }
        }

        if (isBall && spawnedBall!=null)
        {
            spawnedBall.transform.position = transform.position;
        }
	}

    private void FixedUpdate()
    {
        if (isShooting)
        {
            isShooting = false;
            spawnedBall = Instantiate(ball,new Vector3(transform.position.x,transform.position.y,transform.position.z),Quaternion.identity);
            spawnedBall.transform.eulerAngles = new Vector3(transform.parent.eulerAngles.x,transform.parent.parent.eulerAngles.y,0);
            ballsRigidbody = spawnedBall.GetComponent<Rigidbody>();
            spawnedBall.transform.SetParent(gameObject.transform);
        }

        if (isBall)
        {
            if (spawnedBall.transform.localScale.x<=1.1)
            {
                spawnedBall.transform.localScale += new Vector3(0.2f, 0.2f, 0.2f) * Time.deltaTime;
                ballsRigidbody.mass *= 1.1f;
                spawnedBall.SendMessage("On");
            }
            else
            {
                spawnedBall.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                ballsRigidbody.constraints = RigidbodyConstraints.FreezeAll;
                isGravityBall = true;
                isBall = false;
                transform.DetachChildren();
                spawnedBall.SendMessage("GravityOn");
                spawnedBall = null;
            }
        }

        if (addForce && !isGravityBall)
        {
            addForce = false;
            isBall = false;
            transform.DetachChildren();
            ballsRigidbody.AddForce(spawnedBall.transform.forward * force);
            spawnedBall.SendMessage("On");
            spawnedBall = null;
        }
    }
}
