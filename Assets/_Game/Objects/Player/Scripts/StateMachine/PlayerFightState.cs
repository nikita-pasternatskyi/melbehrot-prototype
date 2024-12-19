using MP.Game.Utils.Extensions;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace MP.Game.Objects.Player.Scripts.StateMachine
{

    public class PlayerFightState : PlayerState
    {
        public UnityEvent UnityNormalAttack;

        [SerializeField] private Transform _camera;
        [SerializeField] private LayerMask _whatIsEnemy;
        [SerializeField] private PlayerMoveAndRotateAction _moveAndRotate;
        [SerializeField] private float _aimingTime;
        [SerializeField] private float _animationDuration;
        [SerializeField] private float _cooldownDuration;
        [SerializeField] private float _inputCollectionDuration;
        private Health.Health _target;
        public event Action<Vector2> AttackSelected;

        private PlayerWeapon _weapon;

        public Vector2 LastAttackDirection;
        private Vector3 _lastCameraForward;
        private Vector2 _startXY;
        private Vector2 _mouseOffset;
        private Vector2 _mouseDirection;
        private Vector2 _previousMousePos;
        private Vector2 _mouseDeltaPrev;
        private float _aimingTimer;
        private float _cooldownTimer;
        private float _animationTimer;
        private float _inputTimer;


        private Vector2 _mouseMove;

        public int CurrentAttackID { get; private set; }

        private void OnGUI()
        {
            GUI.Label(new Rect(0, 100, 250, 250), $"currentMouse: {_mouseMove}");
            GUI.Label(new Rect(0, 150, 250, 250), $"mouseOffset: {_mouseOffset}");
            GUI.Label(new Rect(0, 200, 250, 250), $"lastAttackDirection: {LastAttackDirection}");
            GUI.Label(new Rect(0, 250, 250, 250), $"lastAttackDirection: {_mouseDirection}");
        }

        public override void OnExit()
        {
            _weapon.UnequipWeapon();
        }

        public override void OnEnter()
        {
            _weapon.EquipWeapon();
            _mouseOffset = Vector2.zero;
            _lastCameraForward = _camera.forward;
            LastAttackDirection = new Vector2(0, -1);
            BeginAttack();
        }

        public override bool CanEnter()
        {
            return _weapon.HasWeapon();
        }

        public override void StateUpdate()
        {
            _moveAndRotate.MoveAndRotate(Character, PlayerInput);
                var newValue = PlayerInput.InputMap.Gameplay.Look.ReadValue<Vector2>();
                if (newValue != Vector2.zero)
                {
                    _mouseOffset += newValue;
                }
                _mouseDirection = (newValue - _mouseDeltaPrev).normalized;
                _mouseDeltaPrev = newValue;
            if (_animationTimer > 0)
            {
                _animationTimer -= Time.deltaTime;
            }
            if (_animationTimer <= 0)
            {
                if (PlayerInput.InputMap.Gameplay.SwordAttack.WasPressedThisFrame())
                {
                    BeginAttack();
                }
            }
            if(_cooldownTimer > 0)
            {
                _cooldownTimer -= Time.deltaTime;
            }
            if (_cooldownTimer <= 0)
                StateMachine.ChangeState<PlayerIdleState>();
        }

        public void Hit()
        {
            _target?.TakeDamage(1f);
        }

        private void BeginAttack()
        {
            _cooldownTimer = _cooldownDuration;
            var targetPoint = Physics.SphereCast(_camera.transform.position, 0.5f, _camera.transform.forward, out RaycastHit hit, _whatIsEnemy);
            if(targetPoint)
            {
                if(hit.collider.TryGetComponent<Health.Health>(out var health))
                {
                    _target = health;
                }
            }
            UnityNormalAttack?.Invoke();
            var vec2Diff = _mouseOffset.normalized;//_mouseDirection;//(_previousMousePos+_mouseOffset - _previousMousePos).normalized;
            if (vec2Diff == Vector2.zero)
            {
                vec2Diff = new Vector2(0, -1);
            }
            LastAttackDirection = vec2Diff;
            AttackSelected?.Invoke(vec2Diff);
            _animationTimer = _animationDuration;
            _previousMousePos = _mouseOffset;
            _mouseOffset = Vector2.zero;
        }

        protected override void StateInited()
        {
            _weapon = StateMachine.GetComponent<PlayerWeapon>();
            _weapon.UnequipWeapon();
        }
    }
}
