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

            cameraTarget.localPosition = cameraTargetPosition;
            cameraTarget.localRotation = _snake.Head.localRotation;
            
            virtualCamera.PreviousStateIsValid = false;
            
            virtualCamera.OnTargetObjectWarped(cameraTarget, delta);
        }

        public void Move()
        {
            cameraTarget.localPosition = Vector3.MoveTowards
            (
                cameraTarget.localPosition,
                _snake.Head.localPosition + _snake.Forward,
                Time.fixedDeltaTime / _snake.MoveDelay
            );

            cameraTarget.localRotation = Quaternion.RotateTowards
            (
                cameraTarget.localRotation, 
                _snake.Head.localRotation,
                Time.fixedDeltaTime / _snake.MoveDelay * 180
            );
        }
    }
}
