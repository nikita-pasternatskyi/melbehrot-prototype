using MP.Game.Objects.Health;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MP.Game
{

    public class PlayerMagic : SerializedMonoBehaviour
    {
        public FirstPersonController FirstPersonController;
        public Transform ArmMagicSpawnPoint;
        public Image WheelWindow;
        public GameplaySpellSlot[] MagicSpells;
        public Dictionary<Spell, SpellImplementation> SpellImplementations;
        [field:SerializeField] public Health Mana { get; private set; }
        [field:SerializeField] public Health Health { get; private set; }
        private bool _selectingMagic;
        private Vector2 _mouseSelected;

        private int _selectedMagic = 0;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                WheelWindow.fillAmount = 1;
                _selectingMagic = true;
                _mouseSelected = Vector2.zero;
                FirstPersonController.cameraCanMove = false;
            }

            if (Input.GetKeyUp(KeyCode.Q))
            {
                _selectingMagic = false;
                WheelWindow.fillAmount = 0;
                FirstPersonController.cameraCanMove = true;
                foreach (var item in MagicSpells)
                {
                    item.Deselect();
                }
            }

            if(_selectingMagic)
            {
                var mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
                _mouseSelected += mouseInput;
                //_mouseSelected = new Vector2(Input.mousePosition.x - (Screen.width / 2), Input.mousePosition.y - (Screen.height/2)).normalized;
                _mouseSelected.Normalize(); 
                if (_mouseSelected == Vector2.zero)
                    return;
                var angle = Mathf.Atan2(_mouseSelected.y, -_mouseSelected.x) / Mathf.PI;
                angle *= 180;
                angle -= 90;
                if (angle < 0)
                    angle += 360;
                float singleAngle = (360.0f / MagicSpells.Length);
                for (int i = 0; i < MagicSpells.Length; i++)
                {
                    var bottomEdge = i * singleAngle;
                    var topEdge = (i + 1) * singleAngle;
                    if (topEdge == 90)
                        topEdge = 60; 
                    if (bottomEdge == 90)
                        bottomEdge = 60;
                    if (bottomEdge == 270)
                        bottomEdge = 240;
                    if (topEdge == 270)
                        topEdge = 240;
                    if (topEdge == 360)
                        topEdge = 340;

                    if (angle >= bottomEdge && angle < topEdge)
                    {
                        _selectedMagic = i;
                        MagicSpells[i].Select();
                    }
                    else
                    {
                        MagicSpells[i].Deselect();
                        if (i == MagicSpells.Length - 1 && angle >= 340)
                        {
                            MagicSpells[0].Select();
                        }
                    }
                }
            }

            if(!_selectingMagic)
            {
                if(Input.GetMouseButtonDown(1))
                {
                    var currentSpell = MagicSpells[_selectedMagic].CurrentSpell;
                    if (currentSpell != null)
                    {
                        if(SpellImplementations.TryGetValue(currentSpell, out var spell))
                        {
                            spell.Begin();
                        }
                    }
                }
            }
        }
    }
}
