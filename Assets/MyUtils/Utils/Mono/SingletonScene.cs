using UnityEngine;


namespace My_Utils
{
    public class SingletonScene<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                    _instance.transform.parent = null;
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
