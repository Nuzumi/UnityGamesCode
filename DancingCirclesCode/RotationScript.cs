using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationScript : MonoBehaviour {

    public float speed;

    private Vector3 rotator;

	void Start () {
        rotator = new Vector3(0, 0, 1);
	}
	
	
	void Update () {
        transform.Rotate(rotator, 10 * speed*Time.deltaTime);
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            GameObject.Destroy(gameObject);
        }
    }
}
