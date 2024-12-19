using UnityEngine;

namespace MP.Game
{
    public class AddForceToRigidbody : MonoBehaviour
    {
        public float Force;
        private Rigidbody _rigidbody;
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void Act()
        {
            _rigidbody.AddForce(-transform.forward * Force, ForceMode.VelocityChange);
        }
    }
}
