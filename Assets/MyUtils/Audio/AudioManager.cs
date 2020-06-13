using System;
using UnityEngine;

namespace My_Utils.Audio
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;

        public Sound[] sounds;

        private void Awake()
        {
            if (Instance == null)
            {
                transform.parent = null;
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            foreach (Sound sound in sounds)
            {
                sound.source = gameObject.AddComponent<AudioSource>();
                sound.source.volume = sound.volume;
                sound.source.pitch = sound.pitch;
                sound.source.clip = sound.clip;
                sound.source.loop = sound.loop;
                sound.source.outputAudioMixerGroup = sound.outputMixer;
            }
        }


        /// <summary>
        /// Play a sound by a given name. Return false if sound not found.
        /// </summary>
        public bool PlaySound(string soundName)
        {
            Sound sound = Array.Find(sounds, s => s.name == soundName);
            if (sound == null)
            {
                Debug.Log("Sound" + sound.name + " not found.");
                return false;
            }

            sound.source.Play();
            return true;
        }
    }
}
