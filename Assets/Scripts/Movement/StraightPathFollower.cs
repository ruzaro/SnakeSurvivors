using UnityEngine;

namespace SnakeSurvivors
{
    public class StraightPathFollower : MonoBehaviour
    {
        [SerializeField] private float speed = 1.0f;
        
        public void OnUpdate(float deltaTime)
        {
            var dir = transform.up;
            
            var pos = transform.position;
            pos += dir * speed * deltaTime;
            transform.position = pos;
        }
    }
}