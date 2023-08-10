using System;
using System.Collections;
using UnityEngine;

namespace CircleMenu
{
    public class DeviceOrientationManager : MonoBehaviour
    {
        public static event Action<DeviceOrientation> OnDeviceOrientationChange;
        private static DeviceOrientation deviceOrientation;
        private const float CHECK_DELAY = 0.1f;

        private void Start()
        {
            StartCoroutine(CheckForChange());
        }

#if UNITY_EDITOR
        public void Update()
        {
            if (Input.GetKeyDown("l"))
            {
                OnDeviceOrientationChange(DeviceOrientation.LandscapeLeft);
            }

            if (Input.GetKeyDown("p"))
            {
                OnDeviceOrientationChange(DeviceOrientation.Portrait);
            }
        }
#endif

        private static IEnumerator CheckForChange()
        {
            deviceOrientation = Input.deviceOrientation;

            while (true)
            {
                if (Input.deviceOrientation == DeviceOrientation.LandscapeLeft || Input.deviceOrientation == DeviceOrientation.Portrait)
                {
                    if (deviceOrientation != Input.deviceOrientation)
                    {
                        deviceOrientation = Input.deviceOrientation;

                        OnDeviceOrientationChange?.Invoke(deviceOrientation);
                    }
                }

                yield return new WaitForSeconds(CHECK_DELAY);
            }
            // ReSharper disable once IteratorNeverReturns
        }
    }
}