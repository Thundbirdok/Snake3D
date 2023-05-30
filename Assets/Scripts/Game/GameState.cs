using UnityEngine;

namespace Game
{
    using Field;
    using Snake;

    public class GameState : MonoBehaviour
    {
        [SerializeField]
        private Field field;

        [SerializeField]
        private Snake snake;

        private void OnEnable()
        {
            snake.OnSettedNewPosition += Check;
        }

        private void OnDisable()
        {
            snake.OnSettedNewPosition -= Check;
        }

        private void Check()
        {
            CheckAppleHit();
        }
        
        private void CheckAppleHit()
        {
            if (field.Apple.position == snake.HeadTargetPosition)
            {
                //snake.Grow();
                field.SetAppleNewPosition();
            }
        }

        // private void CheckWallHit()
        // {
        //     if (field.Wall.Position == snake.Head.Position)
        //     {
        //         snake.Die();
        //     }
        // }

        // private void CheckSnakeHit()
        // {
        //     for (var i = 1; i < snake.Body.Count; i++)
        //     {
        //         if (snake.Head.Position == snake.Parts[i].transform)
        //         {
        //             snake.Die();
        //         }
        //     }
        // }
    }
}
