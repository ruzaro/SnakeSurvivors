using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace SnakeSurvivors
{
    public class ActionAfterSeconds : MonoBehaviour
    {
        [SerializeField] private float destructionTimer = 5.0f;
        [SerializeField] private UnityEvent<GameObject> action;
        public UnityEvent<GameObject> Action => action;

        private void Start()
        {
            StartCoroutine(DestroyAfterTime());
        }

        private IEnumerator DestroyAfterTime()
        {
            yield return new WaitForSeconds(destructionTimer);
            
            action?.Invoke(gameObject);
        }
    }
}