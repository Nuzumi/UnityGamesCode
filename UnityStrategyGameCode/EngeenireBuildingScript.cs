using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngeenireBuildingScript : MonoBehaviour {

    public GameObject triangleTower;
    public GameObject squareTower;
    public GameObject hexagonTower;
    public int buildingsPerBattle;

    private GameObject plate;
    private int plateType;
    private int buildingsCounter;
    private bool isOn;

    private void Start()
    {
        plate = null;
        isOn = false;
    }

    private void OnMouseDown()
    {
        if (buildingsCounter < buildingsPerBattle)
        {
            if (!isOn)
            {
                if (plate != null)
                {
                    plate.SendMessage("builidingPlate");
                }
                isOn = true;
            }
            else
            {
                turnOffPlate();
                isOn = false;
            }
        }
    }

    void setPlateToGo(GameObject plate)
    {
        if(this.plate != null)
        {
            turnOffPlate();
        }
        this.plate = plate;
        plateType = plate.GetComponent<NodeScript>().plateType;
        isOn = false;
    }

    void turnOffPlate()
    {
        plate.SendMessage("makeNotViableToGo");
    }

    void build()
    {
        buildingsCounter++;
        switch (plateType)
        {
            case 0:
                GameObject triangle = Instantiate(triangleTower,new Vector3(plate.transform.position.x,0,plate.transform.position.z), Quaternion.identity);
                triangle.transform.eulerAngles = new Vector3(0, plate.transform.eulerAngles.y + 60, 0);
                plate.SendMessage("setBuilding", triangle);
                break;
            case 1:
                GameObject square = Instantiate(squareTower, plate.transform.position, Quaternion.identity);
                square.transform.rotation = plate.transform.rotation;
                plate.SendMessage("setBuilding", square);
                break;
            default:
                GameObject hexagon = Instantiate(hexagonTower, plate.transform.position, Quaternion.identity);
                plate.SendMessage("setBuilding", hexagon); ;
                break;
        }
        OnMouseDown();
    }
}
