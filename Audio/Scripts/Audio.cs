using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.IO;
using System.Text;
using System.Reflection;
#if UNITY_EDITOR
using UnityEditor;
#endif
#pragma warning disable
namespace KR{
	public class EaAudio : KR.ScriptableSingleton<EaAudio>
	{

		const string defaultResourcePath = "Assets/Ea/Audio/Resources/";
		const string accessModify = "public";
		const string type = " enum ";
		const string className = " EaAudioList";
		const string fileType = ".cs";
        
#if UNITY_EDITOR

		[MenuItem("Ea/Audio/Init")]
		public static void CreateInitiator()
		{
			KR.Scriptable.CreateInitiator<AudioInitiator>();
		}

		[MenuItem("Ea/Audio/Settings")]
		public static void CreateAsset()
		{
			KR.Scriptable.CreateAsset<EaAudio>(defaultResourcePath);
		}



#endif


		public List<AudioClip> audioList = new List<AudioClip>();
		[SerializeField]
		private EaAudioList generatedAudio;

		public AudioSource bgm;
		public static AudioSource source { get; set; }
		public static AudioSource backgroundMusic { get; set; }

		public override void OnInitialized()
		{
			base.OnInitialized();

			source = new GameObject(typeof(EaAudio).Name).AddComponent<AudioSource>();
			backgroundMusic = Instantiate(bgm);
			DontDestroyOnLoad(backgroundMusic.gameObject);
			DontDestroyOnLoad(source);
		}
#if UNITY_EDITOR

		[Button]
		private void GenerateAudio()
		{
			StringBuilder builder = new StringBuilder(accessModify + type + className);
			builder.AppendLine(" { ");
			for (int i = 0; i < audioList.Count; i++)
			{
				if (audioList[i] != null)
				{
					builder.AppendLine(audioList[i].name + ",");
				}
			}
			builder.AppendLine(" } ");
			//Debug.Log(builder.ToString());
			System.IO.File.WriteAllText(Application.dataPath + "/Ea/Audio/Resources/" + className + fileType, builder.ToString());
			//Debug.Log(Application.dataPath + "/Ea/Audio/Resources/EaAudioList.cs");
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}
#endif
		public static void Play(EaAudioList audioName)
		{

			for (int i = 0; i < instance.audioList.Count; i++)
			{
				if (instance.audioList[i].name == audioName.ToString())
				{
					//Debug.Log(instance.audioList[i]);
					//Debug.Log(source);
					source.PlayOneShot(instance.audioList[i]);
					return;
				}
			}
		}
	}

}
