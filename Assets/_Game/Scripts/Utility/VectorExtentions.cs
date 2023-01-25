using UnityEngine;

public static class VectorExtentions 
{
    public static float ManhattanDistance(this Vector3 v)
    {
        return Mathf.Abs(v.x) + Mathf.Abs(v.y) + Mathf.Abs(v.z);
    }
}
