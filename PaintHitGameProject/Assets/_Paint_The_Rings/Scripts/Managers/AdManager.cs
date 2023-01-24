using System.Collections.Generic;
using UnityEngine;

namespace OnefallGames
{
    public class AdManager : MonoBehaviour
    {

        [Header("Banner Ad config")]
        [SerializeField] private BannerAdType bannerAdType = BannerAdType.NONE;
        [SerializeField] private float showingBannerAdDelay = 0.5f;


        [Header("Interstitial Ad Config")]
        [SerializeField] private List<InterstitialAdConfig> listShowInterstitialAdConfig = new List<InterstitialAdConfig>();

        [Header("Rewarded Video Ad Config")]
        [SerializeField] private float showingRewardedVideoAdDelay = 0.25f;
        [SerializeField] private List<RewardedAdType> listRewardedAdType = new List<RewardedAdType>();

        [Header("AdManager References")]
        [SerializeField] private AdmobController admobController = null;
        [SerializeField] private UnityAdController unityAdController = null;

        private List<int> listShowAdCount = new List<int>();
        private RewardedAdType readyAdType = RewardedAdType.UNITY;

        private bool isCalledback = false;
        private bool isRewarded = false;
        private void OnEnable()
        {
            IngameManager.IngameStateChanged += IngameManager_IngameStateChanged;
        }

        private void OnDisable()
        {
            IngameManager.IngameStateChanged -= IngameManager_IngameStateChanged;
        }
       

        private void Start()
        {
            foreach (InterstitialAdConfig o in listShowInterstitialAdConfig)
            {
                listShowAdCount.Add(o.GameStateCountForShowingAd);
            }

            //Show banner ad
            if (bannerAdType == BannerAdType.ADMOB)
            {
                admobController.LoadAndShowBanner(showingBannerAdDelay);
            }
            if (bannerAdType == BannerAdType.UNITY)
            {
                unityAdController.ShowBanner(showingBannerAdDelay);
            }


            //Request interstitial ads (unity ads auto requests interstitial)
            foreach (InterstitialAdConfig o in listShowInterstitialAdConfig)
            {
                foreach(InterstitialAdType a in o.ListInterstitialAdType)
                {
                    if (a == InterstitialAdType.ADMOB)
                    {
                        admobController.RequestInterstitial();
                    }
                }
            }

            //Request rewarded video (unity ads auto requests rewarded video)
            foreach (RewardedAdType o in listRewardedAdType)
            {
                if (o == RewardedAdType.ADMOB)
                {
                    admobController.RequestRewardedVideo();
                }
            }
        }

        private void Update()
        {
            if (isCalledback)
            {
                isCalledback = false;
                if (isRewarded)
                {
                    IngameManager.Instance.SetContinueGame(); //Revive
                }
                else
                {
                    IngameManager.Instance.GameOver(); ///Game over
                }
            }
        }


        private void IngameManager_IngameStateChanged(IngameState obj)
        {
            for (int i = 0; i < listShowAdCount.Count; i++)
            {
                if (listShowInterstitialAdConfig[i].GameStateForShowingAd == obj)
                {
                    listShowAdCount[i]--;
                    if (listShowAdCount[i] <= 0)
                    {
                        //Reset gameCount 
                        listShowAdCount[i] = listShowInterstitialAdConfig[i].GameStateCountForShowingAd;

                        for (int a = 0; a < listShowInterstitialAdConfig[i].ListInterstitialAdType.Count; a++)
                        {
                            InterstitialAdType type = listShowInterstitialAdConfig[i].ListInterstitialAdType[a];
                            if (type == InterstitialAdType.ADMOB && admobController.IsInterstitialReady())
                            {
                                admobController.ShowInterstitial(listShowInterstitialAdConfig[i].ShowAdDelay);
                                break;
                            }
                            else if (type == InterstitialAdType.UNITY && unityAdController.IsInterstitialReady())
                            {
                                unityAdController.ShowInterstitial(listShowInterstitialAdConfig[i].ShowAdDelay);
                                break;
                            }
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Determines whether rewarded video ad is ready.
        /// </summary>
        /// <returns></returns>
        public bool IsRewardedVideoAdReady()
        {
            for(int i = 0; i < listRewardedAdType.Count; i++)
            {
                if (listRewardedAdType[i] == RewardedAdType.UNITY && unityAdController.IsRewardedVideoReady())
                {
                    readyAdType = RewardedAdType.UNITY;
                    return true;
                }
                else if(listRewardedAdType[i] == RewardedAdType.ADMOB && admobController.IsRewardedVideoReady())
                {
                    readyAdType = RewardedAdType.ADMOB;
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// Show the rewarded video ad with delay time
        /// </summary>
        /// <param name="delay"></param>
        public void ShowRewardedVideoAd()
        {
            if (readyAdType == RewardedAdType.UNITY)
            {
                unityAdController.ShowRewardedVideo(showingRewardedVideoAdDelay);
            }
            else if (readyAdType == RewardedAdType.ADMOB)
            {
                admobController.ShowRewardedVideo(showingRewardedVideoAdDelay);
            }
        }

        public void OnRewardedVideoClosed(bool isFinishedVideo)
        {
            isCalledback = true;
            isRewarded = isFinishedVideo;
        }
    }
}
