using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallScript : MonoBehaviour {

    public int pointValue;
    public float wallGrowSpeed;
    public float destroyTime;

    private GameObject spown;
    private GameObject player;
    private GameObject pointCounter;

    private void Start()
    {
        pointCounter = GameObject.FindGameObjectsWithTag("PointCounter")[0];
        spown = GameObject.Find("Spown");
        player = GameObject.Find("Circle");
        Destroy(gameObject, destroyTime);
    }

    void Update () {
        transform.localScale += new Vector3(wallGrowSpeed * Time.deltaTime, 0, 0);
	}

    private void OnDestroy()
    {
        if (pointCounter != null)
        {
            pointCounter.SendMessage("pointUp", pointValue);
        }
        
        if(player != null)
        {
            player.SendMessage("wallOff");
        }

        if (spown != null)
        {
            spown.SendMessage("wallOff");
        }
    }
}
