using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hpPoints : MonoBehaviour {


    public int maxHpPoints;

    private float points;

	void Start () {
        points = maxHpPoints; 
	}

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "ball")
        {
            if (collision.gameObject.GetComponent<gravityBall>().damage)
            {
                points -= damageSteps(collision.gameObject.GetComponent<Rigidbody>().mass);
                Debug.Log(points);
                if (points < 0)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    private float damageSteps(float mass)
    {
        if (mass < 2)
        {
            return 10;
        }
        else
        {
            if (mass < 10)
            {
                return 25;
            }
            else
            {
                if (mass < 50)
                {
                    return 100;
                }
                else
                {
                    if (mass < 250)
                    {
                        return 200;
                    }
                    else
                    {
                        if (mass < 50000)
                        {
                            return 500;
                        }
                        else
                        {
                            return Mathf.Infinity;
                        }
                    }
                }
            }
        }
    }
}
