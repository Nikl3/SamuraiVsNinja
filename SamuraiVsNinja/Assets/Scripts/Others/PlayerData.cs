using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Sweet_And_Salty_Studios
{
    [Serializable]
    public class PlayerData
    {
        [HideInInspector] public string Name;

        public Color IndicatorColor;
        public string InputDeviceName;
        public InputDevice InputDevice;
        public int ID;

        private bool hasInitialized;

        public void Initialize()
        {
            if(hasInitialized)
            {
                return;
            }

            Name = $"Player {ID}";

            hasInitialized = true;
        }

    //    public PlayerData(int id)
    //    {
    //        Name = $"Player: {id}";

        //        ID = id;
        //    }
    }
}

