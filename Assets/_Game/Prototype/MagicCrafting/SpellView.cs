using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MP.Game
{

    public class SpellView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public TextMeshProUGUI Title;
        public Image image;
        public bool ShowRecipe = false;
        public Spell Spell;

        public void Init(Spell spell)
        {
            Spell = spell;
            image.sprite = spell.Preview;
            if (!ShowRecipe)
                return;
            foreach (var item in spell.Recipe)
            {
                var instance = Instantiate(item.Prefab, transform);
                instance.Init(item);
            }
            if (image.gameObject.transform.parent == transform)
                image.gameObject.transform.SetAsLastSibling();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (Spell == null)
                return;
            SpellUIManager.Instance.Focus(Spell);    
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            SpellUIManager.Instance.Focus(null);
        }
    }
}
