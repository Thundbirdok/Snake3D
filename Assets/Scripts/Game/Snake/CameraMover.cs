namespace Game.Snake
{
    using System;
    using Cinemachine;
    using Game.Snake.PartsPoses;
    using UnityEngine;

    [Serializable]
    public class CameraMover
    {
        [SerializeField]
        public Transform cameraTarget;

        [SerializeField]
        private CinemachineVirtualCamera virtualCamera;

        private SnakePartsPosesHandler _partsPosesHandler;
        private SnakeDirectionController _directionController;
        private SnakePartsMover _snakePartsMover;
        
        public void Construct
        (
            SnakePartsPosesHandler partsPosesHandler,
            SnakeDirectionController directionController,
            SnakePartsMover snakePartsMover
        )
        {
            _partsPosesHandler = partsPosesHandler;
            _directionController = directionController;
            _snakePartsMover = snakePartsMover;
        }

        public void Setup() => SetCameraNearHead();

        public void Move()
        {
            cameraTarget.localPosition = Vector3.MoveTowards
            (
                cameraTarget.localPosition,
                _partsPosesHandler.HeadPosition + _directionController.Forward,
                Time.fixedDeltaTime / _snakePartsMover.Delay
            );

            cameraTarget.localRotation = Quaternion.RotateTowards
            (
                cameraTarget.localRotation, 
                _partsPosesHandler.HeadRotation,
                Time.fixedDeltaTime / _snakePartsMover.Delay * 180
            );
        }

        private void SetCameraNearHead()
        {
            if (_partsPosesHandler == null)
            {
                return;
            }

            var cameraTargetPosition = _partsPosesHandler.HeadPosition;
            var localTargetRotation = _partsPosesHandler.HeadRotation;
            
            var delta = cameraTarget.localPosition - (Vector3)cameraTargetPosition;

            cameraTarget.localPosition = cameraTargetPosition;
            cameraTarget.localRotation = localTargetRotation;

            virtualCamera.PreviousStateIsValid = false;
            
            virtualCamera.OnTargetObjectWarped(cameraTarget, delta);
        }
    }
}
