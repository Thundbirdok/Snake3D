namespace Camera
{
    using System.Threading.Tasks;
    using Cinemachine;
    using NaughtyAttributes;
    using UnityEngine;

    public class OrientationCamera : MonoBehaviour
    {
        [SerializeField] 
        private CinemachineVirtualCamera virtualCamera;

        [Range(0.1f,2)]
        [Tooltip("If aspect lower threshold it is portrait")]
        public float horizontalToVerticalFovThreshold = 0.5627f;

        [SerializeField]
        private float verticalFovOnLandscapeOrientation = 70.49793f;
        
        private float _portraitFov;
        private float _landscapeFov;    
        
        private void Start()
        {
            SetOrientationFovValues();

            if (Application.isEditor)
            {
                SetFovInEditor();
                
                return;
            }
            
            SetFov();
        }

        private void OnRectTransformDimensionsChange()
        {
            if (Application.isEditor == false)
            {
                SetFov();
            }
        }

        private void SetOrientationFovValues()
        {
            _landscapeFov = verticalFovOnLandscapeOrientation;

            _portraitFov = Camera.HorizontalToVerticalFieldOfView
            (
                verticalFovOnLandscapeOrientation,
                horizontalToVerticalFovThreshold
            );
        }

        [Button("Update Resolution")]
        private async void SetFovInEditor()
        {
            //Aspect on start always 1, better to wait cam initialization, but do not know how
            await Task.Yield();

            if (IsPortraitAspectCamera())
            {
                SetPortrait();
                
                return;
            }
            
            SetLandscape();
        }

        private void SetFov()
        {
            switch (Input.deviceOrientation)
            {
                case DeviceOrientation.Portrait:
                    SetPortrait();

                    break;

                case DeviceOrientation.PortraitUpsideDown:
                    SetPortrait();

                    break;

                case DeviceOrientation.LandscapeLeft:
                    SetLandscape();
                    
                    break;

                case DeviceOrientation.LandscapeRight:
                    SetLandscape();

                    break;

                case DeviceOrientation.Unknown:
                    break;

                case DeviceOrientation.FaceUp:
                    break;

                case DeviceOrientation.FaceDown:
                    break;

                default:
                    break;
            }
        }

        private void SetLandscape() => virtualCamera.m_Lens.FieldOfView = _landscapeFov;

        private void SetPortrait() => virtualCamera.m_Lens.FieldOfView = _portraitFov;

        private bool IsPortraitAspectCamera() =>
            virtualCamera.m_Lens.Aspect < horizontalToVerticalFovThreshold;
    }
}
