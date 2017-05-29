using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeScript : MonoBehaviour
{

    public List<GameObject> neighbourList;
    public int plateType;
    public bool buildingOn;
    public bool mageAttack; //pomyslec nad zmiana

    private GameObject wall;
    private GameObject battleControl;
    private GameObject counter;
    private GameObject building;
    private SpriteRenderer colorPlate;
    private Collider plateCollider;
    private Collider stopCollider;
    private Color startColor;
    private Color canGoColor;
    private int counterType;
    private bool isTakenByFriend;
    private bool isTakenByEnemy;
    private bool playerTaunt;
    private bool enemyTaunt;
    private bool canBuild;
    private bool isBuilding;

    private void Start()
    {
        mageAttack = false;
        wall = null;
        building = null;
        counter = null;
        colorPlate = transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();
        Collider[] tmpTab = GetComponents<Collider>();
        plateCollider = tmpTab[0];
        stopCollider = tmpTab[1];
        startColor = colorPlate.color;
        canGoColor = Color.green;
        plateCollider.enabled = false;
        stopCollider.enabled = false;
        isTakenByFriend = false;
        isTakenByEnemy = false;
        playerTaunt = false;
        enemyTaunt = false;
        counterType = -1;
        battleControl = GameObject.FindGameObjectWithTag("BattleControler");
        canBuild = true;
        isBuilding = false;
        buildingOn = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.SendMessage("stopMoving", gameObject);
            isTakenByFriend = true;
            stopCollider.enabled = false;
        }
        else
        {
            if (other.gameObject.tag == "Enemy")
            {
                other.SendMessage("stopMoving", gameObject);
                isTakenByEnemy = true;
                stopCollider.enabled = false;
            }
        }

        if (buildingOn)
        {
            other.SendMessage("moveUp", plateType);
        }
    }

    private void OnMouseDown()
    {

        if (mageAttack)
        {
            battleControl.SendMessage("setPlateToAttack", gameObject);
            mageAttack = false;
        }
        else
        {
            if (!isTakenByFriend && !isTakenByEnemy)
            {
                if (wall == null)
                {
                    battleControl.SendMessage("setClickedPlate", gameObject);
                    stopCollider.enabled = true;
                }
            }
            else
            {
                if (isBuilding)
                {
                    canBuild = false;
                    battleControl.SendMessage("setBuildingPlate", gameObject);
                    isBuilding = false;
                    buildingOn = true;
                }
                else
                {
                    battleControl.SendMessage("setPlateToAttack", gameObject);
                }
            }
        }
    }

    void makeViableToGo()
    {
        if (!isTakenByFriend && !isTakenByEnemy && wall == null)
        {
            colorPlate.color = canGoColor;
            plateCollider.enabled = true;
        }
    }

    void makeNotViableToGo()
    {
        colorPlate.color = startColor;
        plateCollider.enabled = false;
    }

    void makeNeighbourViableToGo()
    {
        bool flag = true;

        if (counter != null)
        {
            if (counter.tag == "Player")
            {
                flag = true;
            }
            else
            {
                flag = false;
            }
        }

        if (!doesNeighbourHasTaunt(flag))
        {
            foreach (GameObject elem in neighbourList)
            {
                elem.SendMessage("makeViableToGo");
            }
        }
    }

    void makeNeighbourNotViableToGo()
    {
        foreach (GameObject elem in neighbourList)
        {
            elem.GetComponent<NodeScript>().makeNotViableToGo();
        }
    }

    void freePlate()
    {
        counter = null;
        isTakenByFriend = false;
        isTakenByEnemy = false;
        playerTaunt = false;
        enemyTaunt = false;
    }

    void findEnemyInNeighbour()
    {
        foreach (GameObject elem in neighbourList)
        {
            elem.GetComponent<NodeScript>().findEnemy();
        }
    }

    void findEnemy()
    {
        if (isTakenByEnemy || isTakenByFriend || mageAttack)
        {
            colorPlate.color = Color.red;
            plateCollider.enabled = true;
        }
    }

    void setCounter(GameObject counterToGet)
    {
        counter = counterToGet;
    }

    void makeAttack(int attackValue)
    {
        if (counter != null)
        {
            counter.SendMessage("receiveAttack", attackValue);
        }
    }

    public GameObject getCounter()
    {
        return counter;
    }

    void makeTauntOn(bool isPlayer)
    {
        if (isPlayer)
        {
            playerTaunt = true;
        }
        else
        {
            enemyTaunt = true;
        }
    }

    void makeTauntOff()
    {
        playerTaunt = false;
        enemyTaunt = false;
    }

    bool doesNeighbourHasTaunt(bool isPlayer)
    {
        foreach (GameObject elem in neighbourList)
        {
            if (elem.GetComponent<NodeScript>().hasTaunt(isPlayer))
            {
                return true;
            }
        }

        return false;
    }

    void builidingPlate()
    {
        if (canBuild)
        {
            colorPlate.color = Color.blue;
            plateCollider.enabled = true;
            isBuilding = true;
        }
    }

    public bool hasTaunt(bool isPlayer)
    {
        if (isPlayer)
        {
            if (enemyTaunt)
            {
                return true;
            }
        }
        else
        {
            if (playerTaunt)
            {
                return true;
            }
        }

        return false;
    }

    void setBuilding(GameObject build)
    {
        building = build;
    }

    void makeWall(GameObject newWall)
    {
        wall = newWall;
    }

    void destroyEverything()
    {
        if (wall != null)
        {
            Destroy(wall);
            wall = null;
        }

        if (building != null)
        {
            Destroy(building);
            building = null;
        }
    }

    void oAttack(int attackValue)
    {
        foreach(GameObject elem in neighbourList)
        {
            elem.SendMessage("oAttackMake",attackValue);
        }
    }

    void oAttackMake(int attackValue)
    {
        if (counter != null)
        {
            counter.SendMessage("receiveAttack", attackValue);
        }
    }

    public bool canGo()
    {
        if (doesNeighbourHasTaunt(false))
        {
            if (!isTakenByFriend && !isTakenByEnemy && wall == null)
            {
                return true;
            }
        }

        return false;
    }

}
