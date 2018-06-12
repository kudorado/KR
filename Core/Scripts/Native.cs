using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace KR
{
	public sealed class Native
	{

		public static void Share(string shareText, string shareSubject = "")
		{

#if UNITY_ANDROID

        try
        {
            AndroidJavaClass Intent = new AndroidJavaClass("android.content.Intent");//using Intent = android.content.Intent;
            AndroidJavaObject intent = new AndroidJavaObject("android.content.Intent");//Intent intent = new Intent();

            intent.Call<AndroidJavaObject>("setAction", Intent.GetStatic<string>("ACTION_SEND")); //intent.setAction(Intent.ACTION_SEND);
            intent.Call<AndroidJavaObject>("setType", "text/plain");//intent.setType(type);

            intent.Call<AndroidJavaObject>("putExtra", Intent.GetStatic<string>("EXTRA_TEXT"), shareText);

            //instantiate the class UnityPlayer
            AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject chooser = Intent.CallStatic<AndroidJavaObject>("createChooser", intent, shareSubject);
            //call the activity with our Intent
            currentActivity.Call("startActivity", chooser);
        }
        catch (System.Exception e)
        {
            Debug.Log("Dummy share called." + e);
        }
#endif
		}
	}
}