using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HP : MonoBehaviour {

    public int startHpPoints;
    public Image hpBar;
    public bool enemy;

    private int hpPoints;

    private void Start()
    {
        hpPoints = startHpPoints;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!enemy)
        {
            if (collision.gameObject.tag == "hazardFP")
            {
                Debug.Log("hazardFP");
                hpPoints -= collision.gameObject.GetComponent<hazard>().damage;
                Debug.Log(hpPoints);
                if (hpPoints <= 0)
                {
                    Destroy(collision.gameObject);
                }

                if (hpBar != null)
                {
                    hpBarActualization();
                }
            }
        }
        else
        {
            if (collision.gameObject.tag == "hazardFE")
            {
                Debug.Log("hazardFR");
                hpPoints -= collision.gameObject.GetComponent<hazard>().damage;
                Debug.Log(hpPoints);
                if (hpPoints <= 0)
                {
                    Destroy(collision.gameObject);
                }

                if (hpBar != null)
                {
                    hpBarActualization();
                }
            }
        }

        if (collision.gameObject.tag == "hazard")
        {
            Debug.Log("hazard");
            hpPoints -= collision.gameObject.GetComponent<hazard>().damage;
            Debug.Log(hpPoints);
            if (hpPoints <= 0)
            {
                Destroy(collision.gameObject);
            }

            if (hpBar != null)
            {
                hpBarActualization();
            }
        }
    }

    void hpBarActualization()
    {
        Debug.Log("akt");
        float hpProcent =  (startHpPoints - hpPoints) / startHpPoints;
        hpBar.transform.localScale -= new Vector3(hpProcent / 100, 0, 0);
        hpBar.transform.position -= new Vector3(hpProcent / 2, 0, 0);
    }
}
