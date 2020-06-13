using UnityEngine;

namespace My_Utils
{
    /// <summary>
    /// Store LoadType and TransitionType of all transitions. SceneLoader uses this parameters.
    /// </summary>
    public class TransitionManager : MonoBehaviour
    {
        public static TransitionManager Instance { get; private set; }

        public LoadType LastLoadType { get; private set; }

        public TransitionType LastTransitionType { get; private set; }

        public float LastAnimationRate { get; private set; }

        private void Awake()
        {
            Undestructable();
        }

        private void Undestructable()
        {
            if (Instance == null)
            {
                Instance = this;
                if (transform.parent != null)
                {
                    transform.parent = null;
                }
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void Atualize(LoadType loadType, TransitionType transitionType, float animationRate)
        {
            LastLoadType = loadType;
            LastTransitionType = transitionType;
            LastAnimationRate = animationRate;
        }
    }
}
