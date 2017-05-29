using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class getForce : MonoBehaviour {


    private Rigidbody rb;
    private bool isOn;

	void Start () {
        rb = GetComponent<Rigidbody>();
        isOn = false;
	}

    private void OnCollisionEnter(Collision collision)
    {
        if(isOn && collision.gameObject.tag == "ball")
        {
            if (rb != null)
            {
                collision.gameObject.SendMessage("getOtherForce", rb);
            }
        }
    }

    void getOtherForce(Rigidbody otherRb)
    {
        if (rb != null)
        {
            if (rb.mass > 5)
            {
                Vector3 pos = transform.position;
                Vector3 otherPos = otherRb.transform.position;
                Vector3 forceDirection = new Vector3(pos.x - otherPos.x, pos.y - otherPos.y, pos.z - otherPos.z);
                Vector3 forceDirectionVersor = forceDirection * (1 / helper.VectorLength(forceDirection));
                float otherKineticE = kineticEnergy(otherRb);
                rb.velocity = forceDirectionVersor * otherKineticE / 6;
            }
        }
    }

    float kineticEnergy(Rigidbody toGet)
    {
        return (toGet.mass * Mathf.Pow(helper.VectorLength(toGet.velocity), 2)) / 2;
    }

    public void On()
    {
        isOn = true;
    }

    public void Off()
    {
        isOn = false;
    }
}
