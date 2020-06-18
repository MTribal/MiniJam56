using System.Collections.Generic;
using UnityEngine;

namespace My_Utils
{
    public class ObjectPooler : MonoBehaviour
    {
        [System.Serializable]
        public class Pool
        {
            public string tag;
            public int size;
            public PooledObject prefab;
        }


        [Tooltip("You can create pools via inspector here.")]
        [SerializeField] private Pool[] _inspectorPools = default;

        private readonly Dictionary<string, Queue<PooledObject>> _dictPool = new Dictionary<string, Queue<PooledObject>>();

        public static ObjectPooler Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        private void Start()
        {
            for (int i = 0; i < _inspectorPools.Length; i++)
            {
                CreatePool(_inspectorPools[i].tag, _inspectorPools[i].size, _inspectorPools[i].prefab);
            }
        }


        /// <summary>
        /// Return the quantity of objects remaining in a pool. Return '0' if tag not exist.
        /// </summary>
        /// <param name="poolTag">The tag of the pool.</param>
        /// <returns></returns>
        public int QuantityInThePool(string poolTag)
        {
            if (_dictPool.ContainsKey(poolTag))
                return _dictPool[poolTag].Count;
            else
                return 0;
        }


        /// <summary>
        /// Return if a pool is empty or no. If poolTag doesn't exist, return false.
        /// </summary>
        /// <param name="poolTag">The tag of the pool you want to check.</param>
        /// <returns></returns>
        public bool PoolIsEmpty(string poolTag)
        {
            return !_dictPool.ContainsKey(poolTag) || _dictPool[poolTag].Count == 0;
        }


        /// <summary>
        /// Create a new pool of PooledObjects. Return false if poolTag already exist.
        /// </summary>
        /// <param name="poolTag">The tag of the pool that you want to create.</param>
        /// <param name="size">The size of the pool that you want to create.</param>
        /// <param name="prefab">The model object of the pool.</param>
        /// <returns></returns>
        public bool CreatePool(string poolTag, int size, PooledObject prefab)
        {
            if (_dictPool.ContainsKey(poolTag))
                return false;

            string name = poolTag + "_Pool";
            Transform parent = new GameObject(name).transform;

            Queue<PooledObject> queue = new Queue<PooledObject>();
            for (int i = 0; i < size; i++)
            {
                PooledObject obj = Instantiate(prefab, parent);
                obj.gameObject.SetActive(false);
                obj.PoolTag = poolTag;
                queue.Enqueue(obj);
            }
            _dictPool[poolTag] = queue;

            return true;
        }


        /// <summary>
        /// It's like Instantiate, but with pools. Check if pool is empty before call this.
        /// </summary>
        /// <param name="poolTag">The tag of the pool that you want to instantiate.</param>
        /// <param name="position">The position to instantiate.</param>
        /// <param name="rotation">The rotation to instantiate.</param>
        /// <param name="reuseAutomatically">If the object will automatically go to queue again, or you will call PutIntoPool() when you want. If you want totally control of your objects, set it to false.</param>
        /// <returns></returns>
        public T SpawnFromPool<T>(string poolTag, Vector3 position, Quaternion rotation, bool reuseAutomatically) where T : PooledObject
        {
            return (T)SpawnFromPool(poolTag, position, rotation, reuseAutomatically);
        }


        /// <summary>
        /// It's like Instantiate, but with pools. Check if pool is empty before call this.
        /// </summary>
        /// <param name="poolTag">The tag of the pool that you want to instantiate.</param>
        /// <param name="position">The position to instantiate.</param>
        /// <param name="rotation">The rotation to instantiate.</param>
        /// <param name="reuseAutomatically">If the object will automatically go to queue again, or you will call PutIntoPool() when you want. If you want totally control of your objects, set it to false.</param>
        /// <returns></returns>
        public PooledObject SpawnFromPool(string poolTag, Vector3 position, Quaternion rotation, bool reuseAutomatically)
        {
            if (!_dictPool.ContainsKey(poolTag))
            {
                Debug.LogError("Pool dict not contains tag " + poolTag);
                return null;
            }

            PooledObject instantiatedObj = _dictPool[poolTag].Dequeue();
            instantiatedObj.transform.position = position;
            instantiatedObj.transform.rotation = rotation;
            instantiatedObj.gameObject.SetActive(true);
            instantiatedObj.OnSpawnFromPool();

            if (reuseAutomatically)
                _dictPool[poolTag].Enqueue(instantiatedObj);

            return instantiatedObj;
        }


        /// <summary>
        /// It's like Destroy() but with pools.
        /// Disable a gameObject of the scene and put into the queue. 
        /// </summary>
        /// <param name="pooledObject">The object that you want to disable.</param>
        public void PutToPool(PooledObject pooledObject)
        {
            pooledObject.gameObject.SetActive(false);
            pooledObject.OnPutToPool();

            if (!_dictPool[pooledObject.PoolTag].Contains(pooledObject))
                _dictPool[pooledObject.PoolTag].Enqueue(pooledObject);
        }
    }
}
