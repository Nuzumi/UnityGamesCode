using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteObstacle : MonoBehaviour {

    public float minObstacleSpeed;
    public float maxObstacleSpeed;
    public int pointValue;

    private float obstacleSpeed;
    private GameObject player;
    private GameObject pointCounter;
    private Rigidbody2D rb;
    private Vector2 velocity;
    private bool isWall;

    private void Start()
    {
        isWall = false;
        obstacleSpeed = Random.Range(minObstacleSpeed, maxObstacleSpeed);
        pointCounter = GameObject.FindGameObjectsWithTag("PointCounter")[0];
        player = GameObject.Find("Circle");
        rb = GetComponent<Rigidbody2D>();
        velocity = makeVectorToCenter();
        rb.velocity = velocity * obstacleSpeed;
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if(collision.gameObject.tag == "Destroy")
        {
            pointCounter.SendMessage("pointUp",pointValue);
            Destroy(gameObject);
        }
    }

    private Vector2 makeVectorToCenter()
    {
        Vector2 vect;

        if (isWall)
        {
            vect = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y);
        }
        else
        {
             vect = new Vector2(-1 * transform.position.x, -1 * transform.position.y);
        }


        float dlugosc = Mathf.Sqrt(Mathf.Pow(vect.x, 2) + Mathf.Pow(vect.y, 2));
        vect *= (1 / dlugosc);

        return vect;
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
