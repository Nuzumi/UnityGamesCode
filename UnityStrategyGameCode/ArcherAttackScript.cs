using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherAttackScript : MonoBehaviour {

    public GameObject arrow;
    public GameObject battleControler;
    public float attackRange;
    public int attackPerTurn;
    public int attackValue;
    public int critChance;

    private List<GameObject> enemyCounterList;
    private List<GameObject> enemyInRange;
    private List<GameObject> enemysPlateInRange;
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
        foreach(GameObject elem in enemyCounterList)
        {
            if (FunctionHelperScript.vectorLength(transform.position, elem.transform.position) <= attackRange)
            {
                enemyInRange.Add(elem);
                enemysPlateInRange.Add(elem.GetComponent<CounterMovementScript>().getPlate());
            }
        }
    }

    void prepareAttack()
    {
        foreach(GameObject elem in enemysPlateInRange)
        {
            elem.SendMessage("findEnemy");
        }
    }

    IEnumerator attackOnPlate(GameObject plateToAttack)
    {
        attackCounter++;
        turnOffPlates();
        float timeToWait = fireArrow(plateToAttack);
        plate.SendMessage("makeNeighbourNotViableToGo");
        yield return new WaitForSecondsRealtime(timeToWait-0.6f);
        plateToAttack.SendMessage("makeAttack", makeAttackValue());
        OnMouseDown();
    }

    void turnOffPlates()
    {
        foreach (GameObject elem in enemysPlateInRange)
        {
            elem.SendMessage("makeNotViableToGo");
        }
    }

    void turnReset()
    {
        attackCounter = 0;
        isAttackOn = false;
    }

    float fireArrow(GameObject plateToAttack)
    {
        Vector3 forceVersor = FunctionHelperScript.makeVersor(transform.position, plateToAttack.transform.position);
        forceVersor = new Vector3(forceVersor.x, forceVersor.y + 1, forceVersor.z);
        forceVersor = forceVersor / FunctionHelperScript.vectorLength(forceVersor);

        float forceToAdd;
        if (onBuilding)
        {
            float bHeight;
            if (plateType == 2)
            {
                bHeight = 0.8f;
            }
            else
            {
                bHeight = 0.53f;
            }
            float length = FunctionHelperScript.vectorLength(new Vector3(transform.position.x, 0, transform.position.z), plateToAttack.transform.position);
            forceToAdd = (9.81f * Mathf.Pow(length, 2)) / (length + bHeight);
            forceToAdd = Mathf.Sqrt(forceToAdd);
        }
        else
        {
            forceToAdd = 3.132f * Mathf.Sqrt(FunctionHelperScript.vectorLength(transform.position, plateToAttack.transform.position));
        }


        GameObject arrowCopy =  Instantiate(arrow, transform.position,Quaternion.identity);
        Rigidbody arrowRb = arrowCopy.GetComponent<Rigidbody>();
        arrowRb.AddForce(forceVersor * forceToAdd*10);
        float timeToDestroyArrow = (2 * forceToAdd )/ 9.81f;
        Destroy(arrowCopy, timeToDestroyArrow-0.1f);
        return timeToDestroyArrow;
    }

    int makeAttackValue()
    {
        if (plateType == 0)
        {
            if (FunctionHelperScript.random0To100() > critChance)
            {
                return attackValue;
            }
            else
            {
                Debug.Log("crit");
                return attackValue * 2;
            }
            
        }
        else
        {
            if(plateType == 1)
            {
                if (FunctionHelperScript.random0To100() > critChance*2.5)
                {
                    return attackValue;
                }
                else
                {
                    Debug.Log("crit");
                    return attackValue * 2;
                }
            }
            else
            {
                attackCounter--;
                return attackValue;
            }
        }
    }
}
