using UnityEngine;

namespace My_Utils
{
    /// <summary>
    /// Base class of all pooled objects.
    /// Inherit from this to be a pooled object. (This is a MonoBehaviour)
    /// </summary>
    public class PooledObject : MonoBehaviour
    {
        private string _tag;
        public string PoolTag
        {
            get
            {
                return _tag;
            }
            set
            {
                if (_tag == null || _tag == "")
                {
                    _tag = value;
                }
            }
        }


        /// <summary>
        /// Is called when spawning from pool.
        /// </summary>
        public virtual void OnSpawnFromPool()
        {

        }


        /// <summary>
        /// Is called when putting(disabling the gameObject) to pool.
        /// </summary>
        public virtual void OnPutToPool()
        {

        }
    }
}
