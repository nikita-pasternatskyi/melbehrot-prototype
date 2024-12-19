using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MP.Game
{
    [CreateAssetMenu(menuName ="MP/Rune")]
    public class Rune : ScriptableObject
    {
        public RuneView Prefab;
        public string GetName()
        {
            return name.Substring(name.LastIndexOf('_') + 1);
        }
    }
}
