using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyScript : MonoBehaviour {

    public int pointValue;

    private GameObject pointCounter;

    private void Start()
    {
        pointCounter = GameObject.FindGameObjectsWithTag("PointCounter")[0];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Destroy")
        {
            pointCounter.SendMessage("pointUp", pointValue);
            Destroy(gameObject);
        }
    }
}
