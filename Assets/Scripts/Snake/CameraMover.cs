using UnityEngine;

namespace Snake
{
    using System;

    [Serializable]
    public class CameraMover
    {
        [field: SerializeField]
        public Transform CameraTarget { get; private set; }

        private Snake _snake;
        
        public void Construct(Snake snake) => _snake = snake;

        public void Move()
        {
            var cameraTarget = CameraTarget.transform;

            cameraTarget.localPosition = Vector3.MoveTowards
            (
                cameraTarget.localPosition,
                _snake.Parts[0].transform.localPosition + _snake.Direction.forward,
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
