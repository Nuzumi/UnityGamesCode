using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hpPointsScript : MonoBehaviour {

    public GameObject battleControler;
    public int hpPoints;

    private GameObject mainCamera;
    private GameObject plate;
    private GameObject hpBar;
    private List<SpriteRenderer> hpBarsList;
    private List<SpriteRenderer> hpBarsList2;

    private void Start()
    {
        mainCamera = GameObject.FindWithTag("MainCamera");
        hpBar = transform.GetChild(0).gameObject;
        hpBarsList = new List<SpriteRenderer>();
        hpBarsList2 = new List<SpriteRenderer>();
        fillHpList();
    }

    private void Update()
    {
        hpBar.transform.LookAt(mainCamera.transform.position);
    }

    void setPlateToGo(GameObject plate)
    {
        this.plate = plate;
    }

    void receiveAttack(int attackValue)
    {
        if (hpPoints - attackValue <= 0)
        {
            hpPoints = 0;
            turnOffHpPoints(hpPoints);
            plate.SendMessage("freePlate");
            battleControler.SendMessage("deleteCounter", gameObject);
            Destroy(gameObject);//smierc muhahahahahhahahahahha
        }
        else
        {
            turnOffHpPoints(attackValue);
            hpPoints -= attackValue;
        }
    }

    void fillHpList()
    {
        for(int i = 0; i < 5; i++)
        {
            hpBarsList.Add(hpBar.transform.GetChild(i).GetComponent<SpriteRenderer>());
            hpBarsList2.Add(hpBar.transform.GetChild(i).GetChild(0).GetComponent<SpriteRenderer>());
        }

        for(int i = 4; i >= hpPoints; i--)
        {
            hpBarsList[i].enabled = false;
            hpBarsList2[i].enabled = false;
        }
    }

    void turnOffHpPoints(int howMany)
    {
        
        for(int i = hpPoints-1;i != hpPoints - howMany-1; i--)
        {
            hpBarsList[i].enabled = false;
            hpBarsList2[i].enabled = false;
        }
    }
}
