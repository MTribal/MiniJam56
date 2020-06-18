using UnityEngine;

namespace My_Utils
{
    public class SingletonPermanent<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        protected virtual void Awake()
        {
            if (_instance != null) Destroy(gameObject);
        }

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                    _instance.transform.parent = null;
                    DontDestroyOnLoad(_instance.gameObject);
                }

                return _instance;
            }
        }

        public static bool InstanceIsNull
        {
            get
            {
                return _instance == null;
            }
        }
    }
}
