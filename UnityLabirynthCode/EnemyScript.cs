using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour {

    public float minJump;
    public float maxJump;
    public float minForceValue;
    public float maxForceValue;
    public List<GameObject> pack;

    private float lastJumpToime;
    private Rigidbody rb;
    private GameObject player=null;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

    }

    private void Update()
    {
        if (player != null)
        {
            if(Time.time > lastJumpToime)
            {
                lastJumpToime += Random.Range(minJump, maxJump);
                rb.AddForce(getVectorToPlayer() * Random.Range(minForceValue,maxForceValue));
            }
        }
    }

    void OnParticleCollision(GameObject other)
    {
        Debug.Log("hit");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            player = other.gameObject;
            lastJumpToime = Time.timeSinceLevelLoad;
            foreach(GameObject elem in pack)
            {
                if (elem != null)
                {
                    elem.SendMessage("setPlayer", other.gameObject);
                }
            }
        }
    }

    private Vector3 getVectorToPlayer()
    {
        Vector3 tmp = new Vector3(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y, player.transform.position.z - transform.position.z);
        float length = Mathf.Sqrt(Mathf.Pow(tmp.x, 2) + Mathf.Pow(tmp.y, 2) + Mathf.Pow(tmp.z, 2));
        return tmp*(1/length);
    }

    private void setPlayer(GameObject playerToAttack)
    {
        if (player == null)
        {
            lastJumpToime = Time.timeSinceLevelLoad;
            player = playerToAttack;
        }
    }
}
