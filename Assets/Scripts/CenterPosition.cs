using System.Collections.Generic;
using UnityEngine;

namespace SnakeSurvivors
{
    public class CenterPosition
    {
        private readonly List<(Transform, float)> _targets = new();

        public Vector2 CentralPosition
        {
            get
            {
                if (_targets.Count == 0) return Vector3.zero;
                
                var sum = Vector2.zero;
                var weightSum = 0.0f;

                foreach (var (t, w) in _targets)
                {
                    sum += (Vector2)t.position * w;
                    weightSum += w;
                }

                return sum / weightSum;
            }
        }

        public void AddTarget(Transform target, float weight = 1.0f)
        {
            _targets.Add((target, weight));
        }

        public void RemoveTarget(Transform target)
        {
            _targets.RemoveAll(t => t.Item1 == target);
        }
    }
}