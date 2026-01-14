using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scripts
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private Cube _cubePrefab;
        [SerializeField] private float _spawnPositionX;
        [SerializeField] private float _spawnPositionZ;

        private PoolObject<Cube> _poolCubes;
        private Pool _poolCube;

        private void Start()
        {
            CubeFactory factory = new CubeFactory(_cubePrefab);
            _poolCubes = new(factory);
            StartCoroutine(Spawn());
        }

        private IEnumerator Spawn()
        {
            WaitForSeconds wait = new WaitForSeconds(0.1f);

            while (enabled)
            {
                yield return wait;
                SpawnCube();
            }
        }

        private void SpawnCube()
        {
            Cube cube = _poolCubes.Get(_poolCubes.Return);
            
            Vector3 cubePosition = new Vector3(
                Random.Range(-_spawnPositionX, _spawnPositionX), 0,
                Random.Range(-_spawnPositionZ, _spawnPositionZ));
            
            cube.transform.parent = transform;
            cube.transform.localPosition = cubePosition;
            cube.gameObject.SetActive(true);
        }
    }

    public class Pool
    {
        private List<MonoBehaviour> _items;

        private Action<MonoBehaviour> _returnToPool;
        private Func<MonoBehaviour> _create;
        
        public Pool(MonoBehaviour prefab)
        {
            _items = new List<MonoBehaviour>();
            _create = () => MonoBehaviour.Instantiate(prefab);
        }

        public MonoBehaviour Get => _items.Count > 0 ? _items.Find(item => item.gameObject.activeSelf == false) : _create.Invoke();
    }
    
    public class PoolObject<T> where T : MonoBehaviour, IPoolItem
    {
        private Queue<IPoolItem> _items;
        private IPoolObjectFactory _factory;

        public PoolObject(IPoolObjectFactory factory)
        {
            _items = new Queue<IPoolItem>();
            _factory = factory;
        }

        private T Create(Action<MonoBehaviour> returnToPool) 
        {
            T newItem = (T)_factory.Create();
            newItem.Init(returnToPool);
            newItem.gameObject.SetActive(false);
            return newItem;
        }

        public T Get(Action<MonoBehaviour> returnToPool)
        {
            return (T)(_items.Count > 0 ? _items.Dequeue() : Create(returnToPool));
        }

        public void Return(MonoBehaviour item) 
        {         
            item.gameObject.SetActive(false);
            _items.Enqueue((IPoolItem)item);
        }
    }

    public class CubeFactory : IPoolObjectFactory
    {
        private readonly Cube _prefab;

        public CubeFactory(Cube prefab)
        {
            _prefab = prefab;
        }

        public IPoolItem Create()
        {
            IPoolItem item = MonoBehaviour.Instantiate(_prefab);
            return item;
        }
    }

    public interface IPoolItem
    {
        public void Init(Action<MonoBehaviour> action);
    }

    public interface IPoolObjectFactory
    {
        public IPoolItem Create();
    }
}