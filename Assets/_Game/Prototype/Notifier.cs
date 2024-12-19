using UnityEngine;
using UnityEngine.Events;

namespace MP.Game
{
    public class Notifier : MonoBehaviour
    {
        public UnityEvent Event;

        public void Notify()
        {
            Event?.Invoke();
        }
    }
}
