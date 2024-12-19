using MP.Game.Objects.Health;
using UnityEngine;

namespace MP.Game
{
    public class RestoreManaForHealth : SpellImplementation<ManaRestoreFromBloodSpell>
    {
        private bool _casting;
        private float _castingTimer;

        private void Update()
        {
            if (Input.GetMouseButtonUp(1))
            {
                _casting = false;
                RecordCooldown();
            }
            if(_casting)
            {
                _castingTimer -= Time.deltaTime;
                if (_castingTimer <= 0)
                {
                    Mana.Heal(Spell.ManaRestoreRate * Time.deltaTime);
                    Health.TakeDamage(Spell.DrainRatePerSecond * Time.deltaTime);
                    if(Mana.CurrentHealth == Mana.MaxHealth)
                    {
                        _casting = false;
                        RecordCooldown();
                    }
                }
            }
        }

        protected override void OnBegin()
        {
            _casting = true;
            _castingTimer = Spell.CastTime;
        }
    }
}
