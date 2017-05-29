using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterScript : MonoBehaviour {

    public GameObject bulets;
    public int minAmmo;
    public int maxAmmo;
    public float minFireRate;
    public float maxFireRate;
    public float minBuletSpeed;
    public float maxBuletSpeed;

    private float buletSpeed;
    private int ammo;
    private float fireRate;
    private int buletsFired;
    private GameObject bulet;
    private GameObject player;
    private float timeFire;
    private List<Vector2> targetList;
    private bool isWall;

	void Start () {
        isWall = false;
        buletSpeed = Random.Range(minBuletSpeed, maxBuletSpeed);
        fireRate = Random.Range(minFireRate, maxFireRate);
        ammo = Random.Range(minAmmo, maxAmmo);
        timeFire = 0;
        timeFire = Time.time;
        targetList = new List<Vector2>();
        player = GameObject.Find("Circle");

        if (Mathf.Abs(transform.position.y) == 6)
        {
            makeTargetList(true);
        }
        else
        {
            makeTargetList(false);
        }
	}
	
	
	void Update () {
        if (timeFire < Time.time && buletsFired <ammo)
        {
            timeFire += fireRate;
            buletsFired++;

            bulet = Instantiate(bulets, transform.position, Quaternion.identity);
            bulet.GetComponent<Rigidbody2D>().velocity = makeVelocityVersor()*buletSpeed;
        }

        if(buletsFired == ammo)
        {
            Destroy(gameObject);
        }
	}

    void makeTargetList(bool isY0)
    {

        if (isY0)
        {
            float odcinki = 6 / ((float)ammo + 1);

            for(int i = 1; i <= ammo; i++)
            {
                targetList.Add(new Vector2(-3 + odcinki * i, 0));
            }
        }
        else
        {
            if (isWall)
            {
                float odcinki = 5 / ((float)ammo + 1);

                if (player.transform.position.y > 0)
                {
                    for (int i = 1; i <= ammo; i++)
                    {
                        targetList.Add(new Vector2(0,  odcinki * i));
                    }
                }
                else
                {
                    for (int i = 1; i <= ammo; i++)
                    {
                        targetList.Add(new Vector2(0, -5 + odcinki * i));
                    }
                }
            }
            else
            {
                float odcinki = 10 / ((float)ammo + 1);

                for (int i = 1; i <= ammo; i++)
                {
                    targetList.Add(new Vector2(0, -5 + odcinki * i));
                }
            }

        }
    } 

    Vector2 makeVelocityVersor()
    {
        Vector2 vector = targetList[0];
        targetList.RemoveAt(0);

        float odlegloc = Mathf.Sqrt(Mathf.Pow(transform.position.x - vector.x, 2) + Mathf.Pow(transform.position.y - vector.y, 2));

        vector = new Vector2(vector.x - transform.position.x, vector.y - transform.position.y)*(1/odlegloc);
        return vector;
    }
    
    void wallOn()
    {
        isWall = true;
    }

    void wallOff()
    {
        isWall = false;
    }

}
