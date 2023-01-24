using System;
using System.Collections;
using UnityEngine;

#if OG_ADMOB
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
#endif

namespace OnefallGames
{
    public class AdmobController : MonoBehaviour
    {

#if OG_ADMOB


        [Header("Banner Id")]
#if UNITY_ANDROID
        [SerializeField] private string androidBannerId = "ca-app-pub-1064078647772222/9329609006";
#elif UNITY_IOS
        [SerializeField] private string iOSBannerId = "ca-app-pub-1064078647772222/9329609006";
#endif
        [SerializeField] private AdPosition bannerPosition = AdPosition.Bottom;


        [Header("Interstitial Ad Id")]
#if UNITY_ANDROID
        [SerializeField] private string androidInterstitialAdId = "ca-app-pub-1064078647772222/2139808686";
#elif UNITY_IOS
        [SerializeField] private string iOSInterstitialAdId = "ca-app-pub-1064078647772222/2139808686";
#endif

        [Header("Rewarded Base Video Id")]
#if UNITY_ANDROID
        [SerializeField] private string androidRewardedAdId = "ca-app-pub-1064078647772222/9919321234";

#elif UNITY_IOS
        [SerializeField] private string iOSRewardedAdId = "ca-app-pub-1064078647772222/9919321234";
#endif


        private BannerView bannerView = null;
        private InterstitialAd interstitialAd = null;
        private RewardedAd rewardedAd = null;
        private bool isCompletedRewardedVideo = false;

#endif






        private void Awake()
        {
#if OG_ADMOB
            MobileAds.SetiOSAppPauseOnBackground(true);
            MobileAds.Initialize(HandleInitCompleteAction);

            //Create the singleton rewardedAd.
#if UNITY_ANDROID
            rewardedAd = new RewardedAd(androidRewardedAdId);
#elif UNITY_IOS
            rewardedAd = new RewardedAd(iOSRewardedAdId);
#endif
            //Register events for rewardedAd
            rewardedAd.OnAdClosed += HandleRewardedClosed;
            rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
#endif
        }


#if OG_ADMOB
        private void HandleInitCompleteAction(InitializationStatus initstatus)
        {
            // Callbacks from GoogleMobileAds are not guaranteed to be called on
            // main thread.
            // In this example we use MobileAdsEventExecutor to schedule these calls on
            // the next Update() loop.
            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                Debug.Log("Initialization completed.");
            });
        }
#endif






        /// <summary>
        /// Load and show a banner ad
        /// </summary>
        public void LoadAndShowBanner(float delay)
        {
            StartCoroutine(CRLoadAndShowBanner(delay));
        }
        private IEnumerator CRLoadAndShowBanner(float delay)
        {
            yield return new WaitForSeconds(delay);
#if OG_ADMOB
            // Clean up banner ad before creating a new one.
            if (bannerView != null)
            {
                bannerView.Destroy();
            }

            // Create a 320x50 banner at the top of the screen.
#if UNITY_ANDROID
            bannerView = new BannerView(androidBannerId, AdSize.SmartBanner, bannerPosition);
#elif UNITY_IOS
            bannerView = new BannerView(iOSBannerId, AdSize.SmartBanner, bannerPosition);
#endif
            // Load banner ad.
            bannerView.LoadAd(new AdRequest.Builder().Build());
#endif
        }

        /// <summary>
        /// Hide the current banner ad
        /// </summary>
        public void HideBanner()
        {
#if OG_ADMOB
            if (bannerView != null)
            {
                bannerView.Hide();
            }
#endif
        }






        /// <summary>
        /// Request interstitial ad
        /// </summary>
        public void RequestInterstitial()
        {
#if OG_ADMOB
            // Clean up interstitial ad before creating a new one.
            if (interstitialAd != null)
            {
                interstitialAd.Destroy();
            }

            // Create an interstitial.
#if UNITY_ANDROID
            interstitialAd = new InterstitialAd(androidInterstitialAdId);
#elif UNITY_IOS
            interstitialAd = new InterstitialAd(iOSInterstitialAdId);
#endif
            // Register for ad events.
            interstitialAd.OnAdClosed += HandleInterstitialClosed;

            // Load an interstitial ad.
            interstitialAd.LoadAd(new AdRequest.Builder().Build());
#endif
        }


        /// <summary>
        /// Request rewarded video ad
        /// </summary>
        public void RequestRewardedVideo()
        {
#if OG_ADMOB
            rewardedAd.LoadAd(new AdRequest.Builder().Build());
#endif
        }


        /// <summary>
        /// Determine whether the interstitial ad is ready
        /// </summary>
        /// <returns></returns>
        public bool IsInterstitialReady()
        {
#if OG_ADMOB
            if (interstitialAd.IsLoaded())
            {
                return true;
            }
            else
            {
                RequestInterstitial();
                return false;
            }
#else
            return false;
#endif
        }

        /// <summary>
        /// Show interstitial ad with given delay time
        /// </summary>
        /// <param name="delay"></param>
        public void ShowInterstitial(float delay)
        {
            StartCoroutine(CRShowInterstitial(delay));
        }
        private IEnumerator CRShowInterstitial(float delay)
        {
            yield return new WaitForSeconds(delay);
#if OG_ADMOB
            if (interstitialAd.IsLoaded())
            {
                interstitialAd.Show();
            }
            else
            {
                RequestInterstitial();
            }
#endif
        }



        /// <summary>
        /// Determine whether the rewarded video ad is ready
        /// </summary>
        /// <returns></returns>
        public bool IsRewardedVideoReady()
        {
#if OG_ADMOB
            if (rewardedAd.IsLoaded())
            {
                return true;
            }
            else
            {
                RequestRewardedVideo();
                return false;
            }
#else
            return false;
#endif
        }

        /// <summary>
        /// Show rewarded video ad with given delay time 
        /// </summary>
        /// <param name="delay"></param>
        public void ShowRewardedVideo(float delay)
        {
            StartCoroutine(CRShowRewardedVideoAd(delay));
        }
        IEnumerator CRShowRewardedVideoAd(float delay)
        {
            yield return new WaitForSecondsRealtime(delay);
#if OG_ADMOB
            if (rewardedAd.IsLoaded())
            {
                rewardedAd.Show();
            }
            else
            {
                RequestRewardedVideo();
            }
#endif
        }





#if OG_ADMOB
        //Events callback
        private void HandleInterstitialClosed(object sender, EventArgs args)
        {
            RequestInterstitial();
        }

        private void HandleUserEarnedReward(object sender, Reward args)
        {
            //User watched the whole video
            isCompletedRewardedVideo = true;
        }

        private void HandleRewardedClosed(object sender, EventArgs args)
        {
            //User closed the video
            ServicesManager.Instance.AdManager.OnRewardedVideoClosed(isCompletedRewardedVideo);
            isCompletedRewardedVideo = false;
            RequestRewardedVideo();
        }
#endif
    }
}

