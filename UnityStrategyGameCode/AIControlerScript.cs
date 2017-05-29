using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIControlerScript : MonoBehaviour {

    public GameObject battleControler;

    private List<GameObject> counterToManageList;
    private List<GameObject> enemyCounterList;
    private List<GameObject> countersToUseThisTurn;
    private List<GameObject> viablePlatesToGo;

    private void Start()
    {
        viablePlatesToGo = new List<GameObject>();
        countersToUseThisTurn = new List<GameObject>();
        counterToManageList = battleControler.GetComponent<BattleControlerScript>().otherSiteCounter;
        enemyCounterList = battleControler.GetComponent<BattleControlerScript>().playerSiteCounter;
    }

    void yourTurn()
    {
        for(int i = 0; i < countersToUseThisTurn.Capacity;)
        {
            makeRandomMove();
        }
    }

    void makeRandomMove()
    {
        viablePlatesToGo.Clear();
        Debug.Log(countersToUseThisTurn.Count);
        GameObject counter = countersToUseThisTurn[0];
        countersToUseThisTurn.RemoveAt(0);
        int countersMove = counter.GetComponent<CounterMovementScript>().movesPerTurn;

        for(int i = 0; i < countersMove; i++)
        {
            GameObject countersPlate = counter.GetComponent<CounterMovementScript>().plate;
            List<GameObject> neighbourPlate = countersPlate.GetComponent<NodeScript>().neighbourList;

            foreach (GameObject elem in neighbourPlate)
            {
                if (elem.GetComponent<NodeScript>().canGo())
                {
                    viablePlatesToGo.Add(elem);
                }
            }
            battleControler.SendMessage("setActivCounter", counter);
            if (viablePlatesToGo.Capacity > 0)
            {
                Debug.Log("OnMouseDown");
                viablePlatesToGo[Random.Range((int)0, viablePlatesToGo.Capacity)].SendMessage("OnMouseDown");
            }
            else
            {
                Debug.Log("nie ma gdzie isc");
                battleControler.SendMessage("setActivCounter", counter);
            }
        }
    }

    private void turnReset()
    {
        GameObject counter;
        countersToUseThisTurn.Clear();
        countersToUseThisTurn.Add(counterToManageList[Random.Range(0, counterToManageList.Count -1)]);
        while(countersToUseThisTurn.Count != 2)
        {
            counter = counterToManageList[Random.Range(0, counterToManageList.Count - 1)];
            if(counter != countersToUseThisTurn[0])
            {
                countersToUseThisTurn.Add(counter);
            }
        }

        battleControler.SendMessage("goOnAi");
    }

    
}
