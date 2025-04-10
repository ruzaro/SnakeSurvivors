using UnityEngine;

namespace SnakeSurvivors
{
    public static class Extensions
    {
        public static Vector3 ToVec3(this Vector2 vec, float z = 0.0f) => new(vec.x, vec.y, z);
        
        public static bool IsPlayer(this GameObject gameObject) => gameObject.CompareTag(Const.Tags.Player);
        public static bool IsEnemy(this GameObject gameObject) => gameObject.CompareTag(Const.Tags.Enemy);
        public static bool IsProjectile(this GameObject gameObject) => gameObject.CompareTag(Const.Tags.Projectile);
        
        public static bool IsPlayer(this Component gameObject) => gameObject.CompareTag(Const.Tags.Player);
        public static bool IsEnemy(this Component gameObject) => gameObject.CompareTag(Const.Tags.Enemy);
        public static bool IsProjectile(this Component gameObject) => gameObject.CompareTag(Const.Tags.Projectile);
    }
}