using UnityEngine;

namespace MP.Game.Objects.Player.Scripts.StateMachine
{
    public class PlayerFallState : PlayerState
    {
        [SerializeField] private PlayerMoveAndRotateAction _moveAndRotate;
        private PlayerWallCheck _wallCheck;

        protected override void StateInited()
        {
            base.StateInited();
            _wallCheck = StateMachine.GetComponent<PlayerWallCheck>();
        }

        public override void StateFixedUpdate()
        {
            _moveAndRotate.MoveAndRotate(Character, PlayerInput);
        }

        public override void StateUpdate()
        {
            if (Character.Grounded)
                StateMachine.ChangeState<PlayerIdleState>();
            if (_wallCheck.Check(out var hit))
                StateMachine.ChangeState<PlayerWallRunState>();
        }
    }
}
