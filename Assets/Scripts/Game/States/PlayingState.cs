namespace Game.States
{
    using Game.Field;
    using Game.Snake;
    using UnityEngine;

    public class PlayingState : State
    {
        [SerializeField]
        private GameStateMachine stateMachine;
        
        [SerializeField]
        private GameOverState gameOverState;
        
        [SerializeField]
        private Field field;

        [SerializeField]
        private Snake snake;

        public override void Enable()
        {
            base.Enable();
            
            snake.OnSettedNewPosition += Check;
            snake.IsActive = true;
        }

        public override void Disable()
        {
            base.Disable();
            
            snake.IsActive = false;
            snake.OnSettedNewPosition -= Check;
        }

        private void Check()
        {
            CheckAppleHit();

            if (IsSnakeHit() || IsWallsHit())
            {
                stateMachine.SetState(gameOverState);
            }
        }
        
        private void CheckAppleHit()
        {
            var appleLocalPosition = field.AppleSpawner.Apple.localPosition;
            var snakeHeadTargetPosition = snake.HeadTargetPosition;
            
            if (Mathf.Abs(appleLocalPosition.x - snakeHeadTargetPosition.x) > 0.1f)
            {
                return;
            }
            
            if (Mathf.Abs(appleLocalPosition.y - snakeHeadTargetPosition.y) > 0.1f)
            {
                return;
            }
            
            if (Mathf.Abs(appleLocalPosition.z - snakeHeadTargetPosition.z) > 0.1f)
            {
                return;
            }

            field.AppleSpawner.EatApple();
            snake.Grow();
        }

        private bool IsWallsHit()
        {
            var snakeHeadTargetPosition = snake.HeadTargetPosition;
            
            if (snakeHeadTargetPosition.x < 0.5f)
            {
                return true;
            }

            if (snakeHeadTargetPosition.x > field.Size.x - 0.5f)
            {
                return true;
            }
            
            if (snakeHeadTargetPosition.y < 0.5f)
            {
                return true;
            }
            
            if (snakeHeadTargetPosition.y > field.Size.y - 0.5f)
            {
                return true;
            }

            if (snakeHeadTargetPosition.z < 0.5f)
            {
                return true;
            }

            if (snakeHeadTargetPosition.z > field.Size.z - 0.5f)
            {
                return true;
            }

            return false;
        }

        private bool IsSnakeHit()
        {
            for (var i = 1; i < snake.PartsTargetPosition.Count; i++)
            {
                if (snake.HeadTargetPosition == snake.PartsTargetPosition[i])
                {
                    return true;
                }
            }
            
            return false;
        }
    }
}
