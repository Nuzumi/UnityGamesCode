using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gravityBall : MonoBehaviour {

    public int forceForBalls;
    public int forceForEnemis;
    public bool damage;

    private bool on;
    private List<Rigidbody> ballsInRange;
    private List<Rigidbody> enemisInRange;

	void Start () {
        damage = true;
        on = false;
        ballsInRange = new List<Rigidbody>();
        enemisInRange = new List<Rigidbody>();
	}

    private void FixedUpdate()
    {
        if (ballsInRange.Count > 0)
        {
            for (int i = 0; i < ballsInRange.Count; i++)
            {
                if (ballsInRange[i] != null)
                {
                    Vector3 forceDirection = helper.Versor(helper.Vector(ballsInRange[i].transform.position, transform.position));
                    ballsInRange[i].AddForce(forceDirection * forceForBalls * Time.deltaTime);
                }
            }
        }

        if (enemisInRange.Count > 0)
        {
            for (int i = 0; i < enemisInRange.Count; i++)
            {
                if (enemisInRange[i] != null)
                {
                    Vector3 forceDirection = helper.Versor(helper.Vector(transform.position,enemisInRange[i].transform.position));
                    enemisInRange[i].AddForce(forceDirection * forceForEnemis * Time.deltaTime);
                }
            }
        }

        ballsInRange.Remove(null);
        enemisInRange.Remove(null);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (on)
        {
            if (other.gameObject.tag == "ball")
            {
                ballsInRange.Add(other.gameObject.GetComponent<Rigidbody>());
            }
            else
            {
                if(other.gameObject.tag == "enemy")
                {
                    Debug.Log("add enemy");
                    enemisInRange.Add(other.gameObject.GetComponent<Rigidbody>());
                    other.gameObject.GetComponent<Rigidbody>().useGravity = false;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (on)
        {
            if (other.gameObject.tag == "ball")
            {
                ballsInRange.Remove(other.gameObject.GetComponent<Rigidbody>());
            }
            else
            {
                if (other.gameObject.tag == "enemy")
                {
                    Debug.Log("removed enemy");
                    enemisInRange.Remove(other.gameObject.GetComponent<Rigidbody>());
                    other.gameObject.GetComponent<Rigidbody>().useGravity = true;
                }
            }
        }
    }

    public void GravityOn()
    {
        on = true;
        damage = false;
    }

    public void GravityOff()
    {
        on = false;
    }
}
