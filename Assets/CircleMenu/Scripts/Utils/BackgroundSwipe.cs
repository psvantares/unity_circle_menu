using UnityEngine;
using UnityEngine.UI;

namespace CircleMenu
{
    public class BackgroundSwipe : MonoBehaviour
    {
        [SerializeField]
        private Sprite portraitImage;

        [SerializeField]
        private Sprite landscapeImage;

        private Image img;

        private void Start()
        {
            img = GetComponent<Image>();
        }

        private void OnEnable()
        {
            DeviceOrientationManager.OnDeviceOrientationChange += SetOrientation;
        }

        private void OnDisable()
        {
            DeviceOrientationManager.OnDeviceOrientationChange -= SetOrientation;
        }

        private void SetOrientation(DeviceOrientation orientation)
        {
            StopAllCoroutines();

            switch (orientation)
            {
                case DeviceOrientation.Portrait:
                    Portrait();
                    break;
                case DeviceOrientation.PortraitUpsideDown:
                    Portrait();
                    break;
                case DeviceOrientation.LandscapeLeft:
                    Landscape();
                    break;
                case DeviceOrientation.LandscapeRight:
                    Landscape();
                    break;
                case DeviceOrientation.FaceUp:
                    break;
            }
        }

        private void Portrait()
        {
            img.sprite = portraitImage;
        }

        private void Landscape()
        {
            img.sprite = landscapeImage;
        }
    }
}