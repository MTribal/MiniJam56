using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace My_Utils
{
    /// <summary>
    /// Load scenes and trigger transitions effects when it exists.
    /// </summary>
    public class SceneLoader : MonoBehaviour
    {
        [HideInInspector]
        public static SceneLoader Instance { get; private set; }

        [Tooltip("Mark to animate the initial scene of the game.")]
        public bool animateFirstTransition;

        public TransitionSettings transitionSettings;

        #region Transition Effect
        [Tooltip("Mark if you have a transition animation effect.")]
        public bool hasTransitionEffect;

        [ConditionalShow("hasTransitionEffect", true)]
        [Tooltip("Transition animator with 'exit_scene' and 'start_scene' trigger.")]
        public Animator transitionAnimator;
        #endregion

        private void Awake()
        {
            Instance = this;

            if (TransitionManager.Instance != null)
            {
                // Animate if LastTransitionType == true
                transitionAnimator.speed = TransitionManager.Instance.LastAnimationRate;
                transitionAnimator.SetBool("start_scene", TransitionManager.Instance.LastTransitionType == TransitionType.Animated && animateFirstTransition);
            }
            else
            {
                // If true: animate
                transitionAnimator.speed = transitionSettings.animationRate;
                transitionAnimator.SetBool("start_scene", animateFirstTransition);
            }
        }

        #region ReloadScene


        /// <summary>
        /// Return the atual scene build index.
        /// </summary>
        public int AtualBuildIndex
        {
            get
            {
                return SceneManager.GetActiveScene().buildIndex;
            }
        }


        /// <summary>
        /// Reload the scene.
        /// </summary>
        /// <param name="animateExit">If the end of the atual scene will has animation.</param>
        public void ReloadScene(bool animateExit) // Use default transition duration
        {
            TransitionType transitionType = animateExit ? TransitionType.Animated : TransitionType.NotAnimated;

            StartCoroutine(Load(SceneManager.GetActiveScene().buildIndex, transitionSettings.animationRate, animateExit, LoadType.Reload, transitionType));
        }


        /// <summary>
        /// Reload the scene.
        /// </summary>
        /// <param name="animationRate">How fast the animation will play.</param>
        /// <param name="animateExit">If the end of the atual scene will has animation.</param>
        public void ReloadScene(float animationRate, bool animateExit) // Use specified transition duration
        {
            TransitionType transitionType = animateExit ? TransitionType.Animated : TransitionType.NotAnimated;

            StartCoroutine(Load(SceneManager.GetActiveScene().buildIndex, animationRate, animateExit, LoadType.Reload, transitionType));
        }

        #endregion

        #region LoadScene

        // BuildIndex

        /// <summary>
        /// Load a scene by build index
        /// </summary>
        /// <param name="buildIndex">Build index of scene that will be loaded.</param>
        /// <param name="animateExit">If the end of the atual scene will has animation.</param>
        public void LoadScene(int buildIndex, bool animateExit)
        {
            TransitionType transitionType = animateExit ? TransitionType.Animated : TransitionType.NotAnimated;

            StartCoroutine(Load(buildIndex, transitionSettings.animationRate, animateExit, LoadType.Transition, transitionType));
        }

        /// <summary>
        /// Load a scene by build index
        /// </summary>
        /// <param name="buildIndex">Build index of scene that will be loaded.</param>
        /// <param name="animationRate">How fast transition animation will play.</param>
        /// <param name="animateExit">If the end of the atual scene will has animation.</param>
        public void LoadScene(int buildIndex, float animationRate, bool animateExit)
        {
            TransitionType transitionType = animateExit ? TransitionType.Animated : TransitionType.NotAnimated;

            StartCoroutine(Load(buildIndex, animationRate, animateExit, LoadType.Transition, transitionType));
        }

        // SceneName

        /// <summary>
        /// Load a scene by build scene name
        /// </summary>
        /// <param name="sceneName">Name of scene that will be loaded.</param
        /// <param name="animateExit">If the end of the atual scene will has animation.</param>
        public void LoadScene(string sceneName, bool animateExit)
        {
            TransitionType transitionType = animateExit ? TransitionType.Animated : TransitionType.NotAnimated;

            StartCoroutine(Load(sceneName, transitionSettings.animationRate, animateExit, LoadType.Transition, transitionType));
        }

        /// <summary>
        /// Load a scene by scene name
        /// </summary>
        /// <param name="sceneName">Name of scene that will be loaded.</param>
        /// <param name="animationRate">How fast transition animation will play.</param>
        /// <param name="animateExit">If the end of the atual scene will has animation.</param>
        public void LoadScene(string sceneName, float animationRate, bool animateExit)
        {
            TransitionType transitionType = animateExit ? TransitionType.Animated : TransitionType.NotAnimated;

            StartCoroutine(Load(sceneName, animationRate, animateExit, LoadType.Transition, transitionType));
        }

        #endregion

        #region Load

        /// <summary>
        /// Load a level by buildIndex
        /// </summary>
        /// <param name="buildIndex">Build index of scene that will load.</param>
        /// <param name="transitionTime">How much time transition will durate.</param>
        /// <param name="animateExit">If the end of atual scene will has animation.</param>
        /// <param name="loadType">The load type of the scene that will load.</param>
        /// <param name="transitionType">The transition type of the scene that will load.</param>
        private IEnumerator Load(int buildIndex, float animationRate, bool animateExit, LoadType loadType, TransitionType transitionType)
        {
            AtualizeTransitionManager(loadType, transitionType, animationRate);
            if (animateExit)
            {
                transitionAnimator.speed = animationRate;
                StartExitAnimation();
            }

            yield return new WaitForSeconds(transitionSettings.animationDuration / animationRate);

            SceneManager.LoadScene(buildIndex);
        }

        /// <summary>
        /// Load a level by scene name
        /// </summary>
        /// <param name="sceneName">Name of scene that will load.</param>
        /// <param name="transitionTime">How much time transition will durate.</param>
        /// <param name="animateExit">If the end of atual scene will has animation.</param>
        /// <param name="loadType">The load type of the scene that will load.</param>
        /// <param name="transitionType">The transition type of the scene that will load.</param>
        private IEnumerator Load(string sceneName, float animationRate, bool animateExit, LoadType loadType, TransitionType transitionType)
        {
            AtualizeTransitionManager(loadType, transitionType, animationRate);
            if (animateExit)
            {
                transitionAnimator.speed = animationRate;
                StartExitAnimation();
            }

            yield return new WaitForSeconds(transitionSettings.animationDuration / animationRate);

            SceneManager.LoadScene(sceneName);
        }
        #endregion

        private void StartExitAnimation()
        {
            transitionAnimator.SetBool("exit_scene", true);
        }

        private void AtualizeTransitionManager(LoadType loadType, TransitionType transitionType, float animationRate)
        {
            TransitionManager.Instance.Atualize(loadType, transitionType, animationRate);
        }
    }
}
