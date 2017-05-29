using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleScript : MonoBehaviour {

    public int lives;
    public bool spining;
    public float ax;
    public float speed;
    public float timeWait;
    public GameObject follower;
    public GameObject pointCounter;
    public GameObject spown;
    public GameObject canvas;

    private bool isThereWall;
    private bool canBeStoped;
    private bool canSpin;
    private List<Vector2> positionList;
    private Rigidbody2D rb;
    private Vector3 rotationPoint;

	void Start () {
        positionList = new List<Vector2>();
        rb = GetComponent<Rigidbody2D>();
        spining = true;
        rotationPoint = makePoint();
        canSpin = true;
        canBeStoped = false;
        isThereWall = false;
	}

    private void Update()
    {
        if (spining)
        {
            transform.RotateAround(rotationPoint, new Vector3(0, 0, 1), 40 * Time.deltaTime * speed);
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (!canBeStoped)
            {
                pointCounter.SendMessage("canCountOn");
            }
            canBeStoped = true;
            if (canSpin)
            {
                if (spining)
                {
                    spining = false;
                    rb.velocity = makeAfterSpinVelocity() * speedMotifacator();
                }
                else
                {

                    rotationPoint = makePoint();
                    spining = true;
                    rb.velocity = Vector2.zero;
                }
            }
            
        }
            

            positionList.Add(transform.position);      

        if (Time.timeSinceLevelLoad > timeWait)
        {
            follower.transform.position = positionList[0];
            positionList.RemoveAt(0);
        }
    }

  /*  private void FixedUpdate()
    {
        if (!spining)
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0.0f);

            rb.AddForce(movement * speed);
        }
    }*/

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (canBeStoped)
        {
            if (collision.gameObject.tag == "Obstacle")
            {
                if(lives == 1)
                {
                    spown.SendMessage("turnOff");
                    pointCounter.SendMessage("canCountOff");
                    canSpin = false;
                    spining = false;
                    rb.velocity = Vector2.zero;
                    canvas.SendMessage("stop");
                }
                else
                {
                    turnOffDepestFolower(follower);
                }
            }
            else
            {
                if (collision.gameObject.tag == "Bonus")
                {
                    pointCounter.SendMessage("bonusPointUp", 1);
                }
                else
                {
                    if (collision.gameObject.tag == "followingObstacle")
                    {
                        if (lives == 1)
                        {
                            spown.SendMessage("turnOff");
                            pointCounter.SendMessage("canCountOff");
                            canSpin = false;
                            spining = false;
                            rb.velocity = Vector2.zero;
                            canvas.SendMessage("stop");
                        }
                        else
                        {
                            turnOffDepestFolower(follower);
                        }

                        collision.SendMessage("dstr");
                    }
                }
            }
        }        
    }

    private Vector3 makePoint()
    {
        float x;
        float y;


        if (isThereWall && Mathf.Abs(transform.position.y)< (1 *ax + 0.1f)*2)
        {

            if (transform.position.x > 0)
            {
                x = transform.position.x - (0.714f * ax +0.1f);
            }
            else
            {
                x = transform.position.x + (0.714f * ax + 0.1f);
            }

            if (transform.position.y > 0)
            {
                y = transform.position.y + (0.714f * ax + 0.1f);
            }
            else
            {
                y = transform.position.y - (0.714f * ax + 0.1f);
            }
        }
        else
        {
            if (Mathf.Abs(transform.position.x) < 1 * ax +0.1f)
            {
                if (transform.position.x == 0)
                {
                    x = 0.1f;
                }
                else
                {
                    x = 0;
                }

                if (transform.position.y > 1)
                {
                    y = transform.position.y - Mathf.Sqrt(Mathf.Pow(1 * ax + 0.1f, 2) - Mathf.Pow(transform.position.x, 2));
                }
                else
                {
                    y = transform.position.y + Mathf.Sqrt(Mathf.Pow(1 * ax + 0.1f, 2) - Mathf.Pow(transform.position.x, 2));
                }
            }
            else
            {
                if (transform.position.x > 0)
                {
                    x = transform.position.x - (0.714f * ax + 0.1f);
                }
                else
                {
                    x = transform.position.x + (0.714f * ax + 0.1f);
                }

                if (transform.position.y > 0)
                {
                    y = transform.position.y - (0.714f * ax + 0.1f);
                }
                else
                {
                    y = transform.position.y + (0.714f * ax + 0.1f);
                }
            }
        }

        return new Vector3(x, y, 0);
    }

    private Vector2 makeAfterSpinVelocity()
    {
        float srodekOkreguX = rotationPoint.x;
        float srodekOkreguY = rotationPoint.y;
        float punktStycznyX = transform.position.x;
        float punktStycznyY = transform.position.y;

        float a = (srodekOkreguY - punktStycznyY) / (srodekOkreguX - punktStycznyX);
        a = -1 / a;

        float b = punktStycznyY - a * punktStycznyX;

        Vector2 point1 = new Vector2(0, b);
        Vector2 point2 = new Vector2(1, a + b);
        Vector2 vector = new Vector2(point2.x - point1.x, point2.y - point1.y);
        float dlugosc = Mathf.Sqrt(Mathf.Pow(point2.x - point1.x, 2) + Mathf.Pow(point2.y - point1.y, 2));
        vector = new Vector2(vector.x * (1 / dlugosc), vector.y * (1 / dlugosc));

        return vector;
    }

    private float speedMotifacator()
    {
        float modificator = -1;

        if (transform.position.y <= rotationPoint.y)
        {
            modificator = 1;
        }

        modificator *= (Mathf.PI * 2 * ax * speed*2) / 9;
        return modificator;
    }

    private void turnOffSpining()
    {
        canSpin = false;
    }

    private void turnOffDepestFolower(GameObject fol)
    {
        if (fol.GetComponent<FollowingScript>().hasFollower && !fol.GetComponent<FollowingScript>().follower.GetComponent<FollowingScript>().isBlack)  //!fol.GetComponent<FollowingScript>().isBlack && fol.GetComponent<FollowingScript>().hasFollower
        {
            turnOffDepestFolower(fol.GetComponent<FollowingScript>().follower);
        }
        else
        {
            lives--;
            fol.SendMessage("turnOffColor");
        }
    }

    private void wallOn()
    {
        isThereWall = true;
    }

    private void wallOff()
    {
        isThereWall = false;
    }
}
