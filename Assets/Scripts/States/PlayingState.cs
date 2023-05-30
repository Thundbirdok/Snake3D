namespace States
{
    using Field;
    using Snake;
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
            CheckSnakeHit();
        }
        
        private void CheckAppleHit()
        {
            if (Mathf.Abs(field.Apple.position.x - snake.HeadTargetPosition.x) > 0.1f)
            {
                return;
            }
            
            if (Mathf.Abs(field.Apple.position.y - snake.HeadTargetPosition.y) > 0.1f)
            {
                return;
            }
            
            if (Mathf.Abs(field.Apple.position.z - snake.HeadTargetPosition.z) > 0.1f)
            {
                return;
            }

            snake.Grow();
            field.SetAppleNewPosition();
        }

        // private void CheckWallHit()
        // {
        //     if (field.Wall.Position == snake.Head.Position)
        //     {
        //         snake.Die();
        //     }
        // }

        private void CheckSnakeHit()
        {
            for (var i = 1; i < snake.PartsTargetPosition.Count; i++)
            {
                if (snake.HeadTargetPosition == snake.PartsTargetPosition[i])
                {
                    stateMachine.SetState(gameOverState);
                }
            }
        }
    }
}
