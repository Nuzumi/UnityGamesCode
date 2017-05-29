using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapControler : MonoBehaviour {

    public int seed;
    public GameObject mapGenerator50X100;
    public GameObject mapGenerator100X50;

    private System.Random rand;
    private List<List<List<int []>>> mapPlan;

    private void Start()
    {
        mapPlan = new List<List<List<int[]>>>();
        rand = new System.Random(seed);
        makePlan();
    }

    private void makePlan()
    {
        for(int layer = 0; layer < 100; layer++)//5 mozna zmienic na wiekszy
        {
            int layerNumber = layer;
            mapPlan.Add(new List<List<int[]>>());
            for(int wall = 0; wall < 4; wall++)//4tu zostaje
            {
                int wallNumber = wall;
                mapPlan[layer].Add(new List<int[]>());
                for (int brick = 0; brick < layer + 1; brick++)//0-width 1-height 2-seed  3-possitionX 4-possitionZ 5- isBuild(0-false|1-true)
                {
                    int[] klocek = new int[6];
                    if (layer > 0)
                    {
                        klocek[5] = 0;
                    }
                    else
                    {
                        klocek[5] = 1;
                    }
                    if (wallNumber % 2 == 0)
                    {
                        klocek[0] = 100;
                        klocek[1] = 50;
                    }
                    else
                    {
                        klocek[0] = 50;
                        klocek[1] = 100;
                    }
                    klocek[2] = rand.Next(10000000);
                    switch (wallNumber)
                    {
                        case 0:
                            klocek[3] = -25 - layer * 50 + brick * 100;
                            klocek[4] = (layer + 1) * 50;
                            break;
                        case 1:
                            klocek[3] = (layer + 1) * 50;
                            klocek[4] = 25 + layer * 50 - brick * 100;
                            break;
                        case 2:
                            klocek[3] = 25 - layer * 50 + brick * 100;
                            klocek[4] = -50 * (layer + 1);
                            break;
                        default:
                            klocek[3] = -50 * (layer + 1);
                            klocek[4] = -25 + 50 * layer - 100 * brick;
                            break;
                    }

                    mapPlan[layer][wall].Add(klocek);

                }
            }
        }
    }

    void showPlan()
    {
        foreach(List<List<int []>> elem in mapPlan)
        {
            foreach(List<int[]> elem2 in elem)
            {
                foreach(int [] elem3 in elem2)
                {
                    
                }
            }
        }
    }

    void crossed(Vector3 pos)
    {
        Debug.Log("1");
        Vector2 tilePos = new Vector2(pos.x, pos.z);
        int[] val = calculateBrick(tilePos);// layer wall brick
        Debug.Log("1,2");
        Debug.Log(val[0] +" " +val[1] +" "+val[2]);
        int[] brick = mapPlan[val[0]][val[1]][val[2]];
        Debug.Log("1,5");
        List<int[]> brickListToCheck = getNearBricks(val);
        Debug.Log(brickListToCheck.Count + " ilosc kafelek");
        foreach(int[] elem in brickListToCheck)
        {
            Debug.Log("2");
            int [] tile = mapPlan[elem[0]][elem[1]][elem[2]];
            if (tile[5] == 0)
            {
                Debug.Log("3");
                if (tile[0] == 100)
                {
                    tile[5] = 1;
                    Debug.Log("4");
                    Instantiate(mapGenerator100X50, new Vector3(tile[3], 0, tile[4]), Quaternion.identity).SendMessage("getValuesAndMakeMap", tile[2]);
                }
                else
                {
                    tile[5] = 1;
                    Debug.Log("5");
                    Instantiate(mapGenerator50X100, new Vector3(tile[3], 0, tile[4]), Quaternion.identity).SendMessage("getValuesAndMakeMap", tile[2]);
                }
            }
        }
    }

    int[] calculateBrick(Vector2 tilePos)
    {
        int x = (int)tilePos.x;
        int y = (int)tilePos.y;
        int wall;
        int layer;
        int brick;
        if (y % 50 == 0)
        {
            if (y > 0)
            {
                wall = 0;
            }
            else
            {
                wall = 2;
            }
            layer = Mathf.Abs(y / 50) - 1;
        }
        else
        {
            if (x > 0)
            {
                wall = 1;
            }
            else
            {
                wall = 3;
            }
            layer = Mathf.Abs(x / 50) - 1;
        }

        switch (wall)
        {
            case 0:
                brick = (x + 25 + layer * 50) / 100;
                break;

            case 1:
                brick = (y - 25 - layer * 50) / -100;
                break;

            case 2:
                brick = (x - 25 + layer * 50) / 100;
                break;
            case 3:
                brick = (y + 25 - 50 * layer) / -100;
                break;
            default:
                brick = -1;
                Debug.Log("jakis blad wall>3");
                break;
        }

        int[] res = new int[3];
        res[0] = layer;
        res[1] = wall;
        res[2] = brick;
        return res;
    }
    
    List<int[]> getNearBricks(int[] brickPosition)//narazie tylko z przodu
    {
        List<int[]> brickList = new List<int[]>();
        int bricksInTheWall = brickPosition[0] + 1;
        int[] brickToAdd = { brickPosition[0] + 1, brickPosition[1], brickPosition[2] };
        brickList.Add(brickToAdd);
        int[] b2 = { brickPosition[0] + 1, brickPosition[1], brickPosition[2] +1};
        brickList.Add(b2);

        return brickList;
    }

}
