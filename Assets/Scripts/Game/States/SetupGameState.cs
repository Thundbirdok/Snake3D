namespace Game.States
{
    using Game.Field;
    using Game.Snake;
    using UnityEngine;

    public class SetupGameState : State
    {
        [SerializeField]
        private GameStateMachine stateMachine;
        
        [SerializeField]
        private PlayingState playingState;

        [SerializeField]
        private Field field;

        [SerializeField]
        private Snake snake;

        public override void Enable()
        {
            base.Enable();

            snake.Setup();
            field.Setup();

            stateMachine.SetState(playingState);
        }
    }
}
