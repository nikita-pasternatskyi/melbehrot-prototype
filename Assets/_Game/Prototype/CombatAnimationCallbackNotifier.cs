using UnityEngine;
using UnityEngine.Events;

namespace MP.Game
{
    public class CombatAnimationCallbackNotifier : MonoBehaviour
    {
        public UnityEvent DealDamage;
        public UnityEvent CanAttack;

        public void Damage() => DealDamage?.Invoke();
        public void CanAtack() => CanAttack?.Invoke();
    }
}
