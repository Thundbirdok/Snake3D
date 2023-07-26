namespace DeviceOrientationHandler.Camera
{
    using Cinemachine;
    using UnityEngine;

    public class OrientationCamera : MonoBehaviour
    {
        [SerializeField]
        private DeviceOrientationHandler handler;
        
        [SerializeField] 
        private CinemachineVirtualCamera virtualCamera;

        [SerializeField]
        private float verticalFovOnLandscapeOrientation = 70.49793f;
        
        private float _portraitFov;
        private float _landscapeFov;

        private bool _isInitialized;
        
        private void OnEnable()
        {
            Initialize();
            
            handler.OnDeviceOrientationChanged += SetOrientation;
        }

        private void OnDisable()
        {
            handler.OnDeviceOrientationChanged -= SetOrientation;
        }

        private void Initialize()
        {
            if (_isInitialized)
            {
                return;
            }
            
            _isInitialized = true;
            
            _landscapeFov = verticalFovOnLandscapeOrientation;

            _portraitFov = Camera.HorizontalToVerticalFieldOfView
            (
                verticalFovOnLandscapeOrientation,
                handler.HorizontalToVerticalFovThreshold
            );
        }

        private void SetOrientation()
        {
            switch (handler.Orientation)
            {
                case DeviceOrientation.LandscapeLeft:
                    SetLandscape();
                    break;
                
                case DeviceOrientation.Portrait:
                    SetPortrait();
                    break;
            }
        }

        private void SetLandscape() => virtualCamera.m_Lens.FieldOfView = _landscapeFov;

        private void SetPortrait() => virtualCamera.m_Lens.FieldOfView = _portraitFov;
    }
}
