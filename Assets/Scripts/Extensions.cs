using UnityEngine;

namespace SnakeSurvivors
{
    public static class Extensions
    {
        public static Vector3 ToVec3(this Vector2 vec) => new(vec.x, vec.y, 0);
    }
}