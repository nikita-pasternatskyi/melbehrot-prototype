using UnityEngine;

namespace MP.Game.Objects.Player.Scripts
{
    public class PlayerWallCheck : MonoBehaviour
    {
        [SerializeField] private float _wallCheckRadius;
        [SerializeField] private LayerMask _whatIsWall;
        private bool _onWall;
        private ControllerColliderHit _wallHit;

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (Mathf.Abs(Vector3.Dot(hit.normal, Vector3.up)) < 0.1f)
            {
                _onWall = true;
                _wallHit = hit;
                return;
            }
            _onWall = false;
            _wallHit = null;
        }

        private void FixedUpdate()
        {
            if (!Physics.CheckSphere(transform.position, _wallCheckRadius, _whatIsWall))
            {
                _onWall = false;
                _wallHit = null;
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, _wallCheckRadius);
        }

        public bool Check(out ControllerColliderHit hit)
        {
            hit = _wallHit;
            return _onWall;
            //var direction = _playerInput.RelativeMovementInput;
            //return Physics.SphereCast(transform.position, _wallCheckRadius, direction, out hit, _wallCheckLength, _whatIsWall);
        }
    }
}
