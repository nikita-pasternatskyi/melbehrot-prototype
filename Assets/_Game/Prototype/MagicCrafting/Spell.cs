using Sirenix.OdinInspector;
using UnityEngine;

namespace MP.Game
{
    public enum SpellCostType
    {
        Instant,
        Drain,
        Reservation,
    }

    [CreateAssetMenu(menuName = "MP/Spells/Spell")]
    public class Spell : SerializedScriptableObject
    {
        public Rune[] Recipe;
        public Sprite Preview;
        public string Description;
        public float Cooldown;
        public float CastTime = 0;
        public bool UseBlood;
        public SpellCostType CostType;
        [ShowIf("@CostType == SpellCostType.Instant")] public float InstantCost;
        [ShowIf("@CostType == SpellCostType.Drain")] public float DrainRatePerSecond;
        [ShowIf("@CostType == SpellCostType.Reservation")] public float ReservationCost;
    }
}
