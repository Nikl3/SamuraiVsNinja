using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

namespace Sweet_And_Salty_Studios
{
    public class AudioManager : Singelton<AudioManager>
    {
        #region VARIABLES

        [Space]
        [Header("Audio Mixer")]
        public AudioMixer AudioMixer;

        [Space]
        [Header("Sounds")]
        public MusicTrack[] MusicTracks;
        public Sfx[] SoundEffects;
        public UI_Sfx[] UI_SoundEffects;

        private MusicTrack currentlyPlayingTrack;
        private bool isFading;

        #endregion VARIABLES

        #region PROPERTIES

        #endregion PROPERTIES

        #region UNITY_FUNCTIONS

        private void Start()
        {
            CreateSounds();
        }

        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTIONS

        private void CreateSounds()
        {
            var index = 0;

            var MusicTracks_Parent = new GameObject("MusicTracks").transform;
            var SoundEffects_Parent = new GameObject("SoundEffects").transform;
            var UI_SoundEffects_Parent = new GameObject("UI_SoundEffects").transform;

            for(; index < MusicTracks.Length; index++)
            {
                MusicTracks[index].SetAudioSource(MusicTracks_Parent);
            }

            for(index = 0; index < SoundEffects.Length; index++)
            {
                SoundEffects[index].SetAudioSource(SoundEffects_Parent);
            }

            for(index = 0; index < UI_SoundEffects.Length; index++)
            {
                UI_SoundEffects[index].SetAudioSource(UI_SoundEffects_Parent);
            }
        }

        private IEnumerator ISwitchToTrack(MusicTrack musicTrack, bool saveTrackProgress, float fadeOutDuration = 0.5f, float fadeInDuration = 0.5f)
        {
            if(currentlyPlayingTrack != null)
            {
                yield return IFadeVolume("Music", 0f, fadeOutDuration);

                if(saveTrackProgress)
                {
                    currentlyPlayingTrack.Pause();
                }
                else
                {
                    currentlyPlayingTrack.Stop();
                }
            }

            currentlyPlayingTrack = musicTrack;

            currentlyPlayingTrack.Play();

            yield return IFadeVolume("Music", 1f, fadeInDuration);
        }

        private IEnumerator IFadeVolume(string channelParameterName, float targetVolume, float fadeDuration)
        {
            yield return new WaitWhile(() => isFading);

            var startChannelVolume = GetChannelValue(channelParameterName);
            var startLerpTime = Time.unscaledTime;
            var timeSinceStarted = Time.unscaledTime - startLerpTime;
            var percentToComplete = timeSinceStarted / fadeDuration;

            targetVolume = LinearToDecibelValue(targetVolume);

            if(targetVolume != startChannelVolume)
            {
                isFading = true;

                while(percentToComplete <= 1f)
                {
                    timeSinceStarted = Time.unscaledTime - startLerpTime;
                    percentToComplete = timeSinceStarted / fadeDuration;

                    var currentVolume = Mathf.Lerp(startChannelVolume, targetVolume, percentToComplete);
                    AudioMixer.SetFloat(channelParameterName, currentVolume);

                    yield return null;
                }

                isFading = false;
            }
        }

        private float DecibelToLinearValue(float decibelValue)
        {
            return Mathf.Pow(10.0f, decibelValue / 20.0f);
        }

        private float LinearToDecibelValue(float linearValue)
        {
            return linearValue != 0 ? 20.0f * Mathf.Log10(linearValue) : -80f;
        }

        private float GetChannelValue(string channelName)
        {
            AudioMixer.GetFloat(channelName, out float value);
            return value;
        }

        public void PlayMusicTrack(MUSIC_TRACK_TYPE type, bool savePreviousTrackProcess)
        {
            for(int i = 0; i < MusicTracks.Length; i++)
            {
                if(MusicTracks[i].Type == type)
                {
                    StartCoroutine(ISwitchToTrack(MusicTracks[i], savePreviousTrackProcess));
                    return;
                }
            }
        }
    
        public void PlaySfx(SFX_TYPE type)
        {
            for(int i = 0; i < SoundEffects.Length; i++)
            {
                if(SoundEffects[i].Type == type)
                {
                    SoundEffects[i].Play();
                    return;
                }
            }
        }

        public void PlayUISfx(UI_SFX_TYPE type)
        {
            for(int i = 0; i < UI_SoundEffects.Length; i++)
            {
                if(UI_SoundEffects[i].Type == type)
                {
                    UI_SoundEffects[i].Play();
                    return;
                }
            }
        }

        public void SetLowPassValue(float newValueInHertz)
        {
            AudioMixer.SetFloat("LowPassValue", newValueInHertz);
        }

        public bool SetAudioMixerChannelValue(string channelParameterName, float value)
        {
            if(AudioMixer.SetFloat(channelParameterName, LinearToDecibelValue(value)))
            {
                return true;
            }

            return false;
        }

        public void FadeChannelVolume(string channelParameterName, float targetVolume, float fadeTime)
        {       
            StartCoroutine(IFadeVolume(channelParameterName, targetVolume, fadeTime));
        }

        #endregion CUSTOM_FUNCTIONS
    }
}