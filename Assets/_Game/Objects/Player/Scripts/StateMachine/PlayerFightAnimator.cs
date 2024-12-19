using UnityEngine;

namespace MP.Game.Objects.Player.Scripts.StateMachine
{
    public class PlayerFightAnimator : MonoBehaviour
    {
        [SerializeField] private PlayerFightState _fightState;
        [SerializeField] private Animator _animator;
        private void OnEnable()
        {
            _fightState.AttackSelected += OnAttackSelected;
        }

        private void OnAttackSelected(Vector2 obj)
        {
            _animator.SetFloat("X", obj.x);
            _animator.SetFloat("Y", obj.y);
        }

        private void OnDisable()
        {
            _fightState.AttackSelected -= OnAttackSelected;

        }
    }
}
