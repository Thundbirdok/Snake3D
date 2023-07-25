namespace Ui
{
    using System.Threading.Tasks;
    using Cinemachine;
    using NaughtyAttributes;
    using UnityEngine;
    using UnityEngine.UI;

    public class OrientationResolution : MonoBehaviour
    {
        [SerializeField]
        private CanvasScaler canvasScaler;

        [SerializeField]
        private Vector2 landscapeResolution;

        [SerializeField]
        private CinemachineVirtualCamera virtualCamera;

        [Range(0.1f,2)]
        [Tooltip("If aspect lower threshold it is portrait")]
        public float horizontalToVerticalFovThreshold = 0.5627f;
        
        private Vector2 _portraitResolution;

        private void Start() 
        {
            _portraitResolution = new Vector2
            (
                landscapeResolution.y,
                landscapeResolution.x
            );

            if (Application.isEditor)
            {
                SetResolutionInEditor();
                
                return;
            }

            SetResolution();
        }

        [Button("Update Resolution")]
        private async void SetResolutionInEditor()
        {
            //Aspect on start always 1, better to wait cam initialization, but do not know how
            await Task.Yield();
            
            if (virtualCamera.m_Lens.Aspect < horizontalToVerticalFovThreshold)
            {
                SetPortrait();
                
                return;
            }

            SetLandscape();
        }

        private void OnRectTransformDimensionsChange()
        {
            if (Application.isEditor == false)
            {
                SetResolution();
            }
        }
        
        private void SetResolution()
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

        private void SetLandscape()
        {
            canvasScaler.referenceResolution = landscapeResolution;
            canvasScaler.matchWidthOrHeight = 1;
        }

        private void SetPortrait()
        {
            canvasScaler.referenceResolution = _portraitResolution;
            canvasScaler.matchWidthOrHeight = 0;
        }
    }
}
