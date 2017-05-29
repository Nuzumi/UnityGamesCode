using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarriorAttackScript : MonoBehaviour {

    public int attatcPerTurn;
    public Canvas buttonCanvas;

    private GameObject plate;
    private int plateType;
    private int attackCounter;
    private int hpPoints;
    private bool isAtackOn;

    private void Start()
    {
        hpPoints = 5;
        isAtackOn = false;
        attackCounter = 0;
        attatcPerTurn = 1000;
    }

    private void OnMouseDown()
    {
        if (attackCounter < attatcPerTurn)
        {
            if (!isAtackOn)
            {
                plate.SendMessage("findEnemyInNeighbour");
                isAtackOn = true;

                if (plateType == 2)
                {
                    buttonCanvas.enabled = true;
                }
            }
            else
            {
                isAtackOn = false;

                if (plateType == 2)
                {
                    buttonCanvas.enabled = false;
                }
            }

        }
    }

    void setPlateToGo(GameObject plate)
    {
        buttonCanvas.enabled = false;
        if(plateType == 1)
        {
            this.plate.SendMessage("makeTauntOff");
        }
        this.plate = plate;
        plateType = plate.GetComponent<NodeScript>().plateType;
        if (plateType == 1)
        {
            if (tag == "Player")
            {
                plate.SendMessage("makeTauntOn", true);
            }
            else
            {
                plate.SendMessage("makeTauntOn", false);
            }

        }
        isAtackOn = false;
    }

    void attackOnPlate(GameObject plateToAttack)
    {
        attackCounter++;
        plate.SendMessage("makeNeighbourNotViableToGo");
        plateToAttack.SendMessage("makeAttack", attackValue());
        isAtackOn = false;
    }

    int attackValue()
    {
        return 2;
    }

    void turnReset()
    {
        attackCounter = 0;
        isAtackOn = false;
    }

    void oAttack()
    {
        if (attackCounter < attatcPerTurn)
        {
            attackCounter++;
            plate.SendMessage("makeNeighbourNotViableToGo");
            plate.SendMessage("oAttack", attackValue());
            Debug.Log("oAttack");
        }
        else
        {
            Debug.Log("nie mozna");
        }
    }
}
