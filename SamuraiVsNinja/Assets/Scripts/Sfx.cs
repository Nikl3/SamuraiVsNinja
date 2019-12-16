using System;

namespace Sweet_And_Salty_Studios
{
    [Serializable]
    public class Sfx : Sound
    {
        public SFX_TYPE SfxType;

        public override void SetAudioSource()
        {
            base.SetAudioSource();
        }
    }
}

