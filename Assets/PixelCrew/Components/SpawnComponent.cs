using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components
{
    public class SpawnComponent : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private GameObject _prefab;

        [ContextMenu("Spawn")]
        public void Spawn()
        {
            var instantiate = Instantiate(_prefab, _target.position, Quaternion.identity);
            var scale = _target.lossyScale;
            instantiate.transform.localScale = scale;
        }
    }
}
