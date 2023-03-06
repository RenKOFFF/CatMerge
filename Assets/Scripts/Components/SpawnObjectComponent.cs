using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Components
{
    public class SpawnObjectComponent : MonoBehaviour
    {
        [SerializeField] private GameObject _spawnObject;
        [SerializeField] private SpawnType _spawnType;

        [Header("AreaFields")] [SerializeField]
        private Collider2D _spawnArea;

        [Header("SpecificFields")] [SerializeField]
        private Vector2 _spawnPosition;

        [Header("Other")] [SerializeField] private Transform _spawnParent;

        void Start()
        {
            if (_spawnParent == null)
                _spawnParent = transform;

            switch (_spawnType)
            {
                case SpawnType.RandomInArea:
                    if (_spawnArea == null)
                        _spawnArea = GetComponent<Collider2D>();
                
                    SpawnObjectInArea(_spawnArea);
                    break;
                case SpawnType.SpecificPosition:
                    SpawnObject();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    
        private void SpawnObject()
        {
            Instantiate(_spawnObject, _spawnPosition, Quaternion.identity, _spawnParent);
        }
    
        private void SpawnObjectInArea(Collider2D spawnArea)
        {
            var bounds = spawnArea.bounds;
            var randomPositionX = Random.Range(bounds.min.x, bounds.max.x);
            var randomPositionY = Random.Range(bounds.min.y, bounds.max.y);

            var randomSpawnPosition = new Vector2(randomPositionX, randomPositionY);

            Instantiate(_spawnObject, randomSpawnPosition, Quaternion.identity, _spawnParent);
        }
        
        [ContextMenu("SpawnObjectInArea")]
        private void Spawn()
        {
            SpawnObjectInArea(_spawnArea);
        }
    }
    
    public enum SpawnType
    {
        RandomInArea,
        SpecificPosition
    }
}