using MP.Game.Objects.Health;
using MP.Game.Utils;
using UnityEngine;

namespace MP.Game
{
    public class BoneSpearSpell : SpellImplementation<Spell>
    {
        [SerializeField] private DamageArea _bonePrefab;
        [SerializeField] private Vector3 _targetScale;
        [SerializeField] private LayerMask _whatIsEnemy;
        [SerializeField] private Camera _camera;
        private float _castingTimer;
        private bool _casting;
        private GameObject _boneInstance;

        private void Update()
        {
            if(_casting)
            {
                _castingTimer -= Time.deltaTime;
                _boneInstance.transform.localScale = Vector3.Lerp(Vector3.zero, _targetScale, (Spell.CastTime - _castingTimer)/Spell.CastTime);
                
                if (_castingTimer < 0)
                {
                    if (Input.GetMouseButton(1) == false)
                    {
                        _boneInstance.transform.parent = null;
                        _boneInstance.transform.rotation = Quaternion.identity;
                        var direction = _camera.transform.forward;
                        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out var hit, 1000, _whatIsEnemy))
                        {
                            direction = (hit.point - _boneInstance.transform.position).normalized;
                            
                            //_boneInstance.transform.LookAt(hit.point);
                        }
                        _boneInstance.GetComponent<Projectile>().StartMoving(direction);
                        _casting = false;
                        RecordCooldown();
                        Mana.TakeDamage(Spell.InstantCost);
                    }
                }
            }
        }

        protected override void OnBegin()
        {
            _casting = true;
            _castingTimer = Spell.CastTime;
            _boneInstance = GlobalObjectPool.Instance.Get(_bonePrefab.gameObject);
            _boneInstance.transform.parent = transform;
            _boneInstance.transform.localPosition = Vector3.zero;
            _boneInstance.transform.localRotation = Quaternion.identity;
            _boneInstance.transform.localScale = Vector3.zero;
            _boneInstance.SetActive(true);
        }
    }
}
