using UnityEngine;

namespace Menu.UI
{
    using DG.Tweening;
    using Utility;

    public class ShowAnimatedTip : MonoBehaviour
    {
        [SerializeField]
        private PlayGif tip;

        [SerializeField]
        private RectTransform rectTransform;
        
        [SerializeField]
        private Timer showTipTimer;

        private bool _isInitialize;
        
        private bool _isShowing;
        
        private Tween _showingTip;
        
        private void OnEnable()
        {
            Initialize();
            
            tip.gameObject.SetActive(false);
            tip.OnPlayedAll += HideTip;
        }

        private void OnDisable()
        {
            tip.OnPlayedAll -= HideTip;
        }

        private void Update()
        {
            if (_isInitialize == false)
            {
                return;
            }
            
            if (_isShowing)
            {
                return;
            }

            if (showTipTimer.AddTime(Time.deltaTime))
            {
                ShowTip();
            }
        }

        private void Initialize()
        {
            if (_isInitialize)
            {
                return;
            }
            
            _isInitialize = true;
            
            _showingTip = rectTransform
                .DOScale(1, 0.2f).From(0)
                .OnPlay(SetActiveTip)
                .OnComplete(PlayTip)
                .OnRewind(SetInactiveTip)
                .SetAutoKill(false);
        }
        
        private void ShowTip()
        {
            _isShowing = true;

            _showingTip.PlayForward();
        }
        
        private void HideTip()
        {
            _showingTip.PlayBackwards();
        }

        private void SetActiveTip()
        {
            tip.gameObject.SetActive(true);
        }
        
        private void SetInactiveTip()
        {
            _isShowing = false;
            tip.gameObject.SetActive(false);
        }

        private void PlayTip()
        {
            tip.Play();
        }
    }
}
