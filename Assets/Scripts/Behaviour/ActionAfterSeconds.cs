using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace SnakeSurvivors
{
    public class ActionAfterSeconds : MonoBehaviour
    {
        [SerializeField] private float actionTimer = 5.0f;
        [SerializeField] private bool restartOnEnable;
        [SerializeField] private UnityEvent<GameObject> action;
        public UnityEvent<GameObject> Action => action;
        
        private Coroutine _actionCoroutine;

        private void Start()
        {
            _actionCoroutine = StartCoroutine(ActionAfterTime());
        }

        private void OnEnable()
        {
            if (!restartOnEnable) return;

            _actionCoroutine = StartCoroutine(ActionAfterTime());
        }

        private void OnDisable()
        {
            StopCoroutine(_actionCoroutine);
            _actionCoroutine = null;  
        }

        private IEnumerator ActionAfterTime()
        {
            yield return new WaitForSeconds(actionTimer);
            
            action?.Invoke(gameObject);
        }
    }
}