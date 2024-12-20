﻿using UnityEngine;

namespace MP.Game.Objects.Player.Scripts.StateMachine.PlayerHorseStates
{
    public class PlayerHorseRunState : PlayerHorseState
    {
        [SerializeField] private PlayerMoveAndRotateAction _moveAndRotate;

        protected override void OnStateUpdated()
        {
            if (PlayerInput.AbsoluteMovementInput == Vector2.zero)
            {
                StateMachine.ChangeState<PlayerHorseIdleState>();
                return;
            }
            if (PlayerInput.InputMap.Gameplay.Run.phase != UnityEngine.InputSystem.InputActionPhase.Performed)
            {
                StateMachine.ChangeState<PlayerHorseWalkState>();
                return;
            }
        }

        public override void StateFixedUpdate()
        {
            _moveAndRotate.MoveAndRotate(Character, PlayerInput);
        }
    }
}
