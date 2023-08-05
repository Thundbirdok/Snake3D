namespace Game.Snake
{
    using System;
    using Cinemachine;
    using UnityEngine;

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

            var cameraTargetPosition = _snake.Head.Position;
            var localTargetRotation = _snake.Head.Rotation;
            
            var delta = cameraTarget.localPosition - cameraTargetPosition;

            cameraTarget.localPosition = cameraTargetPosition;
            cameraTarget.localRotation = localTargetRotation;

            virtualCamera.PreviousStateIsValid = false;
            
            virtualCamera.OnTargetObjectWarped(cameraTarget, delta);
        }

        public void Move()
        {
            cameraTarget.localPosition = Vector3.MoveTowards
            (
                cameraTarget.localPosition,
                _snake.Head.Position + _snake.Forward,
                Time.fixedDeltaTime / _snake.MoveDelay
            );

            cameraTarget.localRotation = Quaternion.RotateTowards
            (
                cameraTarget.localRotation, 
                _snake.Head.Rotation,
                Time.fixedDeltaTime / _snake.MoveDelay * 180
            );
        }
    }
}
