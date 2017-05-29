using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameCanvasScript : MonoBehaviour {

    public Image movementsPointPreFab;
    public GameObject battleControler;

    private int counterMovesPerTurn;
    private Image colorPlate;


    private void Start()
    {
        counterMovesPerTurn = battleControler.GetComponent<BattleControlerScript>().viableCounterPerTurn;
        colorPlate = movementsPointPreFab.transform.GetChild(0).gameObject.GetComponent<Image>();

    }

    void startSpown()
    {

    }

}
