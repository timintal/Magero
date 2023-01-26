using UnityEngine;

public static class HelperFunctions
{
    public static int PackedIndex(int x, int y)
    {
        int plainIndex = x;
        plainIndex <<= 16;
        plainIndex += y;
        return plainIndex;
    }

}

public static class VectorExtentions 
{
    public static float ManhattanDistance(this Vector3 v)
    {
        return Mathf.Abs(v.x) + Mathf.Abs(v.y) + Mathf.Abs(v.z);
    }
}
