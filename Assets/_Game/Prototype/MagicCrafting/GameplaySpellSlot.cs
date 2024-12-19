using UnityEngine;
using UnityEngine.Events;

namespace MP.Game
{
    public class GameplaySpellSlot : MonoBehaviour
    {
        public Slot TargetSlot;
        public Spell CurrentSpell;
        private Transform _currentChild;
        public UnityEvent Selected;
        public UnityEvent Deselected;

        private void Awake()
        {
            Deselect();
        }

        public void Select()
        {
            Selected?.Invoke();
        }

        public void Deselect()
        {
            Deselected?.Invoke();
        }

        private void OnEnable()
        {
            TargetSlot.NewDraggable.AddListener(OnNewSpellAdded);
            TargetSlot.ChildRemoved.AddListener(OnSpellRemoved);
        }

        private void OnNewSpellAdded(Transform newSpell)
        {
            _currentChild = Instantiate(newSpell, transform);
            CurrentSpell = newSpell.GetComponent<SpellView>().Spell;  
        }
        private void OnSpellRemoved()
        {
            CurrentSpell = null;
            Destroy(_currentChild.gameObject);
        }

        private void OnDisable()
        {
            TargetSlot.NewDraggable.RemoveListener(OnNewSpellAdded);
            TargetSlot.ChildRemoved.RemoveListener(OnSpellRemoved);
        }
    }
}
