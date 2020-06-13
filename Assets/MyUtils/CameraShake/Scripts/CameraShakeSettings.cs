using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace My_Utils.CameraShake
{
    [System.Serializable]
    public class ShakeSettings
    {
        [Tooltip("The intensity of the shake. It is recommended that you use ScaleMagnitude to alter the magnitude of a shake.")]
        public float Magnitude;

        [Tooltip("Roughness of the shake. It is recommended that you use ScaleRoughness to alter the roughness of a shake.")]
        public float Roughness;

        [Tooltip("How long to fade in the shake, in seconds.")]
        public float fadeInDuration;

        [Tooltip("How long to fade out the shake, in seconds")]
        public float fadeOutDuration;
    }
}
