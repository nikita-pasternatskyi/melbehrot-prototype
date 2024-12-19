using UnityEngine;

namespace MP.Game
{
    public class RuneCreator : MonoBehaviour
    {
        [SerializeField] private Rune[] _runes;
        private void Awake()
        {
            foreach (var rune in _runes)
            {
                var inst = Instantiate(rune.Prefab, transform);
                inst.Init(rune);
            }
        }
    }
}
