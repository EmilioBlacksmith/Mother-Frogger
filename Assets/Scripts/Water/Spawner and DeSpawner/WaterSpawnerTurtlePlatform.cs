using Character_System.HP_System;
using Game_Manager;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Water.Spawner_and_DeSpawner
{
    public class WaterSpawnerTurtlePlatform : MonoBehaviour
    {
        private enum PlatformDirection
        {
            Normal,
            Inverse
        };
    
        [SerializeField] private PlatformDirection thisDirection;
        [SerializeField] private GameObject doublePlatform;
        [SerializeField] private GameObject triplePlatform;
        [SerializeField] private float startingTimeBetweenSpawn = 10;
        //[SerializeField] private float lifeSpan = 10;
    
        private float _timeBetweenSpawn;
        private int _randomNum;
        private float _timer = 0;
        private readonly Quaternion _inverseRotationDirection = Quaternion.Euler(0,-90,0);
        private readonly Quaternion _normalRotationDirection = Quaternion.Euler(0,90,0);
        private Quaternion _thisDirectionRotation;

        private void Start()
        {
            _randomNum = (Random.Range(0, 5000))%2;
            _timeBetweenSpawn = startingTimeBetweenSpawn;
        
            _thisDirectionRotation = thisDirection switch
            {
                PlatformDirection.Normal => _normalRotationDirection,
                PlatformDirection.Inverse => _inverseRotationDirection,
                _ => _thisDirectionRotation
            };
        
            switch (_randomNum)
            {
                case 0:
                    //Instantiate(triplePlatform, transform.position, _thisDirectionRotation);
                    ObjectPoolManager.SpawnObject(triplePlatform, transform.position, _thisDirectionRotation, ObjectPoolManager.PoolType.WaterObject);
                    break;
                case 1:
                    ObjectPoolManager.SpawnObject(doublePlatform, transform.position, _thisDirectionRotation, ObjectPoolManager.PoolType.WaterObject);
                    break;
            }
            _timer = 0;
        }
    
        private void Update()
        {
            if(HealthSystem.Instance.IsGameOver) return;

            _timeBetweenSpawn = startingTimeBetweenSpawn; //- (GameManager.Instance.DifficultyLevel() * 2f);
            _timeBetweenSpawn = Mathf.Clamp(_timeBetweenSpawn, 3f, 12f);
        
            if (_timer >= _timeBetweenSpawn)
            {
                _randomNum = (Random.Range(0, 5000))%2;
                var positionSpawn = transform.position;
                var newObj = _randomNum switch
                {
                    0 => ObjectPoolManager.SpawnObject(triplePlatform, positionSpawn, _thisDirectionRotation, ObjectPoolManager.PoolType.WaterObject),
                    1 => ObjectPoolManager.SpawnObject(doublePlatform, positionSpawn, _thisDirectionRotation, ObjectPoolManager.PoolType.WaterObject),
                    _ => null
                };
                //Destroy(newObj, lifeSpan * (10 - GameManager.Instance.DifficultyLevel()));;
                _timer = 0;
            }

            _timer += Time.deltaTime;
        }
    }
}
