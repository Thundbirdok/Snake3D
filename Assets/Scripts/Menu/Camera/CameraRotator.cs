using UnityEngine;

namespace Menu.Camera
{
    using Cinemachine;

    public class CameraRotator : MonoBehaviour
    {
        [SerializeField]
        private CinemachineVirtualCamera virtualCamera;

        [SerializeField, Range(-1, 1)]
        private float rotationSpeed = 1;
        
        private CinemachineOrbitalTransposer _transposer;

        private void Awake()
        {
            _transposer = virtualCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>();
        }

        private void Update()
        {
            _transposer.m_XAxis.m_InputAxisValue = rotationSpeed;
        }
    }
}
