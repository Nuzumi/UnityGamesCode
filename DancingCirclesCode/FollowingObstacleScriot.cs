using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingObstacleScriot : MonoBehaviour {

    public float speedTimer;
    public float rotationTimer;
    public float speed;
    public float liveTime;
    public int pointValue;

    private float insideSpeedTimer;
    private float insideRotationtimer;
    private Rigidbody2D rb;
    private GameObject player;
    private Vector2 childPosition;
    private GameObject pointCounter;

	void Start ()
    {
        pointCounter = GameObject.FindGameObjectsWithTag("PointCounter")[0];
        Destroy(gameObject, liveTime);
        player = GameObject.Find("Circle");
        insideSpeedTimer = Time.timeSinceLevelLoad;
        insideRotationtimer = Time.timeSinceLevelLoad;
        rb = GetComponent<Rigidbody2D>();
	}
	
	void Update ()
    {
        if (insideSpeedTimer < Time.timeSinceLevelLoad)
        {
            insideSpeedTimer += speedTimer;

            rb.velocity = makeVersorToPlayer() * speed * Time.deltaTime;
        }

        if (insideRotationtimer < Time.timeSinceLevelLoad)
        {
            insideRotationtimer += rotationTimer;
            transform.Rotate(new Vector3(0, 0, makeRotationAngle()));
        }
	}

    private void OnDestroy()
    {
        pointCounter.SendMessage("pointUp", pointValue);
    }

    private Vector2 makeVersorToPlayer()
    {
        Vector2 vect;
        vect = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y);
        float dlugosc = Mathf.Sqrt(Mathf.Pow(vect.x, 2) + Mathf.Pow(vect.y, 2));
        vect *= (1 / dlugosc);

        return vect;
    }

    private float makeRotationAngle()
    {
        float wspolczynnikA;
        float wspolczynnikB;
        childPosition = transform.GetChild(0).transform.position;
        float centreToChild = odlegloscPunktow(transform.position, childPosition);
        float centreToPlayer = odlegloscPunktow(transform.position, player.transform.position);
        float childToPlayer = odlegloscPunktow(childPosition, player.transform.position);
        if (centreToPlayer == 0)
        {
            return 0;
        }
        if(childToPlayer == 0)
        {
            return 0;
        }
        float p = (centreToChild + centreToPlayer + childToPlayer) / 2;
        if((p - childToPlayer)<=0)
        {
            return 0;
        }
        float sinA = (2 / (centreToPlayer * centreToChild)) * Mathf.Sqrt(p * (p - centreToChild) * (p - centreToPlayer) * (p - childToPlayer));
        float angle = Mathf.Asin(sinA) * Mathf.Rad2Deg;

        if (transform.position.x != player.transform.position.x)
        {
            wspolczynnikA = (transform.position.y - player.transform.position.y) / (transform.position.x - player.transform.position.x);
        }
        else
        {
            wspolczynnikA = (transform.position.y - player.transform.position.y) / (transform.position.x + 0.1f - player.transform.position.x);
        }

        if(childPosition.x != player.transform.position.x)
        {
            wspolczynnikB = (childPosition.y - player.transform.position.y) / (childPosition.x - player.transform.position.x);
        }
        else
        {
            wspolczynnikB = (childPosition.y - player.transform.position.y) / (childPosition.x + 0.1f - player.transform.position.x);
        }

        if (Mathf.Atan(wspolczynnikA) > Mathf.Atan(wspolczynnikB))
        {
            return angle;
        }
        else
        {
            return -1 * angle;
        }
        

    }

    private float odlegloscPunktow(Vector2 start,Vector2 end)
    {
        return Mathf.Sqrt(Mathf.Pow(start.x - end.x, 2) + Mathf.Pow(start.y - end.y, 2));
    }

    public void dstr()
    {
        Destroy(gameObject);
    }
}
