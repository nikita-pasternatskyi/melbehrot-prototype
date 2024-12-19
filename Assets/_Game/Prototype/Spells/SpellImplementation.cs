using MP.Game.Objects.Health;
using UnityEngine;
using UnityEngine.Events;

namespace MP.Game
{
    public abstract class SpellImplementation : MonoBehaviour
    {
        public abstract void Begin();
    }

    public abstract class SpellImplementation<T> : SpellImplementation where T : Spell
    {
        public UnityEvent Began;
        public T Spell;
        public Health Mana;
        public Health Health;
        private float _lastTimeStamp;

        public override void Begin() 
        {
            bool enoughCapacity = false;
            var currentCapacity = Spell.UseBlood ? Health.CurrentHealth : Mana.CurrentHealth;
            switch (Spell.CostType)
            {
                case SpellCostType.Instant:
                    enoughCapacity = Spell.InstantCost >= currentCapacity;
                    break;
                case SpellCostType.Drain:
                    enoughCapacity = Spell.DrainRatePerSecond >= currentCapacity;
                    break;
                case SpellCostType.Reservation:
                    enoughCapacity = Spell.ReservationCost >= currentCapacity;
                    break;
            }
            if (enoughCapacity || Time.realtimeSinceStartup - _lastTimeStamp < Spell.Cooldown)
                return;
            Began?.Invoke(); OnBegin();
        }

        protected void RecordCooldown() => _lastTimeStamp = Time.realtimeSinceStartup;

        protected abstract void OnBegin();
    }
}
