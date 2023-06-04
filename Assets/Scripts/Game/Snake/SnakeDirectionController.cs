namespace Game.Snake
{
    using System;
    using UnityEngine;

    [Serializable]
    public class SnakeDirectionController
    {
        [SerializeField]
        private SnakeInputHandler snakeInputHandler;

        public Vector3 Forward { get; private set; }
        public Vector3 Up { get; private set; }
        public Vector3 Right { get; private set; }
        
        private Vector3 _tmpForward;
        private Vector3 _tmpUp;
        private Vector3 _tmpRight;
    
        public void Construct()
        {
            UpdateDirection();
        
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

        public void Setup()
        {
            _tmpForward = Vector3.forward;
            _tmpUp = Vector3.up;
            _tmpRight = Vector3.right;
        }
        
        public void UpdateDirection()
        {
            Forward = _tmpForward;
            Up = _tmpUp;
            Right = _tmpRight;
        }

        private void RotateUp() => SetNewRotation(0, 1);
        private void RotateDown() => SetNewRotation(0, -1);
        private void RotateLeft() => SetNewRotation(-1, 0);
        private void RotateRight() => SetNewRotation(1, 0);

        private void SetNewRotation(int x, int y)
        {
            var newZAxis = Up * y + Right * x;
            
            if (Forward + newZAxis == Vector3.zero)
            {
                return;
            }

            Vector3 newYAxis;
            Vector3 newXAxis;

            if (newZAxis.y == 0)
            {
                newYAxis = Vector3.up;
                newXAxis = newZAxis.z != 0 ? Vector3.right * newZAxis.z : Vector3.forward * -newZAxis.x;
            }
            else
            {
                newYAxis = y != 0 ? Forward * -y : Up;
                newXAxis = x != 0 ? Forward * -x : Right;
            }

            _tmpForward = newZAxis;
            _tmpUp = newYAxis;
            _tmpRight = newXAxis;
        }
    }
}
