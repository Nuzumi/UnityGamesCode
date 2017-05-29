using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonControlerScript : MonoBehaviour {

    public GameObject BC;
    public Canvas warriorCanva;

    private void Start()
    {
        warriorCanva.enabled = false;
    }

    public void aAttackButtonDown()
    {
        BC.SendMessage("oAttack");
    }
}
