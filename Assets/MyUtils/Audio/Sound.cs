using UnityEngine;
using UnityEngine.Audio;

namespace My_Utils.Audio
{
    [System.Serializable]
    public class Sound
    {
        public string name;

        public AudioMixerGroup outputMixer;
        public AudioClip clip;

        [HideInInspector]
        public AudioSource source;

        [Range(0f, 1f)]
        public float volume = 1f;

        [Range(0.1f, 3f)]
        public float pitch = 1f;

        public bool loop;

        public Sound()
        {
            volume = 1f;
            pitch = 1f;
        }
    }
}
