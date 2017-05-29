using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterMovementScript : MonoBehaviour {

    public GameObject battleControl;
    public float speed;
    public int movesPerTurn;

    public GameObject plate; // zmienic
    private int plateType;
    private Collider counterCollider;
    private Vector3 vectorToGo;
    private bool isGoing;
    private int movesCounter;
    private float heightTo;
    private bool goUp;
    private bool isUp;

    private void Start()
    {
        heightTo = transform.position.y;
        counterCollider = GetComponent<Collider>();
        // plate = null;
        isGoing = false;
        movesCounter = 0;
        goUp = false;
        isUp = false;
    }

    private void Update()
    {
        if (isGoing)
        {
            transform.Translate(vectorToGo * speed * Time.deltaTime);
        }

        if (goUp)
        {
            transform.Translate(Vector3.up * 0.1f);
            if (transform.position.y > heightTo)
            {
                isUp = true;
                goUp = false;
                heightTo = 0.2f;
            }
        }
    }

    private void OnMouseDown()
    {
        if (plate != null)
        {
            if (movesCounter < movesPerTurn)
            {
                plate.SendMessage("makeNeighbourViableToGo");
            }
            battleControl.SendMessage("setActivCounter", gameObject);
        }
    }

    void setPlate(GameObject plateToGet, int plateTypeToGet)
    {
        plate = plateToGet;
        plateType = plateTypeToGet;
    }

    void setPlateToGo(GameObject plateToGo)
    {
        if (isUp)
        {
            isUp = false;
            transform.position = new Vector3(transform.position.x, 0.2f, transform.position.z);
        }
        movesCounter++;
        plate.SendMessage("freePlate");
        plate.SendMessage("makeNeighbourNotViableToGo");
        isGoing = true;
        vectorToGo = FunctionHelperScript.makeVersor(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(plateToGo.transform.position.x, 0, plateToGo.transform.position.z));
        plate = null;
    }

    void stopMoving(GameObject newPlate)
    {
        isGoing = false;
        plate = newPlate;
    }

    void turnOffCollider()
    {
        counterCollider.enabled = false;
    }

    void turnOnCollider()
    {
        counterCollider.enabled = true;
    }

    void turnOffPlates()
    {
        plate.SendMessage("makeNeighbourNotViableToGo");
    }

    public GameObject getPlate()
    {
        return plate;
    }

    void turnReset()
    {
        movesCounter = 0;
    }

    void moveUp(int plateTy)
    {
        goUp = true;
        if(plateTy == 2)
        {
            heightTo += 0.8f;
        }
        else
        {
            heightTo += 0.53f;
        }
    }

    void slow()
    {
        movesCounter++;
    }

    void snare()
    {
        movesCounter = 99999999;
    }
}
