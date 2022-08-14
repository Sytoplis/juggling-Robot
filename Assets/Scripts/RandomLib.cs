using UnityEngine;

public static class RandomLib
{
    public static Vector3 randomVec(Vector3 min, Vector3 max) {
        return new Vector3(Random.Range(min.x, max.x),
                           Random.Range(min.y, max.y),
                           Random.Range(min.z, max.z));
    }

    public static Vector3 randomVec(Bounds bounds) { return randomVec(bounds.min, bounds.max); }
}
