using System;
using UnityEngine;

namespace Sweet_And_Salty_Studios
{
    [Serializable]
    public class MusicTrack : Sound
    {
        public MUSIC_TRACK_TYPE Type;

        public bool IsLooping;

        public override void SetAudioSource(Transform parent)
        {
            base.SetAudioSource(parent);

            audioSource.loop = IsLooping;
        }
    }
}

