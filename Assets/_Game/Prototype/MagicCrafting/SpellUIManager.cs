using MP.Game.Assets.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MP.Game
{
    public class SpellUIManager : UnitySingleton<SpellUIManager>
    {
        public Image Preview;
        public TextMeshProUGUI Title;
        public TextMeshProUGUI Description;

        public void Focus(Spell spell)
        {
            if(spell != null)
            {
                Title.text = spell.name;
                Description.text = spell.Description;
                Preview.sprite = spell.Preview;
                Preview.color = Color.white;
                return;
            }
            Title.text = string.Empty;
            Description.text = string.Empty;
            Preview.sprite = null;
            Preview.color = new Color(0,0,0,0);
        }
    }
}
