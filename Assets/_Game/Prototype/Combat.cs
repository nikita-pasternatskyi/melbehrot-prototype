using MP.Game.Objects.Health;
using UnityEngine;

namespace MP.Game
{
    public class Combat : MonoBehaviour
    {
        [Header("Animation")]
        public Animator animator;
        public string directionParameterXName;
        public string directionParameterYName;
        public string combatParameterName;
        public string normalParameterName;

        [Header("Gameplay")]
        public Transform arm;
        public Transform sword;
        public Camera camera;
        public LayerMask hittable;
        public float sphereCastRadius;
        public float sphereCastLength;
        public float delayBeforeReturn;
        public bool CanAttack = true;

        public Health target;
        public bool isFighting = false;
        public float DamageRadius;
        public float Damage;
        public bool UseRaycastForDamage = true;

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            var length = sphereCastLength;
            if (Hit(out var hit))
            {
                Gizmos.color = Color.red;
                length = hit.distance;
            }
            Gizmos.DrawRay(camera.transform.position, camera.transform.forward * length);
            Gizmos.DrawWireSphere(camera.transform.position + camera.transform.forward * sphereCastLength, sphereCastRadius);
        }
        public bool Hit(out RaycastHit hit)
        {
            return Physics.SphereCast(camera.transform.position, sphereCastRadius, camera.transform.forward, out hit, sphereCastLength, hittable);
        }

        public void EnableAttack() => CanAttack = true;

        public void DealDamage()
        {
            if (!UseRaycastForDamage)
            {
                var overlaps = Physics.OverlapSphere(sword.transform.position, DamageRadius, hittable);
                foreach (var item in overlaps)
                {
                    if (item == null)
                        continue;
                    if (item.TryGetComponent<Health>(out var hp))
                    {
                        hp.TakeDamage(Damage);
                    }
                }
            }
            else
            {
                if(Hit(out var hit))
                {
                    if(hit.collider.TryGetComponent(out Health health))
                    {
                        health.TakeDamage(Damage);
                    }
                }
            }
        }

    }
}
