using UnityEngine;
using UnityEngine.UI;

namespace SnakeSurvivors
{
    public class BackgroundController : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private RawImage background;
        [SerializeField] private float speed = 0.1f;

        private Transform _transform;
        private Vector2 _posDelta;

        private Vector2 _prevPos;

        private void Awake()
        {
            _transform = transform;
            _prevPos = _transform.position;
        }

        public void SetPosDelta(Vector2 posDelta)
        {
            _posDelta = posDelta;
        }
        
        private void Update()
        {
            Vector2 pos = _transform.position;

            var diff = pos - _prevPos;
            
            background.uvRect = new Rect(background.uvRect.position + diff * speed, background.uvRect.size);

            _prevPos = pos;
            
            // background.uvRect = new Rect(background.uvRect.position + _posDelta * speed, background.uvRect.size);
            _posDelta = Vector2.zero;

            _transform.position = target.position;
            
            // background.uvRect = new Rect(target.position * speed, background.uvRect.size);
        }
    }
}