using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpownScript : MonoBehaviour {

    public GameObject bonusObject;
    public GameObject whiteObstacle;
    public GameObject shooter;
    public GameObject wall;
    public GameObject followingObstacle;
    public GameObject triangleSpown;
    public float timerBonus;
    public float timerDelayBonus;
    public float timerObstacle;
    public float timerShooter;
    public float timeDelayShooter;
    public float timerWall;
    public float timeDelayWall;
    public float timerFollowing;
    public float timeDelayFollowing;
    public float timerTriangleSpown;
    public float timerDelayTriangleSpown;

    private GameObject shoot;
    private GameObject white;
    private GameObject player;
    private float timeSpownBonus;
    private float timeSpownObstacle;
    private float timeSpownShooter;
    private float timeSpownWall;
    private float timeSpownFollowing;
    private float timeSpowmTriangleSpown;
    private int spownX;
    private int spownY;
    private int bonusCount;
    private bool isOn;
    private bool isWall;

	void Start () {
        player = GameObject.Find("Circle");
        isOn = true;
        isWall = false;
        bonusCount = 2;
        timeSpownObstacle = 0;
        timeSpownShooter = timeDelayShooter;
        timeSpownWall = timeDelayWall;
        timeSpownBonus = timerDelayBonus;
        timeSpownFollowing = timeDelayFollowing;
        timeSpowmTriangleSpown = timerDelayTriangleSpown;
	}

    private void Update()
    { 
        if (isOn)
        {
            whiteObstacleSpown();
            shooterSpown();
            wallSpown();
            bonusSpawn();
            followingSpawn();
            triangleSpawnSpawn();
        }
    }


    private void whiteObstacleSpown()
    {
        if (timeSpownObstacle < Time.timeSinceLevelLoad)
        {
            timeSpownObstacle += timerObstacle;

            if (Random.Range(-1, 1) < 0)
            {
                if (Random.Range(-1, 1) < 0)
                {
                    spownX = 4;
                }
                else
                {
                    spownX = -4;
                }

                spownY = Random.Range(-6, 6);
            }
            else
            {
                if (Random.Range(-1, 1) < 0)
                {
                    spownY = 6;
                }
                else
                {
                    spownY = -6;
                }

                spownX = Random.Range(-4, 4);
            }

           white =  Instantiate(whiteObstacle, new Vector2(spownX, spownY), Quaternion.identity);
            if (isWall)
            {
                white.SendMessage("wallOn");
            }
        }
    }

    private void shooterSpown()
    {
            if (timeSpownShooter < Time.timeSinceLevelLoad)
            {
                timeSpownShooter += timerShooter;

                if (Random.Range(-1, 1) < 0)
                {
                    if (Random.Range(-1, 1) < 0)
                    {
                        spownX = 4;
                    }
                    else
                    {
                        spownX = -4;
                    }

                    spownY = Random.Range(-6, 6);
                }
                else
                {
                    if (Random.Range(-1, 1) < 0)
                    {
                        spownY = 6;
                    }
                    else
                    {
                        spownY = -6;
                    }

                    spownX = Random.Range(-4, 4);
                }

               shoot =  Instantiate(shooter, new Vector2(spownX, spownY), Quaternion.identity);

            if (isWall)
            {
                shoot.SendMessage("wallOn");
            }
            }
    }

    private void wallSpown()
    {
            if(timeSpownWall < Time.timeSinceLevelLoad)
            {
                timeSpownWall += timerWall;

                spownY = 0;

                if (Random.Range(-1, 1) < 0)
                {
                    spownX = -5;
                }
                else
                {
                    spownX = 5;
                }
              
               Instantiate(wall, new Vector2(spownX, spownY), Quaternion.identity);
            player.SendMessage("wallOn");
            }
        }

    private void bonusSpawn()
    {
        if(timeSpownBonus < Time.timeSinceLevelLoad)
        {
            float spownXBonus = Random.Range(-2, 2); ;
            float spownYBonus;
            GameObject[] bonusList = GameObject.FindGameObjectsWithTag("Bonus");

            timeSpownBonus += timerDelayBonus;

            if (Random.Range(-1, 1) < 0)
            {
                spownYBonus = Random.Range(-4f, -0.8f);
            }
            else
            {
                spownYBonus = Random.Range(0.8f, 4);
            }

            if(bonusList.Length != 0)
            {
                if (bonusCount % 2 == 0)
                {
                    bonusList[0].transform.position = new Vector2(spownXBonus, spownYBonus);
                }
                else
                {
                    Destroy(bonusList[0]);
                }
            }
            else
            {
                if (bonusCount % 2 == 0)
                {
                    Instantiate(bonusObject, new Vector2(spownXBonus, spownYBonus), Quaternion.identity);
                }
            }
            bonusCount++;
        }
    }

    private void followingSpawn()
    {
        if (timeSpownFollowing < Time.timeSinceLevelLoad)
        {
            timeSpownFollowing += timerFollowing;

            if (Random.Range(-1, 1) < 0)
            {
                if (Random.Range(-1, 1) < 0)
                {
                    spownX = 4;
                }
                else
                {
                    spownX = -4;
                }

                spownY = Random.Range(-6, 6);
            }
            else
            {
                if (Random.Range(-1, 1) < 0)
                {
                    spownY = 6;
                }
                else
                {
                    spownY = -6;
                }

                spownX = Random.Range(-4, 4);
            }

            Instantiate(followingObstacle, new Vector2(spownX, spownY), Quaternion.identity);
        }
    }

    private void triangleSpawnSpawn()
    {
        if (timeSpowmTriangleSpown < Time.timeSinceLevelLoad)
        {
            timeSpowmTriangleSpown += timerTriangleSpown;

            if (Random.Range(-1, 1) < 0)
            {
                if (Random.Range(-1, 1) < 0)
                {
                    spownX = 4;
                }
                else
                {
                    spownX = -4;
                }

                spownY = Random.Range(-6, 6);
            }
            else
            {
                if (Random.Range(-1, 1) < 0)
                {
                    spownY = 6;
                }
                else
                {
                    spownY = -6;
                }

                spownX = Random.Range(-4, 4);
            }

            white = Instantiate(triangleSpown, new Vector2(spownX, spownY), Quaternion.identity);
            if (isWall)
            {
                white.SendMessage("wallOn");
            }
        }
    }

    void turnOff()
    {
        isOn = false;
    }

    void turnOn()
    {
        isOn = true;
    }

    void wallOff()
    {
        isWall = false;
    }

}
