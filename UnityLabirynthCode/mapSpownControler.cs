using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapSpownControler : MonoBehaviour {

    private GameObject mapControler;

    private void Start()
    {
        mapControler = GameObject.FindGameObjectWithTag("mapController");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            mapControler.SendMessage("crossed", transform.position);
        }
    }
}
