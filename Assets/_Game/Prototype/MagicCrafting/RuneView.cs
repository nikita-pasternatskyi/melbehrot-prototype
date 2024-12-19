using TMPro;
using UnityEngine;

namespace MP.Game
{
    public class RuneView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI TextField;
        public void Init(Rune rune)
        {
            var name = rune.GetName();
            gameObject.name = name;
            TextField.text = name;
        }
    }
}
