using UnityEngine;

namespace MP.Game
{
    public class CameraCombat : MonoBehaviour
    {
        [Header("Animation")]
        [SerializeField] private Animator _animator;
        [SerializeField] private string _directionParameterXName;
        [SerializeField] private string _directionParameterYName;
        [SerializeField] private string _combatParameterName;
        [SerializeField] private string _normalParameterName;

        [Header("Gameplay")]
        [SerializeField] private Transform _arm;
        [SerializeField] private Transform _sword;
        [SerializeField] private Camera _camera;
        [SerializeField] private LayerMask _hittable;
        [SerializeField] private float _sphereCastRadius;
        [SerializeField] private float _sphereCastLength;
        [SerializeField] private float _delayBeforeReturn;
        [SerializeField] private float _delayBeforeCanAttackAgain;
        private Vector2 _mouseOffset;
        private Quaternion rot;

        private float _timer;
        private bool _isFighting = false;

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            var length = _sphereCastLength;
            if (Hit(out var hit))
            {
                Gizmos.color = Color.red;
                length = hit.distance;
            }
            Gizmos.DrawRay(_camera.transform.position, _camera.transform.forward * length);
            Gizmos.DrawWireSphere(_camera.transform.position + _camera.transform.forward * _sphereCastLength, _sphereCastRadius);
        }

        private void Start()
        {
        }

        private void Update()
        {
            if (!_isFighting)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    LaunchAttack();
                }
            }
            else if (_isFighting)
            {
                _arm.rotation = rot;
                _timer -= Time.deltaTime;
                _mouseOffset += new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")).normalized;
                if (_timer <= _delayBeforeReturn - _delayBeforeCanAttackAgain)
                {
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        LaunchAttack();
                        return;
                    }
                }
                if (_timer <= 0)
                {
                    _arm.localRotation = Quaternion.identity;
                    _isFighting = false;
                    _animator.SetTrigger(_normalParameterName);
                }
            }

        }

        private bool Hit(out RaycastHit hit)
        {
            return Physics.SphereCast(_camera.transform.position, _sphereCastRadius, _camera.transform.forward, out hit, _sphereCastLength, _hittable);
        }

        private void LaunchAttack()
        {
            _isFighting = false;
            _timer = _delayBeforeReturn + _delayBeforeCanAttackAgain;
            rot = _camera.transform.rotation;
            if (_isFighting)
            {
                _animator.ForceStateNormalizedTime(0);
            }
            else
            {
                _animator.SetTrigger(_combatParameterName);
            }
            var physicsResult = Hit(out var hit);
            if (physicsResult)
            {
                var direction = _mouseOffset.normalized;
                if (direction == Vector2.zero)
                    direction = new Vector2(0, -.1f);
                _animator.SetFloat(_directionParameterXName, direction.x);
                _animator.SetFloat(_directionParameterYName, direction.y);
            }
            _mouseOffset = Vector2.zero;
            _isFighting = true;
        }
    }
}
