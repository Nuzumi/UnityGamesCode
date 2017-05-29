using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallScript : MonoBehaviour {

    private GameObject plate;

    private void OnMouseDown()
    {
        plate.SendMessage("OnMouseDown");
    }

    void getPlate(GameObject plateToGet)
    {
        plate = plateToGet;
    }
}
