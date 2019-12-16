namespace Sweet_And_Salty_Studios
{
    public class AudioManager : Singelton<AudioManager>
    {
        #region VARIABLES

        public MusicTrack[] MusicTracks;
        public Sfx[] SoundEffects;

        #endregion VARIABLES

        #region PROPERTIES

        #endregion PROPERTIES

        #region UNITY_FUNCTIONS

        private void Awake()
        {
            CreateSounds();
        }

        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTIONS

        private void CreateSounds()
        {
            for(int i = 0; i < MusicTracks.Length; i++)
            {
                MusicTracks[i].SetAudioSource();
            }
        }

        public void PlayMusicTrack(MUSIC_TRACK_TYPE type)
        {
            for(int i = 0; i < MusicTracks.Length; i++)
            {
                if(MusicTracks[i].MusicTrackType == type)
                {
                    MusicTracks[i].Play();
                }
            }
        }

        public void PlaySfx()
        {

        }

        #endregion CUSTOM_FUNCTIONS
    }
}