using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NodeOfGraphScript : MonoBehaviour {

    public float weightAftherCanBounce;
    public float multiplier;
    public float weight;
    public float minDist;
    public bool predicted;
    public bool isWining = false;
    public bool canBounce = false;
    public bool isShining;
    public bool startShining;
    public GameObject mainControler;
    public List<GameObject> neighbourhoodNoodeList;
    public List<GameObject> bouncingNeighbourList;
    public GameObject previous;

    private bool once;
    private SpriteRenderer canBounceMarker;
    private SpriteRenderer spriteRenderer;
    private Collider2D colide;
    private GameObject actualStartNode;


    void Start () {

        neighbourhoodNoodeList = new List<GameObject>();
        bouncingNeighbourList = new List<GameObject>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        colide = gameObject.GetComponent<Collider2D>();
        canBounceMarker = gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();

        canBounceMarker.enabled = false;
        colide.enabled = false;
        startShining = false;
        isShining = false;
        predicted = false;
        once = false;
        minDist = (float)double.PositiveInfinity;
        randomWeightType();
        packNeiList();
    }

    private void Update()
    {
        if (!once)
        {
            if (canBounce)
            {
                canBounceMarker.enabled = true;
            }
        }
    }


    private void OnMouseDown()
    {
        mainControler.SendMessage("getNewStartNode", gameObject);
    }

    void startShiningFunction(bool makeItShine)
    {
        if (makeItShine && !isShining)
        {
            spriteRenderer.color = new Color(0.21f, 1.0f, 0f);

            isShining = true;
            colide.enabled = true;
        }
        else
            if(!makeItShine && isShining)
        {
            spriteRenderer.color = new Color(1, 1, 1);

            isShining = false;
            colide.enabled = false;
        }
       
    }

    void makeNeighbourShine(bool shining)
    {
        if (shining)
        {
            foreach (GameObject node in neighbourhoodNoodeList)
            {
                node.SendMessage("startShiningFunction", true);
            }
        }
        else
        {
            foreach (GameObject node in neighbourhoodNoodeList)
            {
                node.SendMessage("startShiningFunction", false);
            }
        }       
     }

    void deletePathTo(GameObject to)
    {
        neighbourhoodNoodeList.Remove(to);
    }

    void addPathTo(GameObject toAdd)
    {
        neighbourhoodNoodeList.Add(toAdd);
    } 

    void changeColorTo(Color newColor)
    {
        spriteRenderer.color = newColor;
    }

    void makeItBounce()
    {
        canBounce = true;
        canBounceMarker.enabled = true;
        weight = weightAftherCanBounce;
    }

    void packNeiList()
    {
        GameObject[] tagArray = GameObject.FindGameObjectsWithTag("Node");

        for(int i = 0; i < tagArray.Length; i++)
        {
            if (!canBounce)
            {
                if (distance(tagArray[i].transform.position) < 1.5f && tagArray[i].transform.position != transform.position)
                {
                    neighbourhoodNoodeList.Add(tagArray[i]);
                }
            }
            else
            {
                if((distance(tagArray[i].transform.position) < 1.5f && tagArray[i].transform.position != transform.position 
                    && !tagArray[i].gameObject.GetComponent<NodeOfGraphScript>().canBounce))
                {
                    neighbourhoodNoodeList.Add(tagArray[i]);
                }
            }

        }
    }

    float distance(Vector3 other)
    {
        return (Mathf.Sqrt(Mathf.Pow(transform.position.x - other.x, 2) + Mathf.Pow(transform.position.y - other.y, 2)));
    }

    void turnOfPred(GameObject element)
    {
        bouncingNeighbourList = new List<GameObject>();
        predicted = false;
        previous = null;
        minDist = (float)double.PositiveInfinity;
        fillBouncingNeighbourList(element);
    }

    void turnOnPred()
    {
        predicted = true;
    }

    void fillBouncingNeighbourList(GameObject elementToExclude)
    {
        foreach(GameObject elem in neighbourhoodNoodeList)
        {
            if (elem.GetComponent<NodeOfGraphScript>().canBounce && elem != elementToExclude)
            {
                bouncingNeighbourList.Add(elem);
            }
        }
    }

    void setMinDist(float newDist)
    {
        minDist = newDist;
    }

    void setPrevious(GameObject toSet)
    {
        previous = toSet;
    }

    /*  void goToNode(GameObject nodeToGo)
    {
        neighbourhoodNoodeList.Remove(nodeToGo);

        mainControler.SendMessage("getNewStartNode", nodeToGo);
    }*/

    float weightMake()
    {
        if(transform.position.y == 0)
        {
            return 3.0f;
        }
        else
        {
            return Mathf.Pow(Mathf.Abs(transform.position.y)+2, multiplier);
        }
    } //linie poziome

    float weightMakeRandom()
    {
        return Random.Range(7, 150);
    } // randomowe

    float weightMakeCircle()
    {
        return Mathf.Pow(transform.position.x, 2) + Mathf.Pow(transform.position.y, 2)+multiplier*2;
    } // kola

    float weightLessRandom()
    {
        int width = mainControler.GetComponent<MainGameControlerScript>().width;
        int height = mainControler.GetComponent<MainGameControlerScript>().height;

        if (Mathf.Abs(transform.position.y)<height/4)
        {
            return Random.Range(5, 10)*multiplier;
        }
        else
        {
            if (transform.position.x > 0)
            {
                if (transform.position.x > width / 8)
                {
                    return Random.Range(5, 55)*multiplier;
                }
                else
                {
                    return Random.Range(55, 110)*multiplier;
                }
            }
            else
            {
                if (transform.position.x < width / -8)
                {
                    return Random.Range(110, 165)*multiplier;
                }
                else
                {
                    return Random.Range(165, 210)*multiplier;
                }
            }
        }
    }

    float weightCircularBase()
    {
        int width = mainControler.GetComponent<MainGameControlerScript>().width;
        int height = mainControler.GetComponent<MainGameControlerScript>().height;
        float dist;

        if (transform.position.x > 0)
        {
            if (transform.position.y > 0)
            {
                 dist = distance(new Vector3(width / 4, height / 4, 0));
            }
            else
            {
                 dist = distance(new Vector3(width / 4, height / -4, 0));
            }

        }
        else
        {
            if (transform.position.y > 0)
            {
                 dist = distance(new Vector3(width / -4, height / 4, 0));
            }
            else
            {
                 dist = distance(new Vector3(width / -4, height / -4, 0));
            }
        }

        return Mathf.Abs(1000 - Mathf.Pow(dist + 5, 3));
    }

    void randomWeightType()
    {
        int type;

        if (GameObject.FindGameObjectsWithTag("InfoObject")[0].GetComponent<InfoObjectScript>().weightCase == 0)
        {
            type = Random.Range(4, 6);
        }
        else
        {
            type = Random.Range(1, 4);
        }

        switch (type)
        {
            case 1:
                weight = weightCircularBase();
                break;

            case 2:
                weight = weightMakeCircle();
                break;

            case 3:
                weight = weightMake();
                break;

            case 4:
                weight = weightLessRandom();
                break;

            default:
                weight = weightMakeRandom();
                break;
        }
    }
}
