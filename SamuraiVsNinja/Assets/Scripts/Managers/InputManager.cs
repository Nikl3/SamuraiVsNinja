﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Sweet_And_Salty_Studios
{
    public class InputManager : Singelton<InputManager>
    {
        #region VARIABLES

        [Space]
        [Header("Connected Input Devices")]
        [SerializeField] List<Connected_InputDevice> connectedInputDevices = new List<Connected_InputDevice>();

        public InputActions InputActions;

        #endregion VARIABLES

        #region PROPERTIES

        public Vector2 GetMovementInput
        {
            get
            {
                return InputActions.Player.Movement.ReadValue<Vector2>();
            }
        }

        #endregion PROPERTIES

        #region UNITY_FUNCTIONS

        private void Awake()
        {
            Initialize();
        }

        private void OnEnable()
        {
            InputSystem.onDeviceChange += OnDeviceChanged;

            InputActions.Enable();
        }

        private void OnDisable()
        {
            InputSystem.onDeviceChange -= OnDeviceChanged;

            InputActions.Disable();
        }

        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTION

        private void Initialize()
        {
            InputActions = new InputActions();

            var devices = InputSystem.devices;

            connectedInputDevices.Clear();

            for(int i = 0; i < devices.Count; i++)
            {
                connectedInputDevices.Add(new Connected_InputDevice(devices[i]));
            }
        }

        private Connected_InputDevice GetConnectedDevice(InputDevice inputDevice)
        {
            for(int i = 0; i < connectedInputDevices.Count; i++)
            {
                if(connectedInputDevices[i].InputDevice == inputDevice)
                {
                    return connectedInputDevices[i];
                }
            }

            return null;
        }

        private void OnDeviceChanged(InputDevice inputDevice, InputDeviceChange inputDeviceChange)
        {
            Debug.LogWarning($"{inputDevice.displayName}: {inputDeviceChange}");

            switch(inputDeviceChange)
            {
                case InputDeviceChange.Added:

                    if(connectedInputDevices.Contains(GetConnectedDevice(inputDevice)))
                    {
                        Debug.LogError($"{GetConnectedDevice(inputDevice).Name} same device already connected!");
                        return;
                    }

                    connectedInputDevices.Add(new Connected_InputDevice(inputDevice));

                    break;
                case InputDeviceChange.Removed:

                    connectedInputDevices.Remove(GetConnectedDevice(inputDevice));

                    break;
                case InputDeviceChange.Disconnected:

                    break;
                case InputDeviceChange.Reconnected:

                    break;
                case InputDeviceChange.Enabled:

                    break;
                case InputDeviceChange.Disabled:

                    break;
                case InputDeviceChange.UsageChanged:

                    break;
                case InputDeviceChange.ConfigurationChanged:

                    break;
                case InputDeviceChange.Destroyed:

                    break;
                default:

                    break;
            }
        }

        public void LockInputs(bool isLocked)
        {
            if(isLocked)
            {
                InputActions.Player.Disable();
            }
            else
            {
                InputActions.Player.Enable();
            }
        }

        #endregion CUSTOM_FUNCTIONS
    }
}