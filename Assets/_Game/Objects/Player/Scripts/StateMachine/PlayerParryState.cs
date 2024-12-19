using MP.Game.Objects.Enemy.Scripts;
using UnityEngine;
using UnityEngine.Events;

namespace MP.Game.Objects.Player.Scripts.StateMachine
{
    public class PlayerParryState : PlayerState
    {
        private enum ParryStates
        {
            SuccesfullParry,
            UnSuccesfullParry,
            ParryAttack,
        }

        [Header("Parry")]
        [SerializeField] private PlayerDangerEvent _parryDanger;
        [SerializeField] private float _parryDuration = 0.5f;

        [Header("Parry Attack")]
        [SerializeField] private float _timeBeforeCanAttack = 0.2f;
        [SerializeField] private PlayerAttack _parryThrustAttack;

        [Header("Events")]
        public UnityEvent SuccesfullParry;
        public UnityEvent UnSuccesfullParry;
        public UnityEvent ParryAttack;

        private ParryStates _currentState;
        private GameObject _enemyToParry;
        private float _timer;
        private Vector3 _facingDirection;

        private PlayerCombatState _combatState;
        private PlayerWeapon _weapon;

        protected override void StateInited()
        {
            _combatState = StateMachine.GetComponent<PlayerCombatState>();
            _weapon = StateMachine.GetComponent<PlayerWeapon>();
        }

        public override void OnEnter()
        {
            if (_enemyToParry == null)
            {
                UnSuccesfullParry?.Invoke();
                _currentState = ParryStates.UnSuccesfullParry;
            }

            else if (_enemyToParry != null)
            {
                //_weapon.Equip();
                //_weapon.SetDamage(0);
                _currentState = ParryStates.SuccesfullParry;
            }
            switch (_currentState)
            {
                case ParryStates.SuccesfullParry:
                    SuccesfullParry?.Invoke();
                    _enemyToParry.GetComponent<Enemy.Scripts.Enemy>().StunEnemy(_parryDuration);
                    _facingDirection = (_enemyToParry.transform.position - Character.transform.position).normalized;
                    break;
                case ParryStates.UnSuccesfullParry:
                    break;
            }

            Character.Velocity = Vector3.zero;
            _timer = _parryDuration;
        }

        public override void StateUpdate()
        {
            Character.AlignToVector(_facingDirection, 0.1f);
            switch (_currentState)
            {
                case ParryStates.SuccesfullParry:
                    if (_timer <= 0)
                    {
                        StateMachine.ChangeState<PlayerIdleState>();
                        break;
                    }
                    if (_timer <= _parryDuration - _timeBeforeCanAttack)
                    {
                        if (PlayerInput.InputMap.Gameplay.SwordAttack.triggered)
                        {
                            _timer = _parryThrustAttack.ReturnDelay;
                            ParryAttack?.Invoke();
                            _currentState = ParryStates.ParryAttack;

                            //_weapon.SetDamage(_parryThrustAttack.Damage);
                            Character.Jump(_parryThrustAttack.JumpForce.y, _parryThrustAttack.JumpForce.x, transform.forward);
                            break;
                        }
                    }
                    break;

                case ParryStates.UnSuccesfullParry:
                    _timer -= Time.deltaTime;
                    if (_timer <= 0)
                    {
                        StateMachine.ChangeState<PlayerIdleState>();
                        break;
                    }
                    break;

                case ParryStates.ParryAttack:
                    _timer -= Time.deltaTime;
                    if (_timer <= 0)
                    {
                        StateMachine.ChangeState<PlayerIdleState>();
                    }
                    break;
            }
        }

        public override void OnExit()
        {
            //_weapon.Unequip();
            //_weapon.SetDamage(0);
            _combatState.CanBeAttacked = true;
            ResetParry();
        }

        #region ParryDangerEvent

        private void OnEnable()
        {
            _parryDanger.GenericEvent += OnParryDanger;
        }

        private void OnDisable()
        {
            _parryDanger.GenericEvent -= OnParryDanger;
        }

        private void OnParryDanger(GameObject arg1, EnemyAttack arg2)
        {
            _timer = arg2.DangerWarningTime;
            _enemyToParry = arg1;
        }
        #endregion

        #region ResetParry

        private void Update()
        {
            switch (_currentState)
            {
                case ParryStates.SuccesfullParry:
                    if (_timer > 0)
                    {
                        _timer -= Time.deltaTime;
                        if (_timer <= 0)
                            ResetParry();
                    }
                    break;
            }
        }

        private void ResetParry()
        {
            _timer = 0;
            _enemyToParry = null;
        }
        #endregion
    }
}
