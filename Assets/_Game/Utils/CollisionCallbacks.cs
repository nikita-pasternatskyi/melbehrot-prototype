using MP.Game.Objects.Player.Scripts;
using UnityEngine;
using UnityEngine.Events;

namespace MP.Game.Utils
{


    public class CollisionCallbacks : MonoBehaviour
    {
        public UnityEvent Entered;
        public UnityEvent Exited;

        [SerializeField] private bool _checkForPlayerOnly;

        private void OnTriggerEnter(Collider other)
        {
            if (_checkForPlayerOnly)
            {
                if (other.GetComponent<Player>() == null)
                {
                    return;
                }
            }
            Entered?.Invoke();
        }

        private void OnTriggerExit(Collider other)
        {
            if (_checkForPlayerOnly)
            {
                if (other.GetComponent<Player>() == null)
                {
                    return;
                }
            }
            Exited?.Invoke();
        }
    }
}
