namespace Ui
{
    using UnityEngine;
    using UnityEngine.UI;
    
    public class OrientationResolution : MonoBehaviour
    {
        [SerializeField] 
        private CanvasScaler canvasScaler;
        
        [SerializeField]
        private Vector2 landscapeResolution;

        private Vector2 _portraitResolution;

        private void Awake() => _portraitResolution = new Vector2(landscapeResolution.y, landscapeResolution.x);

        private void OnRectTransformDimensionsChange()
        {
            switch (Input.deviceOrientation)
            {
                case DeviceOrientation.Portrait:
                    canvasScaler.referenceResolution = _portraitResolution;
                    canvasScaler.matchWidthOrHeight = 0;
                    
                    break;
                
                case DeviceOrientation.PortraitUpsideDown:
                    canvasScaler.referenceResolution = _portraitResolution;
                    canvasScaler.matchWidthOrHeight = 0;
                    
                    break;
                
                case DeviceOrientation.LandscapeLeft:
                    canvasScaler.referenceResolution = landscapeResolution;
                    canvasScaler.matchWidthOrHeight = 1;        
                    
                    break;
                
                case DeviceOrientation.LandscapeRight:
                    canvasScaler.referenceResolution = landscapeResolution;
                    canvasScaler.matchWidthOrHeight = 1;
                    
                    break;
            };
        }
    }
}
