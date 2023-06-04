using UnityEngine;

namespace Snake
{
    using System;
    using Cinemachine;

    [Serializable]
    public class CameraMover
    {
        [SerializeField]
        public Transform cameraTarget;

        [SerializeField]
        private CinemachineVirtualCamera virtualCamera;

        [SerializeField]
        private Camera camera;
        
        private Snake _snake;
        
        public void Construct(Snake snake)
        {
            _snake = snake;
        }

        public void Setup()
        {
            SetCameraNearHead();
        }
        
        private void SetCameraNearHead()
        {
            if (_snake == null)
            {
                return;
            }
            
            if (_snake.Head == null)
            {
                return;
            }

            var cameraTargetPosition = _snake.Head.localPosition;
            var delta = cameraTarget.localPosition - cameraTargetPosition;
            //var delta = cameraTargetPosition - cameraTarget.localPosition;
            
            cameraTarget.localPosition = cameraTargetPosition;
            cameraTarget.localRotation = _snake.Head.localRotation;
            
            virtualCamera.PreviousStateIsValid = false;
            //virtualCamera.ForceCameraPosition(cameraTarget, );
            virtualCamera.OnTargetObjectWarped(cameraTarget, delta);
        }

        public void Move()
        {
            cameraTarget.localPosition = Vector3.MoveTowards
            (
                cameraTarget.localPosition,
                _snake.Parts[0].transform.localPosition + _snake.Forward,
                Time.fixedDeltaTime / _snake.MoveDelay
            );

            cameraTarget.localRotation = Quaternion.RotateTowards
            (
                cameraTarget.localRotation, 
                _snake.Parts[0].transform.localRotation,
                Time.fixedDeltaTime / _snake.MoveDelay * 180
            );
        }
    }
}
