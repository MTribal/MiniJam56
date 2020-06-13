using UnityEngine;

namespace My_Utils
{
    public class DebugTools : MonoBehaviour
    {
        [Tooltip("Mark if when you want to use the script.")]
        public bool activated;

        private bool paused;
        private bool slowed;

        [Header("Keys")]
        [Tooltip("The keyboard key that will pause the game.")]
        public KeyCode pauseKey = KeyCode.P;
        [Tooltip("The keyboard key that will slow down the game.")]
        public KeyCode slowDownKey = KeyCode.S;

        [Header("Values")]
        [Tooltip("Percentage that time will be. A lower value will be slower")]
        [Range(0, 1)]
        public float slowDownPerc = 0.1f;

        private void Update()
        {
            if (activated)
            {
                VerifyInput();
            }
        }

        private void VerifyInput()
        {
            if (Input.GetKeyDown(pauseKey))
            {
                if (paused)
                {
                    paused = false;
                    if (slowed)
                    {
                        Time.timeScale = 1f * slowDownPerc;
                    }
                    else
                    {
                        Time.timeScale = 1f;
                    }
                }
                else
                {
                    paused = true;
                    Time.timeScale = 0f;
                }
            }
            if (Input.GetKeyDown(slowDownKey) && !paused)
            {
                if (slowed)
                {
                    slowed = false;
                    Time.timeScale = 1f;
                }
                else
                {
                    slowed = true;
                    Time.timeScale = 1f * slowDownPerc;
                }
            }
        }
    }
}
