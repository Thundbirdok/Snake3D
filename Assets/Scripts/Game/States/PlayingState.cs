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
            if (Mathf.Abs(field.Apple.localPosition.x - snake.HeadTargetPosition.x) > 0.1f)
            {
                return;
            }
            
            if (Mathf.Abs(field.Apple.localPosition.y - snake.HeadTargetPosition.y) > 0.1f)
            {
                return;
            }
            
            if (Mathf.Abs(field.Apple.localPosition.z - snake.HeadTargetPosition.z) > 0.1f)
            {
                return;
            }

            snake.Grow();
            field.SetAppleNewPosition();
        }

        private bool IsWallsHit()
        {
            if (snake.HeadTargetPosition.x < 0.5f)
            {
                return true;
            }

            if (snake.HeadTargetPosition.x > field.Size.x - 0.5f)
            {
                return true;
            }
            
            if (snake.HeadTargetPosition.y < 0.5f)
            {
                return true;
            }
            
            if (snake.HeadTargetPosition.y > field.Size.y - 0.5f)
            {
                return true;
            }

            if (snake.HeadTargetPosition.z < 0.5f)
            {
                return true;
            }

            if (snake.HeadTargetPosition.z > field.Size.z - 0.5f)
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
