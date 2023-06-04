namespace Game.UI
{
    using System;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;

    public class GameOver : MonoBehaviour
    {
        public event Action OnTryAgain; 
        
        [SerializeField]
        private Button tryAgain;
        
        [SerializeField]
        private Button menu;
        
        private void OnEnable()
        {
            tryAgain.onClick.AddListener(TryAgain);
            menu.onClick.AddListener(LoadMenu);
        }

        private void OnDisable()
        {
            tryAgain.onClick.RemoveListener(TryAgain);
            menu.onClick.RemoveListener(LoadMenu);
        }

        private void TryAgain() => OnTryAgain?.Invoke();

        private void LoadMenu() => SceneManager.LoadSceneAsync("Menu");
    }
}
