using UnityEngine;

namespace SnakeSurvivors
{
    public static class Extensions
    {
        public static Vector3 ToVec3(this Vector2 vec, float z = 0.0f) => new(vec.x, vec.y, z);
    }
}