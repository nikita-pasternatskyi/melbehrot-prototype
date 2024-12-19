using MP.Game.Objects.Health;
using UnityEngine;
using UnityEngine.UIElements;

namespace MP.Game
{


    public class MouseAccurateCombat : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _wideAttackThreshold;
        [SerializeField] private float _highAttackThreshold;
        [SerializeField] private float _stabAttackWaitTime;
        [SerializeField] private float _wideAttackDamage;
        [SerializeField] private float _wideAttackRange;
        [SerializeField] private float _stabAttackDamage;
        [SerializeField] private float _positionChangeThreshold;
        public float Multiplier;
        private Combat _combat;

        private Vector3 _previousPosition;
        private Quaternion _previousRotation;
        private float _timer;


        private void OnDrawGizmosSelected()
        {
            if (_combat == null)
                _combat = GetComponent<Combat>();
            Gizmos.DrawWireSphere(_combat.sword.position, _wideAttackRange);
        }

        private void Awake()
        {
            _combat = GetComponent<Combat>();
        }

        private void Start()
        {
        }

        private void Update()
        {
            if (!_combat.isFighting)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    _combat.CanAttack = true;
                    _previousPosition = _combat.transform.position;
                    _previousRotation = _combat.camera.transform.rotation;
                    LaunchAttack();
                }
            }
            else if (_combat.isFighting)
            {
                _combat.arm.rotation = _previousRotation;
                _timer -= Time.deltaTime;
                if (_combat.CanAttack)
                {
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        LaunchAttack();
                        return;
                    }
                }
                if (_timer <= 0)
                {
                    _combat.arm.localRotation = Quaternion.identity;
                    _combat.isFighting = false;
                    _combat.animator.SetTrigger(_combat.normalParameterName);
                }
            }

        }

        public void DealDamage()
        {
            if (_combat.target == null)
                return;
            _combat.target.TakeDamage(1);
        }

        private bool Hit(out RaycastHit hit)
        {
            return _combat.Hit(out hit);
        }

        private void LaunchAttack()
        {
            _combat.CanAttack = false;
            _combat.Damage = 1;
            _combat.DamageRadius = _combat.sphereCastRadius;
            _combat.UseRaycastForDamage = true;
            _combat.target = null;
            _combat.isFighting = false;
            _timer = _combat.delayBeforeReturn;
            _combat.animator.Play(_combat.combatParameterName, -1, 0);

            var rotation = _combat.camera.transform.rotation;
            var difference = rotation * Quaternion.Inverse(_previousRotation);
            var euler = difference.eulerAngles;
            for (int i = 0; i < 3; i++)
            {
                var num = euler[i];
                euler[i] = (num + 180) % 360 - 180;
            }

            var direction = new Vector2(euler.y, euler.x);
            direction *= Multiplier;
            euler *= Multiplier;
            var dir = _rigidbody.velocity;
            dir.y = 0;
            var dot = Vector3.Dot(dir.normalized, _combat.transform.right);
            var absDot = Mathf.Abs(dot);

            float singleAngle = (360.0f / 8);
            for(int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    var num = euler[i];
                    if (Mathf.Abs(num) < 15)
                        num = 0;
                    else if (Mathf.Abs(num) >= 15)
                        num = Mathf.Sign(num) * 45;
                    if (num > j * singleAngle && num < (j + 1) * singleAngle)
                    {
                        euler[i] = j * singleAngle;
                        Debug.Log(euler[i]);
                    }
                }
            }
            direction = new Vector2(euler.y, euler.x);

            if (dir.magnitude > _positionChangeThreshold)
            {
                if (absDot > 0.6)
                {
                    direction = new Vector2(Mathf.Sign(dot) * 90, 0);
                }
            }
            if (_rigidbody.velocity.y > 0)
            {
                direction = new Vector2(0, Mathf.Sign(_rigidbody.velocity.y) * -90);
            }
            if (Mathf.Abs(direction.x) >= 75 && Mathf.Abs(direction.y) <= 45)
            {
                _combat.Damage = _wideAttackDamage;
                _combat.DamageRadius = _wideAttackRange;
                _combat.UseRaycastForDamage = false;
            }
            _previousRotation = rotation;
            _combat.animator.SetFloat(_combat.directionParameterXName, direction.x);
            _combat.animator.SetFloat(_combat.directionParameterYName, direction.y);
            _combat.isFighting = true;

            _previousPosition = _combat.transform.position;
        }
    }
}
