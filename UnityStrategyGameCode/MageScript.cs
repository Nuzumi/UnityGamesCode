using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageScript : MonoBehaviour
{
    public GameObject wallHexagon;
    public GameObject wallSquare;
    public GameObject wallTriangle;
    public GameObject graphControler;
    public GameObject battleControler;
    public float attackRangeFireBall;
    public float attackRangeSnare;
    public float attackRangeSlow;
    public float attackRangeMeteor;
    public float attackRangeWall;
    public float attackRangeIceSpike;
    public int attackPerTurn;
    public float timeToWait;

    private List<GameObject> enemyCounterList;
    private List<GameObject> enemyInRange;
    private List<GameObject> enemysPlateInRange;
    private List<GameObject> platesList;
    private List<GameObject> platesInRange;
    private GameObject plate;
    private int plateType;
    private bool isAttackOn;
    private int attackCounter;
    private bool onBuilding;


    private void Start()
    {
        onBuilding = false;
        isAttackOn = false;
        attackCounter = 0;
        enemyCounterList = battleControler.GetComponent<BattleControlerScript>().otherSiteCounter;
        enemyInRange = new List<GameObject>();
        enemysPlateInRange = new List<GameObject>();
        platesInRange = new List<GameObject>();
        platesInRange = new List<GameObject>();
        platesList = graphControler.GetComponent<GraphControlerScript>().setList();
     }

    private void OnMouseDown()
    {
        if (attackCounter < attackPerTurn)
        {
            if (!isAttackOn)
            {
                fillInRangeList();
                prepareAttack();
                isAttackOn = true;
            }
            else
            {
                turnOffPlates();
                isAttackOn = false;
            }
        }
    }

    void setPlateToGo(GameObject plate)
    {
        this.plate = plate;
        plateType = plate.GetComponent<NodeScript>().plateType;
        onBuilding = plate.GetComponent<NodeScript>().buildingOn;
        isAttackOn = false;
        turnOffPlates();
    }

    void fillInRangeList()
    {
        enemyInRange.Clear();
        enemysPlateInRange.Clear();
        platesInRange.Clear();

        float range = attackRange();
        foreach (GameObject elem in enemyCounterList)
        {
            if (FunctionHelperScript.vectorLength(transform.position, elem.transform.position) <= range)
            {
                enemyInRange.Add(elem);
                enemysPlateInRange.Add(elem.GetComponent<CounterMovementScript>().getPlate());
            }
        }

        foreach (GameObject elem in platesList)
        {
            if (FunctionHelperScript.vectorLength(transform.position, elem.transform.position) <= range)
            {
                platesInRange.Add(elem);
            }
        }
    }

    void turnOffPlates()
    {
        foreach (GameObject elem in enemysPlateInRange)
        {
            elem.SendMessage("makeNotViableToGo");
        }

        foreach (GameObject elem in platesInRange)
        {
            elem.SendMessage("makeNotViableToGo");
        }
    }

    void turnReset()
    {
        attackCounter = 0;
        isAttackOn = false;
    }

    void prepareAttack()
    {
        if (onBuilding)
        {
            switch (plateType)
            {
                case 0:
                    foreach (GameObject elem in enemysPlateInRange)
                    {
                        elem.SendMessage("findEnemy");
                    }
                    break;
                default:
                    foreach (GameObject elem in platesInRange)
                    {
                        elem.SendMessage("findEnemy");
                        elem.GetComponent<NodeScript>().mageAttack = true;
                    }
                    break;
            }
        }
        else
        {
            foreach (GameObject elem in enemysPlateInRange)
            {
                elem.SendMessage("findEnemy");
            }
        }
    }

    IEnumerator attackOnPlate(GameObject plateToAttack)
    {
        attackCounter++;
        turnOffPlates();
        castSpell(plateToAttack);
        plate.SendMessage("makeNeighbourNotViableToGo");
        yield return new WaitForSecondsRealtime(timeToWait);
        plateToAttack.SendMessage("makeAttack", makeAttackValue());
        OnMouseDown();
    }

    void castSpell(GameObject plateToAttack)
    {
        if (onBuilding)
        {
            switch (plateType)
            {
                case 0:
                    spellIceSpike(plateToAttack);
                    break;

                case 1:
                    spellWall(plateToAttack);
                    break;

                default:
                    spellMeteor(plateToAttack);
                    break;
            }
        }
        else
        {
            switch (plateType)
            {
                case 0:
                    spellSlow(plateToAttack);
                    break;

                case 1:
                    spellSnare(plateToAttack);
                    break;

                default:
                    spellFireBall(plateToAttack);
                    break;
            }
        }
    }

    int makeAttackValue()
    {
        if (onBuilding)
        {
            if (plateType == 2)
            {
                return 100;
            }
            else
            {
                if(plateType == 1)
                {
                    return 50;
                }
                else
                {
                    return 10;
                }
            }
        }
        else
        {
            if(plateType == 2)
            {
                return 2;
            }
            else
            {
                if(plateType == 1)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }
    }

    void spellFireBall(GameObject plateToAttack)
    {
        Debug.Log("fire ball");
    }

    void spellSnare(GameObject plateToAttack)
    {
        Debug.Log("snare");
        GameObject counterToSnare = plateToAttack.GetComponent<NodeScript>().getCounter();
        counterToSnare.SendMessage("snare");
    }

    void spellSlow(GameObject plateToAttack)
    {
        Debug.Log("slow");
        GameObject counterToSlow = plateToAttack.GetComponent<NodeScript>().getCounter();
        counterToSlow.SendMessage("slow");
    }

    void spellMeteor(GameObject plateToAttack)
    {
        Debug.Log("meteor");
        plateToAttack.SendMessage("destroyEverything");
        foreach (GameObject elem in platesInRange)
        {
            elem.GetComponent<NodeScript>().mageAttack = false;
        }
    }

    void spellWall(GameObject plateToAttack)
    {
        Debug.Log("wall");
        GameObject wall;
        switch (plateToAttack.GetComponent<NodeScript>().plateType)
        {
            case 0:
                wall = wallTriangle;
                break;
            case 1:
                wall = wallSquare;
                break;
            default:
                wall = wallHexagon;
                break;
        }

        wall = Instantiate(wall, plateToAttack.transform.position, plateToAttack.transform.rotation);
        plateToAttack.SendMessage("makeWall", wall);
        wall.SendMessage("getPlate", plateToAttack);
        foreach (GameObject elem in platesInRange)
        {
            elem.GetComponent<NodeScript>().mageAttack = false;
        }
    }

    void spellIceSpike(GameObject plateToAttack)
    {
        Debug.Log("ice spike");
    }

    float attackRange()
    {
        if (onBuilding)
        {
            switch (plateType)
            {
                case 0:
                    return attackRangeIceSpike;

                case 1:
                    return attackRangeWall;

                default:
                    return attackRangeMeteor;
            }
        }
        else
        {
            switch (plateType)
            {
                case 0:
                    return attackRangeSlow;

                case 1:
                    return attackRangeSnare;

                default:
                    return attackRangeFireBall;
            }
        }
    }
}
