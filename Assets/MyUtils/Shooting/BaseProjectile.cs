using UnityEngine;

namespace My_Utils.Shooting
{
    /// <summary>
    /// Base class of all projectiles.
    /// </summary>
    public class BaseProjectile : PooledObject
    {
        [Tooltip("The maximum duration of the projectile.")]
        public float duration;
        [Tooltip("The speed of the projectile.")]
        public float speed;
        [Tooltip("The amount of damage that this projectile causes.")]
        public float damage;

        [Tooltip("Layers that the projectile wants to hit, like enemies or player.")]
        public LayerMask targetLayers;
        [Tooltip("Layers that can destroy the projectile. (Walls, ground, ...")]
        public LayerMask solidLayers;


        protected bool shutingDown;

        public override void OnSpawnFromPool()
        {
            base.OnSpawnFromPool();
            shutingDown = false;
            Invoke("DestroyItSelf", duration);
        }

        protected virtual void FixedUpdate()
        {
            if (!shutingDown)
                Move();
        }

        protected virtual void Move()
        {
            transform.Translate(Vector2.right * speed * Time.fixedDeltaTime, Space.Self);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (targetLayers.Contains(collision.gameObject.layer))
            {
                OnTriggerWithTarget(collision);
            }
            if (solidLayers.Contains(collision.gameObject.layer))
            {
                OnTriggerWithSolid();
            }
        }


        /// <summary>
        /// Projectile has collide with target.
        /// </summary>
        /// <param name="collision">The target object that has collide with this projectile.</param>
        protected virtual void OnTriggerWithTarget(Collider2D collision)
        {
            if (!gameObject.activeSelf) // Projectile has been put to pool. Prevents area damage
                return;
        }


        /// <summary>
        /// Projectile has collide with solid.
        /// </summary>
        protected virtual void OnTriggerWithSolid()
        {
            DestroyItSelf();
        }


        /// <summary>
        /// Call this to "destroy" the gameObject.
        /// </summary>
        protected virtual void DestroyItSelf()
        {
            ObjectPooler.Instance.PutToPool(this);
        }
    }
}
