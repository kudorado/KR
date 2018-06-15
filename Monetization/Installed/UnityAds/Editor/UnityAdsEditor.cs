using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace KR.Editor
{
	[CustomEditor(typeof(UnityAds))]
	public class UnityAdsEditor : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
#if UNITY_ADS
        base.OnInspectorGUI();
#else
			EditorGUILayout.HelpBox("No Unity Ads Found, Please do the following method below to make it work.", MessageType.Error);
			EditorGUILayout.HelpBox("Open Window -> Service -> Ads -> On.", MessageType.Info);
			EditorGUILayout.HelpBox("If not work you can download it from https://assetstore.unity.com/packages/add-ons/services/unity-ads-66123 and import to current working project.", MessageType.Info);
#endif
		}
	}
}