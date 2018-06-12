#define EA_AD_IMPLEMENT
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace KR
{
	public delegate void OnQuit();
	public delegate void OnBack();
	public delegate void OnHide();
	public delegate void OnPause(bool status);
	public class FileDelegate : KR.Scripton<FileDelegate>
	{
        
		public  event OnPause onPause = delegate { };
		public  event OnBack onBack = delegate { };
		public  event OnQuit onQuit = delegate { };

		public  List<string> openedFiles = new List<string>();

		void OnApplicationQuit()
		{
			onQuit.Invoke();
		}
		void OnApplicationPause(bool status)
		{
			onPause.Invoke(status);

		}
		void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				onBack.Invoke();
			}
		}


	}
}