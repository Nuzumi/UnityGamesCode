using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum counter { wojownik , lucznik , mag ,inzynier};
enum plateType { trojkat , kwadrat ,szescian};

public class FunctionHelperScript  {

	public static Vector3 makeVersor(Vector3 startPoint , Vector3 endPoint)
    {
        float length = vectorLength(startPoint, endPoint);
        return new Vector3(endPoint.x - startPoint.x, endPoint.y - startPoint.y, endPoint.z - startPoint.z)/length;
    }

    public static Vector3 makeVector(Vector3 startPoint, Vector3 endPoint)
    {
        return new Vector3(endPoint.x - startPoint.x, endPoint.y - startPoint.y, endPoint.z - startPoint.z);
    }

    public static float vectorLength(Vector3 start,Vector3 end)
    {
        return Mathf.Sqrt(Mathf.Pow(start.x - end.x, 2) + Mathf.Pow(start.y - end.y, 2) + Mathf.Pow(start.z - end.z, 2));
    }

    public static float vectorLength(Vector3 vec)
    {
        return Mathf.Sqrt(Mathf.Pow(vec.x, 2) + Mathf.Pow(vec.y, 2) + Mathf.Pow(vec.z, 2));
    }

    public static List<GameObject> mergeList(List<GameObject> one , List<GameObject> secound)
    {
        foreach(GameObject elem in secound)
        {
            one.Add(elem);
        }
        return one;
    }

    public static int random0To100()
    {
        return Random.Range(0, 101);
    }
}
