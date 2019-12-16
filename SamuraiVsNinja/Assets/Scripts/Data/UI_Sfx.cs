using System;
using UnityEngine;

namespace Sweet_And_Salty_Studios
{
    [Serializable]
    public class UI_Sfx : Sound
    {
        public UI_SFX_TYPE Type;

        public override void SetAudioSource(Transform parent)
        {
            base.SetAudioSource(parent);
        }
    }
}

