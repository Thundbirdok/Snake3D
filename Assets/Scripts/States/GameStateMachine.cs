using UnityEngine;

namespace States
{
    public class GameStateMachine : MonoBehaviour
    {
        private State _state;
        
        [SerializeField]
        private PlayingState playingState;

        private void Start()
        {
            _state = playingState;
            SetState(playingState);
        }

        public void SetState(State state)
        {
            _state.Disable();

            _state = state;
            
            _state.Enable();
        }
    }
}
