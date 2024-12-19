using UnityEngine;
using UnityEngine.UI;

namespace MP.Game
{
    public class Draggable : MonoBehaviour
    {
        private Slot _currentSlot;
        private Button _button;
        private Transform _previousParent;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _previousParent = transform.parent;
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(OnClicked);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnClicked);
        }

        private void OnClicked()
        {
            if (!_currentSlot)
                SlotManager.Instance.CurrentlySelectedDraggable = this;
            if (SlotManager.Instance.CurrentlySelectedDraggable != this)
            {
                _currentSlot.AdoptDraggable();
            }
            else
            {
                _currentSlot?.ReleaseDraggable();
            }
        }

        public void Return()
        {
            transform.parent = _previousParent;
            _currentSlot = null;
        }

        public void SnapIntoSlot(Slot slot)
        {
            transform.parent = slot.transform;
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one;
            _currentSlot = slot;
        }
    }
}
