using System;
using UnityEngine;

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

    private void RotateUp()
    {
        SetNewRotation(0, 1);
    }

    private void RotateDown()
    {
        SetNewRotation(0, -1);
    }
    
    private void RotateLeft()
    {
        SetNewRotation(-1, 0);
    }
    
    private void RotateRight()
    {
        SetNewRotation(1, 0);
    }

    private void SetNewRotation(int x, int y)
    {
        var newZAxis = y != 0 ? _snake.Direction.transform.up * y : _snake.Direction.transform.right * x;

        var newLocalZAxis = newZAxis;//_snake.Direction.transform.InverseTransformVector(newZAxis);

        newLocalZAxis.x = Mathf.Abs(newLocalZAxis.x) < 0.1f ? 0 : Mathf.Clamp(newLocalZAxis.x, -1, 1);
        newLocalZAxis.y = Mathf.Abs(newLocalZAxis.y) < 0.1f ? 0 : Mathf.Clamp(newLocalZAxis.y, -1, 1);
        newLocalZAxis.z = Mathf.Abs(newLocalZAxis.z) < 0.1f ? 0 : Mathf.Clamp(newLocalZAxis.z, -1, 1);
        
        _direction = newLocalZAxis;
        
        Debug.Log(_direction);
    }
}
