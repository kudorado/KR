using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_ADS
using UnityEngine.Advertisements;
#endif
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace KR
{
	public class UnityAds : KR.ScriptableSingleton<UnityAds>
	{
#if UNITY_EDITOR

		[MenuItem("KR/Monetization/Installed/Unity/Ads/Init")]
		public static void CreateInitiator()
		{
			KR.Scriptable.CreateInitiator<UnityAdsInitiator>();
		}

		[MenuItem("KR/Monetization/Installed/Unity/Ads/Settings")]
		public static void Advetisement()
		{
			KR.Scriptable.CreateAsset<UnityAds>("Assets/KR/Monetization/Installed/UnityAds/Resources/");
		}

#endif
		//public bool usingServiceAds = true;
		public string unityIdAndroid = "1747849", unityIdIos = "1747850";

		public static bool isRewardVideoAvaiable
		{
			get
			{
#if UNITY_ADS
            return Advertisement.IsReady(rewardPlacementId); 
#endif
				return false;

			}
		}
		public static bool isVideoAvaiable
		{
			get
			{
#if UNITY_ADS

            return Advertisement.IsReady(videoPlacementId);
#endif
				return false;
			}
		}

#if UNITY_ADS

#endif

		public const string rewardPlacementId = "rewardedVideo";
		public const string videoPlacementId = "video";

		public override void OnInitialized()
		{
			base.OnInitialized();
			KR.Monetization.onAdInitialize += OnAdInitialize;

		}
		private static void OnAdInitialize()
		{

			Debug.Log("EaUnityAds Initialized.".color(Color.blue));


			try
			{


				KR.Monetization.onRewardVideoShowing += ShowVideo;
				KR.Monetization.onVideoShowing += ShowVideo;
				KR.Monetization.onVideoValidate = () =>
				{
					return isVideoAvaiable;
				};
				KR.Monetization.onRewardVideoValidate = () =>
				{
					return isRewardVideoAvaiable;
				};

#if UNITY_ADS
            if (!Advertisement.isInitialized)
            {
                string uid = "";
#if UNITY_ANDROID
                uid = instance.unityIdAndroid;
#endif
#if UNITY_IOS
            uid = instance.unityIdIos;
#endif
                Advertisement.Initialize(uid);
            }

                            //if (KR.Monetization.instance.showDebug) Debug.Log("Unity ads status: " + (Advertisement.isInitialized ? " Initialized." : "Not ready."));
#endif

			}
			catch (Exception e)
			{
				Debug.Log("Unity ads service is not ready, please turn on unity ads then try again.\n " + e);
			}

		}
		public static void ShowVideo()
		{
#if UNITY_ADS

                    if (Advertisement.IsReady(videoPlacementId))
                        Advertisement.Show();
#endif
		}
		private static void ShowVideo(Action onFinished, Action onFailed = null, Action onSkipped = null)
		{
#if UNITY_ADS

                    if (Advertisement.IsReady(rewardPlacementId))
                    {
                        ShowOptions options = new ShowOptions();
                        options.resultCallback = result =>
                        {
                            switch (result)
                            {
                                case ShowResult.Finished:
                                    onFinished.InvokeSafe();
                                    break;
                                case ShowResult.Failed:
                                    onFailed.InvokeSafe();
                                    break;
                                case ShowResult.Skipped:
                                    onSkipped.InvokeSafe();
                                    break;
                            }
                        };

                        Advertisement.Show(rewardPlacementId, options);
                    }
        else if (KR.Monetization.instance.showEaLog)
                        Debug.Log("VIDEO is not avaiable!".color("00FFFF"));
#endif

		}

	}
}