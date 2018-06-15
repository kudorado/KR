using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
namespace KR
{
	public class UnityAdsInitiator : AdsInitiator<UnityAdsInitiator>
	{

		// Use this for initialization
		public override void Init()
		{
			UnityAds.Init();
		}



	}
}