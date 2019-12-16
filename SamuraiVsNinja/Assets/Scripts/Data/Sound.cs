using System;
using UnityEngine;
using UnityEngine.Audio;

namespace Sweet_And_Salty_Studios
{
    [Serializable]
    public abstract class Sound
    {
        protected AudioSource audioSource;
        public AudioMixerGroup mixerGroupOutput;

        public AudioClip AudioClip;
        [Range(0, 1)] public float Volume = 0.5f;
        [Range(-3, 3)] public float Pitch = 1f;

        public virtual void SetAudioSource(Transform parent)
        {
            if(audioSource != null)
            {
                return;
            }

            audioSource = new GameObject("New Sound").AddComponent<AudioSource>();

            audioSource.transform.SetParent(parent);

            audioSource.playOnAwake = false;

            audioSource.outputAudioMixerGroup = mixerGroupOutput;

            audioSource.volume = Volume;
            audioSource.pitch = Pitch;
            audioSource.clip = AudioClip;
        }

        public void Play()
        {
            audioSource.Play();
        }

        public void Stop()
        {
            audioSource.Stop();
        }
    }
}

