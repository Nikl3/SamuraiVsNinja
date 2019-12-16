using System;
using UnityEngine;

namespace Sweet_And_Salty_Studios
{
    [Serializable]
    public abstract class Sound
    {
        protected AudioSource audioSource;

        public AudioClip AudioClip;
        [Range(0, 1)] public float Volume;
        [Range(-3, 3)] public float Pitch;

        public virtual void SetAudioSource()
        {
            if(audioSource != null)
            {
                return;
            }

            audioSource = new GameObject("New Sound").AddComponent<AudioSource>();

            audioSource.volume = Volume;
            audioSource.pitch = Pitch;
            audioSource.clip = AudioClip;
        }

        public void Play()
        {
            audioSource.Play();
        }
    }
}

