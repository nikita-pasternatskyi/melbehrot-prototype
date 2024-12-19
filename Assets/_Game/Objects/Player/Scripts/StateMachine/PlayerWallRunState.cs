using MP.Game.Objects.Character;
using UnityEngine;

namespace MP.Game.Objects.Player.Scripts.StateMachine
{
    public class PlayerWallRunState : PlayerState
    {
        [SerializeField] private PlayerState _fallState;
        [SerializeField] private float _wallRunCooldown;
        [SerializeField] private float _wallRunSpeed;
        [SerializeField] private Vector2 _wallJumpForce;
        [SerializeField] private CharacterConfig _wallRunConfig;
        private PlayerWallCheck _wallCheck;
        private float _lastSuccessfullExitTime;

        private CharacterConfig _normalConfig;

        protected override void StateInited()
        {
            _normalConfig = Character.Configuration;
            _wallCheck = StateMachine.GetComponent<PlayerWallCheck>();
        }

        public override bool CanEnter()
        {
            bool result = false;

            if (Time.realtimeSinceStartup - _lastSuccessfullExitTime >= _wallRunCooldown)
                result = true;
            return _wallCheck.Check(out var hit) && result;
        }

        public override void OnEnter()
        {
            Character.Configuration = _wallRunConfig;
            Character.Velocity.y = 0;
        }

        public override void OnExit()
        {
            _lastSuccessfullExitTime = Time.realtimeSinceStartup;
            Character.Configuration = _normalConfig;
        }

        public override void StateUpdate()
        {
            var wall = _wallCheck.Check(out var hit);
            if (!wall || hit == null)
            {
                StateMachine.ChangeState<PlayerFallState>();
                return;
            }
            if (PlayerInput.InputMap.Gameplay.Jump.WasPressedThisFrame())
            {
                var jumpDir = Vector3.Lerp(hit.normal, Vector3.ProjectOnPlane(Character.Velocity, hit.normal), 0.5f).normalized;
                Character.Jump(_wallJumpForce.y, _wallJumpForce.x, jumpDir);
                StateMachine.ChangeState(_fallState);
                //StateMachine.ChangeState<PlayerFallState>();
                return;
            }
            Character.Velocity = Vector3.ProjectOnPlane(Character.Velocity, hit.normal);
        }

        public override void StateFixedUpdate()
        {
        }
    }
}
