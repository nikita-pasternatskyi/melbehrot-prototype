using MP.Game.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace MP.Game.Objects.Player.Scripts
{
    public class PlayerCombatState : MonoBehaviour
    {
        [SerializeField] private ScriptableEvent AddedEnemyToTheFight;
        [SerializeField] private ScriptableEvent RemovedEnemyFromTheFight;

        public UnityEvent CombatBegin;
        public UnityEvent CombatEnd;

        public bool InCombat => _enemies > 0;
        private int _enemies;

        [HideInInspector] public bool CanBeAttacked = true;
        [HideInInspector] public GameObject AttackingEnemy;

        private void OnEnemyRemovedFromFight()
        {
            _enemies--;
            if (_enemies == 0)
                CombatEnd?.Invoke();
        }

        private void OnEnemyAdded()
        {
            if (_enemies == 0)
                CombatBegin?.Invoke();
            _enemies++;
        }

        private void OnEnable()
        {
            AddedEnemyToTheFight.Event += OnEnemyAdded;
            RemovedEnemyFromTheFight.Event += OnEnemyRemovedFromFight;
        }

        private void OnDisable()
        {
            AddedEnemyToTheFight.Event -= OnEnemyAdded;
            RemovedEnemyFromTheFight.Event -= OnEnemyRemovedFromFight;
        }
    }
}
