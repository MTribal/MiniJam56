using System;
using UnityEngine;

namespace My_Utils.Audio
{
    public class AudioManager : SingletonPermanent<AudioManager>
    {
        public Sound[] sounds;

        protected override void Awake()
        {
            base.Awake();
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
        /// Play a sound by a given name. 
        /// </summary>
        public void PlaySound(string soundName)
        {
            GetSound(soundName).source.Play();
        }


        /// <summary>
        /// Stop a sound by a given name. 
        /// </summary>
        public void StopSound(string soundName)
        {
            GetSound(soundName).source.Stop();
        }


        /// <summary>
        /// Pause a sound by a given name. 
        /// </summary>
        public void PauseSound(string soundName)
        {
            GetSound(soundName).source.Pause();
        }


        /// <summary>
        /// Set a sound pitch by a given name. 
        /// </summary>
        public void SetPitch(string soundName, float value)
        {
            GetSound(soundName).source.pitch = value;
        }


        /// <summary>
        /// Set a sound volume by a given name. 
        /// </summary>
        public void SetVolume(string soundName, float value)
        {
            GetSound(soundName).source.volume = value;
        }


        private Sound GetSound(string soundName)
        {
            Sound sound = Array.Find(sounds, s => s.name == soundName);
            if (sound == null)
                Debug.Log("Sound" + sound.name + " not found.");

            return sound;
        }
    }
}
