using UnityEngine;
using UnityEngine.UI;

namespace Menu.UI
{
    using System;
    using Utility;

    public class PlayGif : MonoBehaviour
    {
        public event Action OnPlayedAll;
        
        [SerializeField]
        private Texture2D[] gifFrames;

        [SerializeField]
        private RawImage rawImage;

        [SerializeField]
        private float frameRate = 60f;

        [SerializeField]
        private bool isPlayOnEnable;
        
        [SerializeField]
        private bool isLoop = true;

        private Timer _timer;
        
        private int _currentFrameIndex;

        private bool _isCanPLay;

        private bool _isInitialized;
        
        private void OnEnable()
        {
            if (isPlayOnEnable)
            {
                Play();
            }
        }

        private void OnDisable() => _isCanPLay = false;

        private void Update()
        {
            if (_isCanPLay == false)
            {
                return;
            }
            
            if (_timer.AddTime(Time.deltaTime) == false)
            {
                return;
            }
            
            SetNewFrame();
        }

        private void SetNewFrame()
        {
            ++_currentFrameIndex;

            if (_currentFrameIndex < gifFrames.Length)
            {
                rawImage.texture = gifFrames[_currentFrameIndex];

                return;
            }

            OnPlayedAll?.Invoke();

            if (isLoop)
            {
                _currentFrameIndex %= gifFrames.Length;

                rawImage.texture = gifFrames[_currentFrameIndex];

                return;
            }

            _isCanPLay = false;
        }

        private void Initialize()
        {
            if (_isInitialized)
            {
                return;
            }
            
            _isInitialized = true;
            
            _timer = new Timer(1f / frameRate);
        }
        
        public void Play()
        {
            _isCanPLay = IsCanPLay();

            if (_isCanPLay == false)
            {
                return;
            }

            Initialize();
            
            _currentFrameIndex = 0;

            rawImage.texture = gifFrames[0];
        }

        private bool IsCanPLay()
        {
            return rawImage != null && gifFrames.Length != 0;
        }
    }
}
