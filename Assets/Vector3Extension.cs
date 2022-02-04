using UnityEngine;

public static class Vector3Extension
{
    public static float VectorComponent(Vector3 vector, Vector3 along)
    {
        return Vector3.Dot(vector, along.normalized);
    }
}