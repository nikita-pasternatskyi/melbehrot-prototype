using UnityEngine;

namespace MP.Game
{
    public class Projectile : MonoBehaviour
    {
        public float Speed = 5;
        public float Lifetime = 10;
        public bool MoveFromAwake = false;
        private bool _isMoving;
        private float _timer;
        private Vector3 _direction;

        private void Awake()
        {
            if (MoveFromAwake)
                _isMoving = true;
        }

        public void StartMoving()
        {
            StartMoving(transform.forward);
        }
        public void StartMoving(Vector3 direction)
        {
            _direction = direction;
            _timer = Lifetime;
            _isMoving = true;
        }

        private void Update()
        {
            if (!_isMoving)
                return;
            _timer -= Time.deltaTime;
            transform.position += _direction.normalized * Speed * Time.deltaTime;
            if (_timer <= 0)
            {
                gameObject.SetActive(false);
                _isMoving = false;
            }
        }
    }
}
