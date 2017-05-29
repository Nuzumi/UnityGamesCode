using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraControlerScript : MonoBehaviour {

    public GameObject player;

    private Vector3 distance;

    private void Start()
    {
        distance = transform.position - player.transform.position;
    }

    private void LateUpdate()
    {
        transform.position = player.transform.position + distance;
    }
}
