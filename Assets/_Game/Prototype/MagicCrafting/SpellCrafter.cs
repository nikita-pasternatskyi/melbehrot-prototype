using System.Collections.Generic;
using UnityEngine;

namespace MP.Game
{
    public class SpellCrafter : MonoBehaviour
    {
        [SerializeField] private Spell[] _spells;
        [SerializeField] private SpellView _spellViewPrefab;
        [SerializeField] private Transform _spellBook;
        [SerializeField] private SpellView _fullSpellRecipeViewPrefab;
        [SerializeField] private Transform _spellMiniPreview;
        private Dictionary<string, Spell> _recipes;
        private HashSet<Spell> _foundRecipes = new HashSet<Spell>();

        private void Awake()
        {
            _recipes = new Dictionary<string, Spell>(_spells.Length + 1);
            foreach (var spell in _spells)
            {
                string recipe = string.Empty;
                foreach(var rune in spell.Recipe)
                {
                    recipe += rune.GetName();
                }
                _recipes.Add(recipe, spell);
            }
        }

        public void Craft()
        {
            var possibleRecipe = string.Empty;
            for(int i = 0; i < transform.childCount; i++)
            {
                if(transform.GetChild(i).childCount == 0)
                    continue;
                possibleRecipe += transform.GetChild(i).GetChild(0).name;
            }
            if(_recipes.TryGetValue(possibleRecipe, out Spell spell))
            {
                var instance = Instantiate(_spellViewPrefab, _spellMiniPreview);
                instance.Init(spell);

                if (_foundRecipes.Contains(spell) == false)
                {
                    var inst = Instantiate(_fullSpellRecipeViewPrefab, _spellBook);
                    inst.Init(spell);
                    _foundRecipes.Add(spell);
                }
            }
        }
    }
}
