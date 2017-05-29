using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleControlerScript : MonoBehaviour {

    public List<GameObject> playerSiteCounter;
    public List<GameObject> otherSiteCounter;
    public GameObject grapControler;
    public int viableCounterPerTurn;

    private List<GameObject> thisTurnCounter;
    private List<GameObject> allCounterList;
    private GameObject activeCounter;
    private GameObject aiControler;
    private bool playerTurn;

	void Start () {
        playerTurn = true;
        allCounterList = FunctionHelperScript.mergeList(playerSiteCounter, otherSiteCounter);
        activeCounter = null;
        thisTurnCounter = new List<GameObject>();
        aiControler = GameObject.Find("AIControler");
	}
	
	void setActivCounter(GameObject toGet)
    {
        if (toGet != activeCounter)
        {
            if (thisTurnCounter.Count < viableCounterPerTurn || thisTurnCounter.Contains(toGet))
            {
                if (!thisTurnCounter.Contains(toGet))
                {
                    thisTurnCounter.Add(toGet);
                }
                activeCounter = toGet;
                foreach (GameObject elem in allCounterList)
                {
                    if (elem != activeCounter)
                    {
                        elem.SendMessage("turnOffCollider");
                    }
                }
            }
            else
            {
                Debug.Log("nie mozna");
            }
        }
        else
        {
            activeCounter.SendMessage("turnOffPlates");
            activeCounter = null;
            foreach (GameObject elem in allCounterList)
            {
                elem.SendMessage("turnOnCollider");
            }
        }
    }

    void setClickedPlate(GameObject plate)
    {
        activeCounter.SendMessage("setPlateToGo", plate);
        plate.SendMessage("setCounter", activeCounter);
        makeReset();
    }

    void setPlateToAttack(GameObject plat)
    {
        if (activeCounter != null)
        {
            activeCounter.SendMessage("attackOnPlate", plat);
        }
        makeReset();
    }

    void setBuildingPlate(GameObject plate)
    {
        activeCounter.SendMessage("build");
    }

    void makeReset()
    {
        foreach (GameObject elem in allCounterList)
        {
            elem.SendMessage("turnOnCollider");
        }
        activeCounter = null;
        thisTurnCounter.Clear();
    }

    void deleteCounter(GameObject toDelete)
    {
        allCounterList.Remove(toDelete);
        otherSiteCounter.Remove(toDelete);
        playerSiteCounter.Remove(toDelete);
    }

    public void newTurn()
    {
        grapControler.SendMessage("turnReset");
        makeReset();
        foreach(GameObject elem in allCounterList)
        {
            elem.SendMessage("turnReset");
        }
        if (playerTurn)
        {
            playerTurn = false;
            aiControler.SendMessage("turnReset");
        }
        else
        {
            playerTurn = true;
        }
    }

    void oAttack()
    {
        activeCounter.SendMessage("oAttack");
    }

    void goOnAi()
    {
        aiControler.SendMessage("yourTurn");
    }

}
