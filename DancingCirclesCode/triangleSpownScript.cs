using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triangleSpownScript : MonoBehaviour {

    public GameObject trianglePreFab;
    public float spownRate;

    private float timer;

    private void Start()
    {
        timer = Time.timeSinceLevelLoad;
    }

    private void Update()
    {
        Spown();
    }

    void Spown()
    {
        if (timer < Time.timeSinceLevelLoad)
        {
            timer += spownRate;
            Instantiate(trianglePreFab,transform.position,Quaternion.identity);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Destroy")
        {
            Destroy(gameObject);
        }
    }

}
