using UnityEngine;

namespace My_Utils.CameraShake
{
    [ExecuteAlways]
    public class MakeShake : MonoBehaviour
    {
        public bool simulate;
        [Space]
        [Space]

        [Tooltip("The mode that move will start, by trigger or by seconds. (Trigger need be activated by code, calling 'StartShake')")]
        public StartStopMode startMode = StartStopMode.Trigger;

        [Tooltip("Time in seconds until shake starts.")]
        [ConditionalShow("startMode", StartStopMode.Seconds)]
        public float timeToStart;

        [Space]
        public ShakeSettings shakeSettings;

        private bool hasStarted;

        private void Start()
        {
            if (startMode == StartStopMode.Seconds)
            {
                Invoke("StartShake", timeToStart);
            }
        }

        public void StartShake()
        {
            if (!hasStarted)
            {
                hasStarted = true;
                CameraShaker.Instance.ShakeOnce(shakeSettings);
            }
        }

        public void Update()
        {
            if (!Application.isPlaying)
            {
                if (simulate)
                {
                    CameraShaker.Instance.ShakeOnce(shakeSettings);
                    simulate = false;
                }
            }
        }
    }
}