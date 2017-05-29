using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nodeControler : MonoBehaviour {

    public GameObject plane;
    public GameObject node;

    private List<node> nodeList;
    private int planeScale;


    private void Start()
    {
        nodeList = new List<node>();
        planeScale = (int)plane.transform.localScale.x;
        spawnNodes();
    }

    void spawnNodes()
    {
        
        for(int i =planeScale *-5; i <= planeScale*5; i++)
        {
            for(int j = planeScale*-5; j <= planeScale *5 ; j++)
            {
                node node = new node(i, j);
                nodeList.Add(node);
            }
        }
    }


}
