using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MP.Game
{

    public class Slot : MonoBehaviour
    {
        private Button _button;
        private Draggable _currentDraggable;
        public UnityEvent<Transform> NewDraggable;
        public UnityEvent ChildRemoved;


        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(OnClicked);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnClicked);
        }

        public void AdoptDraggable()
        {
            OnClicked();
        }

        public void ReleaseDraggable()
        {
            _currentDraggable.Return();
            _currentDraggable = null;
            ChildRemoved?.Invoke();
        }

        private void OnClicked()
        {
            if (_currentDraggable)
                _currentDraggable.Return();
            _currentDraggable = SlotManager.Instance.CurrentlySelectedDraggable;

           _currentDraggable?.SnapIntoSlot(this);
            if(_currentDraggable != null)
                NewDraggable?.Invoke(_currentDraggable.transform);
            SlotManager.Instance.CurrentlySelectedDraggable = null;
        }
    }
}
