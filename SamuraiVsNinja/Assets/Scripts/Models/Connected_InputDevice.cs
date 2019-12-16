using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Sweet_And_Salty_Studios
{
    [Serializable]
    public class Connected_InputDevice
    {
        public string Name;
        public int ID;
        [TextArea(0, 200)] public string Description = "There is no Description.";

        public InputDevice InputDevice
        {
            get;
            private set;
        }

        public Connected_InputDevice(InputDevice inputDevice)
        {
            InputDevice = inputDevice;

            Name = inputDevice.displayName;
            ID = inputDevice.deviceId;
            Description = inputDevice.description.ToString();
        }
    }
}

