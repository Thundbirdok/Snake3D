using System;
using UnityEngine;

namespace Snake
{
    [Serializable]
    public class SnakeDirectionController
    {
        [SerializeField]
        private SnakeInputHandler snakeInputHandler;

        private Snake _snake;

        private Vector3 _direction;
    
        public void Construct(Snake snake)
        {
            _snake = snake;
        
            _direction = _snake.Direction.forward;
        
            snakeInputHandler.OnUp += RotateUp;
            snakeInputHandler.OnDown += RotateDown;
            snakeInputHandler.OnLeft += RotateLeft;
            snakeInputHandler.OnRight += RotateRight;
        }

        public void Dispose()
        {
            snakeInputHandler.OnUp -= RotateUp;
            snakeInputHandler.OnDown -= RotateDown;
            snakeInputHandler.OnLeft -= RotateLeft;
            snakeInputHandler.OnRight -= RotateRight;
        }

        public void UpdateDirection() => _snake.Direction.LookAt(_direction);

        private void RotateUp() => SetNewRotation(0, 1);
        private void RotateDown() => SetNewRotation(0, -1);
        private void RotateLeft() => SetNewRotation(-1, 0);
        private void RotateRight() => SetNewRotation(1, 0);

        private void SetNewRotation(int x, int y)
        {
            var direction = _snake.Direction;
            var newZAxis = direction.up * y + direction.right * x;

            ClampDirection(ref newZAxis);

            if (direction.forward + newZAxis == Vector3.zero)
            {
                return;
            }

            _direction = newZAxis;
        }

        private void ClampDirection(ref Vector3 newLocalZAxis)
        {
            newLocalZAxis.x = (int)Mathf.Clamp(newLocalZAxis.x, -1, 1);
            newLocalZAxis.y = (int)Mathf.Clamp(newLocalZAxis.y, -1, 1);
            newLocalZAxis.z = (int)Mathf.Clamp(newLocalZAxis.z, -1, 1);
        }
    }
}
