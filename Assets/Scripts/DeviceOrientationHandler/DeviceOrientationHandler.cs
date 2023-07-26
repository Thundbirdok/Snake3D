namespace DeviceOrientationHandler
{
    using System;
    using System.Threading.Tasks;
    using NaughtyAttributes;
    using UnityEngine;

    public class DeviceOrientationHandler : MonoBehaviour
    {
        public event Action OnDeviceOrientationChanged;

        public DeviceOrientation Orientation { get; private set; } = DeviceOrientation.Unknown;
        
        [SerializeField]
        private UnityEngine.Camera mainCamera;
        
        [Tooltip("If aspect lower threshold it is portrait")]
        [field: SerializeField]
        public float HorizontalToVerticalFovThreshold { get; private set; } = 0.5627f;

        private async void Start() 
        {
            //Aspect on start always 1, better to wait cam initialization, but do not know how
            await Task.Yield();

            Orientation = DeviceOrientation.Unknown;
            
            if (Application.isEditor)
            {
                UpdateOrientationInEditor();
                
                return;
            }

            UpdateOrientation();
        }

        private void OnRectTransformDimensionsChange()
        {
            if (Application.isEditor == false)
            {
                UpdateOrientation();
            }
        }
        
        [Button("Update Orientation")]
        private void UpdateOrientationInEditor()
        {
            if (mainCamera.aspect < HorizontalToVerticalFovThreshold)
            {
                UpdateOrientation(DeviceOrientation.Portrait);
                
                return;
            }

            UpdateOrientation(DeviceOrientation.LandscapeLeft);
        }

        private void UpdateOrientation()
        {
            switch (Input.deviceOrientation)
            {
                case DeviceOrientation.Portrait:
                case DeviceOrientation.PortraitUpsideDown:
                    UpdateOrientation(DeviceOrientation.Portrait);

                    break;

                case DeviceOrientation.LandscapeLeft:
                case DeviceOrientation.LandscapeRight:
                case DeviceOrientation.Unknown:
                case DeviceOrientation.FaceUp:
                case DeviceOrientation.FaceDown:
                default:
                    UpdateOrientation(DeviceOrientation.LandscapeLeft);
                    
                    break;
            }
        }
        
        private void UpdateOrientation(DeviceOrientation newOrientation)
        {
            if (Orientation == newOrientation)
            {
                return;
            }
            
            Orientation = newOrientation;
            
            OnDeviceOrientationChanged?.Invoke();
        }
    }
}