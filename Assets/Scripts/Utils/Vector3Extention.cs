using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector3Extention
{
    public static Vector3 Random(this Vector3 myVector, Vector3 min, Vector3 max)
    {
        myVector = new Vector3(UnityEngine.Random.Range(min.x, max.x), UnityEngine.Random.Range(min.y, max.y), UnityEngine.Random.Range(min.z, max.z));
        return myVector;
    }
}
