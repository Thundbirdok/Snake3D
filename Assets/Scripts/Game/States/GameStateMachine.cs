namespace Game.States
{
    using UnityEngine;

    public class GameStateMachine : MonoBehaviour
    {
        private State _state;
        
        [SerializeField]
        private SetupGameState setupGameState;

        private void Start()
        {
            _state = setupGameState;
            SetState(setupGameState);
        }

        public void SetState(State state)
        {
            _state.Disable();

            _state = state;
            
            _state.Enable();
        }
    }
}
