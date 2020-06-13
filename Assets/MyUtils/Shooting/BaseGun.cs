using System.Collections;
using UnityEngine;

namespace My_Utils.Shooting
{
    public class BaseGun : MonoBehaviour
    {
        [Tooltip("The projectile that the gun will shoot.")]
        public BaseProjectile projectilePrefab;

        [Tooltip("Mark to modify tag to make it unique.")]
        public bool _uniqueTag;
        [Tooltip("The tag of the pool that will be created.")]
        public string poolTag;

        [Tooltip("Mark to automatically calculate a size to the pool based on the projectile duration and on timeBetweenShots")]
        public bool automaticSize;

        [Tooltip("The quantity of objects to store in the pool that will be created to the projectile prefab.")]
        public int poolSize;

        [Space]
        [Tooltip("The shoot position.")]
        public Transform shootPos;
        [Tooltip("The minimum time between each shoot.")]
        public float timeBetweenShots;

        private bool _canShoot = true;
        private ObjectPooler _objectPooler;

        private void Start()
        {
            _objectPooler = ObjectPooler.Instance;

            int size = automaticSize ? _CalculatePoolSize() : poolSize;
            poolTag = MakeTagUnique(poolTag);
            _objectPooler.CreatePool(poolTag, size, projectilePrefab);
        }

        private string MakeTagUnique(string baseTag)
        {
            return baseTag + GetInstanceID();
        }

        public int _CalculatePoolSize()
        {
            float divisor = timeBetweenShots != 0 ? timeBetweenShots : 0.01f;
            return (int)(projectilePrefab.duration / divisor) + 1;
        }

        public void Shoot(float gunAngle)
        {
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, gunAngle);
            if (_canShoot)
            {
                _canShoot = false;
                StartCoroutine(EnableShoot());
                _objectPooler.SpawnFromPool(poolTag, shootPos.position, transform.rotation, false);
            }
        }

        private IEnumerator EnableShoot()
        {
            yield return new WaitForSeconds(timeBetweenShots);
            _canShoot = true;
        }
    }
}