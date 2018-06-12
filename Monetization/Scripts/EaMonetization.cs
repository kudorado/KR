#pragma warning disable
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;
using System.Linq;
using Sirenix.OdinInspector;
using System.IO;
using Random = UnityEngine.Random;
#if UNITY_EDITOR
using UnityEditor;
#endif
public interface IEaAd
{
    void Init();
}


public enum AdType
{
    BANNER_TOP,
    BANNER_BOTTOM,
    INTERSTITIAL,
}
public enum EaAdSize
{
    BANNER,
    SMART_BANNER,
    IAB_BANNER,
    LEADERBOARD,
    MEDIUM_RECTANGLE

}

public class EaMonetization : KR.ScriptableSingleton<EaMonetization>
{
#if UNITY_EDITOR
    [MenuItem("Ea/Monetization/Settings")]
    public static void Advetisement()
    {
		KR.Scriptable.CreateAsset<EaMonetization>("Assets/Ea/Monetization/Resources/");
    }
#endif






    public static event Action<AdType[]> onShowingAds;
    public static event Action<AdType[]> onHidingAds;
    public static event Action onVideoShowing;
    public static event Action<Action, Action, Action> onRewardVideoShowing;
    public static event Action onAdInitialize;
    public static Func<bool> onVideoValidate, onRewardVideoValidate;





    public override void OnInitialized()
    {
        base.OnInitialized();
        onAdInitialize();
    }


    public static void Show(params AdType[] types)
    {
        if (onShowingAds == null)
        {
            Debug.Log("Ads Not Found!");
            return;
        }
        //show random event 
        var shows = onShowingAds.GetInvocationList().Cast<Action<AdType[]>>().ToArray();
        var rand = Random.Range(0, shows.Length);
        shows[rand].Invoke(types);

    }

    public static void Hide(params AdType[] types)
    {
        onHidingAds?.Invoke(types);
    }

    public static void ShowVideo()
    {
        if (onVideoShowing == null)
        {
            Debug.Log("Video Ads Not Found!");
            return;
        }

        var shows = onVideoShowing.GetInvocationList().Cast<Action>().ToArray();
        var rand = Random.Range(0, shows.Length);
        shows[rand].Invoke();

    }

    public static void ShowRewardVideo(Action onCompleted, Action onSkipped = null, Action onCancelled = null)
    {
        if (onRewardVideoShowing == null)
        {
            Debug.Log("Video Reward Ads Not Found!");
            return;
        }

        var shows = onRewardVideoShowing.GetInvocationList().Cast<Action<Action, Action, Action>>().ToArray();
        var rand = Random.Range(0, shows.Length);
        shows[rand].Invoke(onCompleted, onSkipped, onCancelled);

    }
    public static bool isVideoAvaiable
    {
        get
        {
            bool result = false;
            if (onVideoValidate != null)
            {
                var predicate = onVideoValidate.GetInvocationList();
                foreach (Func<bool> res in predicate)
                {
                    result = res();
                    if (result) break;
                }

            }

            return onVideoValidate == null ? false : result;

        }
    }
    public static bool isRewardVideoAvaiable
    {
        get
        {
            bool result = false;
            if (onRewardVideoValidate != null)
            {
                var predicate = onRewardVideoValidate.GetInvocationList();
                foreach (Func<bool> res in predicate)
                {
                    result = res();
                    if (result) break;
                }

            }

            return onRewardVideoValidate == null ? false : result;
        }
    }
    [ShowIf("setting"), PropertyOrder(1)]
    public bool showEaLog, showDummyClient;

    #region EDITOR
#if UNITY_EDITOR

    bool isAdmobInstalled { get { return Directory.Exists(getPath("Ea/Monetization/Installed/AdMob")); } }
    bool isUnityAdsInstalled { get { return Directory.Exists(getPath("Ea/Monetization/Installed/UnityAds")); } }
    bool isUnityIAPInstalled { get { return Directory.Exists(getPath("Ea/Monetization/Installed/UnityIAP")); } }
    bool isPlayGamesInstalled { get { return Directory.Exists(getPath("Ea/Monetization/Installed/PlayGames")); } }



    private bool setting, adMob, unityAds, unityIAP, playGames;

    [HorizontalGroup("Toolbar")]
    [Button]
    public void Setting()
    {
        DeSelectAll();
        setting = true;
    }
    [HorizontalGroup("Toolbar")]
    [Button]
    public void MobileAds()
    {
        DeSelectAll();
        adMob = true;
        //Debug.Log("ADMOB");
    }
    [HorizontalGroup("Toolbar")]
    [Button]
    public void UnityAds()
    {
        DeSelectAll();
        unityAds = true;
    }
    [HorizontalGroup("Toolbar")]
    [Button]
    public void UnityIAP()
    {
        DeSelectAll();
        unityIAP = true;

    }
    [HorizontalGroup("Toolbar")]
    [Button]
    public void PlayGames()
    {
        DeSelectAll();
        playGames = true;
    }
    private void DeSelectAll()
    {
        setting = adMob = unityIAP = unityAds = playGames = false;
    }

    [Button]
    [ShowIf("playGames")]
    void InstallPlayGames()
    {
        AssetDatabase.ImportPackage(playGamesInstalledPath, true);
    }
    [Button]
    [ShowIf("isPlayGamesInstalled")]
    [ShowIf("playGames")]
    void UninstallPlayGames()
    {
        UnInstallPlayGames();
    }

    [Button]
    [ShowIf("unityIAP")]
    void InstallUnityIAP()
    {
        AssetDatabase.ImportPackage(unityIAPInstalledPath, true);
    }

    [Button]
    [ShowIf("isUnityIAPInstalled")]
    [ShowIf("unityIAP")]
    void UninstallUnityIAP()
    {
        UnInstallUnityIAP();
    }



    [Button]
    [ShowIf("unityAds")]
    void InstallUnityAds()
    {
        AssetDatabase.ImportPackage(unityAdsInstallPath, true);
    }

    [Button]
    [ShowIf("isUnityAdsInstalled")]
    [ShowIf("unityAds")]
    void UninstallUnityAds()
    {
        UnInstallUnityAds();
    }

    [Button]
    [ShowIf("adMob")]
    void InstallAdmob()
    {
        AssetDatabase.ImportPackage(admobInstallPath, true);
    }

    [Button]
    [ShowIf("adMob")]
    [ShowIf("isAdmobInstalled")]
    void UninstallAdmob()
    {
        UnInstallAdMob();
    }
    #region VARIABLE
    const string installAdMobText = "Install AdMob", uninstallAdMobText = "Uninstall AdMob", installUnityAdsText = "Install UnityAds",
    uninstallUnityAdsText = "Uninstall UnityAds", installUnityIAPText = "Install UnityIAP", uninstallUnityIAPText = "Uninstall UnityIAP",
    installPlayGamesText = "Install PlayGames", uninstallPlayGamesText = "Uninstall PlayGames";
    public static string[] adMobFolder = { "Ea/Monetization/Installed/AdMob", "GoogleMobileAds", "Plugins/Android/GoogleMobileAdsPlugin" };
    public static string[] unityAdsFolder = { "Ea/Monetization/Installed/UnityAds" };
    public static string[] unityIAPFolder = { "Ea/Monetization/Installed/UnityIAP", "Plugins/UnityChannel", "Plugins/UnityPurchasing" };
    public static string[] playGamesFolder = { "Ea/Monetization/Installed/PlayGames", "GooglePlayGames", "Plugins/Android/libs", "Plugins/Android/MainLibProj" };

    private readonly string googlePlayResolver = "PlayServicesResolver";
    private readonly string googlePlayGames = "GooglePlayGames", googleMobilesAds = "GoogleMobileAds";
    public const string defaultInstallPath = "Assets/Ea/Monetization/Packages/";
    public const string defaultAdMobPackage = "EaAdMob.unitypackage", defaultUnityAdsPackage = "EaUnityAds.unitypackage",
    defaultUnityIAPPackage = "EaUnityIAP.unitypackage", defaultPlayGamesPackage = "EaPlayGames.unitypackage";

    public static string admobInstallPath = defaultInstallPath + defaultAdMobPackage;
    public static string unityAdsInstallPath = defaultInstallPath + defaultUnityAdsPackage;
    public static string unityIAPInstalledPath = defaultInstallPath + defaultUnityIAPPackage;
    public static string playGamesInstalledPath = defaultInstallPath + defaultPlayGamesPackage;
    public static string getPath(string n)
    {
        return Application.dataPath + "/" + n;
    }
    #endregion
    void Uninstall(string[] assetPaths, bool isDone = false)
    {
        float progress = 0;
        for (int i = 0; i < assetPaths.Length; i++)
        {
            //Debug.Log(assetPaths[i]);
            if (!Directory.Exists(getPath(assetPaths[i])))
                continue;

            FileUtil.DeleteFileOrDirectory(getPath(assetPaths[i]));
            progress += 1f / (float)assetPaths.Length;
            EditorUtility.DisplayProgressBar("Assets Deleting", "Deleting " + assetPaths[i], progress);

        }

        if (isDone)
        {
            string plugin = "Plugins";
            try
            {
                var plugins = Directory.GetDirectories(getPath(plugin));
                if (plugins.Length == 0)
                {
                    EditorUtility.DisplayProgressBar("Deleting Plugins", "Finish cleaning, updating assets", 1);
                    FileUtil.DeleteFileOrDirectory(getPath(plugin));

                }
            }
            catch { }
            EditorUtility.ClearProgressBar();
            AssetDatabase.Refresh();
        }
    }
    void UnInstallUnityAds()
    {
        Uninstall(unityAdsFolder, true);
    }
    void UnInstallUnityIAP()
    {
        Uninstall(unityIAPFolder, true);

    }
    void UnInstallPlayGames()
    {


        float progress = 0;
        Uninstall(playGamesFolder);

        if (Directory.Exists(getPath(googlePlayResolver)))
        {
            if (!Directory.Exists(getPath(googleMobilesAds)))
            {
                FileUtil.DeleteFileOrDirectory(getPath(googlePlayResolver));
            }
        }
        UnInstallLib(ref progress);

    FINISH:

        AssetDatabase.Refresh();
        EditorUtility.ClearProgressBar();


    }
    void UnInstallLib(ref float progress)
    {
        string plugin = "Plugins";

        string[] plugins = Directory.Exists(getPath(plugin)) ? Directory.GetDirectories(getPath(plugin)) : null;
        if (plugins == null)
            return;

        for (int i = 0; i < plugins.Length; i++)
        {
            string trunc = plugins[i].Replace(getPath("Plugins/"), "");
            string match = trunc.ToLower() == "android" ? "/Android" : trunc.ToLower() == "ios" ? "/iOS" : string.Empty;
            if (match != string.Empty)
            {
                //check if it have noisUnityAds file or directory
                string currentPath = getPath(plugin + match);
                ClearAndroidLibrary(plugins[i], match, ref progress);
                CleanIOSLibrary(plugins[i], match, ref progress);
                var subFiles = Directory.GetFiles(currentPath);
                var subFolders = Directory.GetDirectories(currentPath);


                if (subFolders.Length == 0)
                {
                    bool isExistAnyFiles = false;
                    if (subFiles.Length > 0)
                    {
                        foreach (var f in subFiles)
                        {
                            if (!f.Contains(".meta"))
                            {
                                isExistAnyFiles = true;
                                break;
                            }
                        }
                    }
                    if (!isExistAnyFiles)
                    {
                        EditorUtility.DisplayProgressBar("Deleting Modules", "Deleting " + trunc, 1);
                        FileUtil.DeleteFileOrDirectory(plugins[i]);
                    }

                }
            }
        }
        try
        {
            plugins = Directory.GetDirectories(getPath(plugin));
            if (plugins.Length == 0)
            {
                EditorUtility.DisplayProgressBar("Deleting Plugins", "Finish cleaning, updating assets", 1);
                FileUtil.DeleteFileOrDirectory(getPath(plugin));

            }
        }
        catch { }

    }
    void UnInstallAdMob()
    {

        float progress = 0;
        Uninstall(adMobFolder);

        if (Directory.Exists(getPath(googlePlayResolver)))
        {
            if (!Directory.Exists(getPath(googlePlayGames)))
            {
                FileUtil.DeleteFileOrDirectory(getPath(googlePlayResolver));
            }
        }
        UnInstallLib(ref progress);



    FINISH:

        AssetDatabase.Refresh();
        EditorUtility.ClearProgressBar();

    }
    void ClearAndroidLibrary(string path, string match, ref float progress)
    {
        string[] pattern = { ".aar", ".jar" };
        List<string> toRemoves = new List<string>();
        if (match == "/Android")
        {
            var subFiles = Directory.GetFiles(path);
            int length = subFiles.Length;

            for (int i = 0; i < length; i++)
            {
                for (int p = 0; p < pattern.Length; p++)
                {
                    if (subFiles[i].Contains(pattern[p]))
                    {
                        toRemoves.Add(subFiles[i]);
                    }

                }
            }
            length = toRemoves.Count;
            for (int rm = 0; rm < length; rm++)
            {
                FileUtil.DeleteFileOrDirectory(toRemoves[rm]);
                progress += (0.8f / (float)length);
                EditorUtility.DisplayProgressBar("Deleting Library", "Cleaning " + toRemoves[rm], progress);
            }

        }
    }
    void CleanIOSLibrary(string path, string match, ref float progress)
    {
        string[] pattern = { ".m", ".mm", ".h" };
        List<string> toRemoves = new List<string>();
        if (match == "/iOS")
        {
            var subFiles = Directory.GetFiles(path);
            int length = subFiles.Length;

            for (int i = 0; i < length; i++)
            {
                for (int p = 0; p < pattern.Length; p++)
                {
                    if (subFiles[i].Contains(pattern[p]))
                    {
                        toRemoves.Add(subFiles[i]);
                    }

                }
            }
            length = toRemoves.Count;
            for (int rm = 0; rm < length; rm++)
            {
                FileUtil.DeleteFileOrDirectory(toRemoves[rm]);
                progress += (0.8f / (float)length);
                EditorUtility.DisplayProgressBar("Deleting Library", "Cleaning " + toRemoves[rm], progress);
            }

        }
    }
#endif

    #endregion
}




