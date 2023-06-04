namespace Game.States
{
    using Game.UI;
    using UnityEngine;

    public class GameOverState : State
    {
        [SerializeField]
        private GameStateMachine stateMachine;
        
        [SerializeField]
        private SetupGameState setupState;
        
        [SerializeField]
        private GameOver gameOverScreen;
        
        public override void Enable()
        {
            base.Enable();
            
            gameOverScreen.gameObject.SetActive(true);
            
            gameOverScreen.OnTryAgain += OnTryAgain;
        }

        public override void Disable()
        {
            base.Disable();
            
            gameOverScreen.gameObject.SetActive(false);
            
            gameOverScreen.OnTryAgain -= OnTryAgain;
        }

        private void OnTryAgain() => stateMachine.SetState(setupState);
    }
}
