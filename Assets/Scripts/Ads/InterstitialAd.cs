using System.Threading.Tasks;

namespace Ads
{
    using UnityEngine;
    
    public class InterstitialAd : MonoBehaviour
    {
        private async void Awake()
        {
            await Task.Delay(5);
            
            //LoadAd();
            
            await Task.Delay(5);
            
            ShowAd();
        }

        public void ShowAd()
        {
            //IronSource.Agent.showInterstitial();
            
            //LoadAd();
        }

        //private void LoadAd() => IronSource.Agent.loadInterstitial();
    }
}