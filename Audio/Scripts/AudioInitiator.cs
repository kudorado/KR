using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace KR
{
	public class AudioInitiator : KR.ManagerSingleton<AudioInitiator>
	{

		protected override void Awake()
		{
			base.Awake();
			EaAudio.Init();
		}
	}
}