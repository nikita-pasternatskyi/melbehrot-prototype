using MP.Game.Utils;
using UnityEngine;

namespace MP.Game
{

    public class BoneHellSpell : SpellImplementation<Spell>
    {
        [SerializeField] private GameObject _boneExplosionPreviewPrefab;
        [SerializeField] private GameObject _boneExplosionPrefab;
        [SerializeField] private Camera _camera;
        [SerializeField] private float _raycastLength;
        [SerializeField] private float _raycastRadius;
        [SerializeField] private LayerMask _floor;
        private float _castingTimer;
        private bool _casting;
        private Transform _previewInstance;

        private void Update()
        {
            if(_casting)
            {
                _castingTimer -= Time.deltaTime;

                RaycastHit hit;
                if (Physics.SphereCast(_camera.transform.position, _raycastRadius, _camera.transform.forward, out hit, 1000f, _floor))
                {
                    _previewInstance.transform.position = hit.point;
                }

                if (Input.GetMouseButtonUp(1))
                {
                    _casting = false;
                    _previewInstance.gameObject.SetActive(false);
                    return;
                }
                if (_castingTimer <= 0)
                {
                    if (Physics.SphereCast(_camera.transform.position, _raycastRadius, _camera.transform.forward, out hit, 1000f, _floor))
                    {
                        _previewInstance.transform.position = hit.point;
                    }
                    var inst = GlobalObjectPool.Instance.Get(_boneExplosionPrefab);
                    inst.transform.position = _previewInstance.transform.position;
                    inst.SetActive(true);
                    Mana.TakeDamage(Spell.InstantCost);
                    RecordCooldown();
                    _casting = false;
                    _previewInstance.gameObject.SetActive(false);
                }
            }
        }

        protected override void OnBegin()
        {
            _casting = true;
            _castingTimer = Spell.CastTime;
            _previewInstance = GlobalObjectPool.Instance.Get(_boneExplosionPreviewPrefab).transform;
            _previewInstance.gameObject.SetActive(true);
        }
    }
}
