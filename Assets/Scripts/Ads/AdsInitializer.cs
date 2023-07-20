namespace Ads
{
    using UnityEngine;
    //using Unity.Services.Core;

    public class AdsInitializer : MonoBehaviour
    {
        private const string ANDROID_APP_ID = "5353335";
        private const string IOS_APP_ID = "5353335";
        
        private async void Start()
        {
            //await UnityServices.InitializeAsync();
            
            //IronSource.Agent.init(GetID());
        }

        private static string GetID()
        {
            return Application.platform == RuntimePlatform.Android ? ANDROID_APP_ID : IOS_APP_ID;
        }
    }
}
