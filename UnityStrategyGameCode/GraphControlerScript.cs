using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphControlerScript : MonoBehaviour {

    private List<GameObject> nodeList;

    private void Awake()
    {
        nodeList = new List<GameObject>();

        fillList();
    }

    private void turnReset()
    {
        foreach (GameObject elem in nodeList)
        {
            elem.SendMessage("makeNotViableToGo");
        }
    }

    void fillList()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            nodeList.Add(transform.GetChild(i).gameObject);
        }
    }

    public List<GameObject> setList()
    {
        return nodeList;
    }
}
