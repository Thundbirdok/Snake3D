using UnityEngine;

namespace UI
{
    using System;
    using UnityEngine.UI;

    public class GameOver : MonoBehaviour
    {
        public event Action OnTryAgain; 
        
        [SerializeField]
        private Button tryAgain;
        
        private void OnEnable()
        {
            tryAgain.onClick.AddListener(TryAgain);
        }

        private void OnDisable()
        {
            tryAgain.onClick.RemoveListener(TryAgain);
        }
        
        private void TryAgain() => OnTryAgain?.Invoke();
    }
}
