using System;

namespace Sweet_And_Salty_Studios
{
    [Serializable]
    public class MusicTrack : Sound
    {
        public MUSIC_TRACK_TYPE MusicTrackType;

        public bool IsLooping;

        public override void SetAudioSource()
        {
            base.SetAudioSource();

            audioSource.loop = IsLooping;
        }
    }
}

