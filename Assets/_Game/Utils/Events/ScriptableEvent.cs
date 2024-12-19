﻿using System;
using UnityEngine;

namespace MP.Game.Utils
{
    [CreateAssetMenu(menuName = "MP/Events/Event")]
    public class ScriptableEvent : ScriptableObject
    {
        public event Action Event;

        public void Invoke() => Event?.Invoke();
    }
}
