using UnityEngine;

namespace Menu.UI
{
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;

    public class MenuUi : MonoBehaviour
    {
        [SerializeField]
        private Button play;

        private void OnEnable()
        {
            play.onClick.AddListener(Play);
        }

        private void OnDisable()
        {
            play.onClick.RemoveListener(Play);
        }
        
        private void Play() => SceneManager.LoadSceneAsync("Game");
    }
}

