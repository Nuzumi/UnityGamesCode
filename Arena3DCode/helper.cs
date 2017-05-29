using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class helper {

	public static Vector3 Vector(Vector3 start, Vector3 end)
    {
        return new Vector3(end.x - start.x, end.y - start.y, end.z - start.z);
    }

    public static float VectorLength(Vector3 vector)
    {
        return Mathf.Sqrt(Mathf.Pow(vector.x, 2) + Mathf.Pow(vector.y, 2) + Mathf.Pow(vector.z, 2));
    }

    public static Vector3 Versor(Vector3 vector)
    {
        return vector / VectorLength(vector);
    }
}
