using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Text;
#if UNITY_EDITOR
using UnityEditor;
#endif
#pragma warning disable


namespace KR
{
	[System.Serializable]
	public struct AudioData
	{
		public string key;
		public AudioClip clip;
	}
	public enum AudioCategory{
	    SoundEffect,
        BackgroundMusic,
	}
	public class Audio : KR.ScriptableSingleton<Audio>
	{

		const string defaultResourcePath = "Assets/KR/Audio/Resources/";
	
#if UNITY_EDITOR

		[MenuItem("KR/Audio/Init")]
		public static void CreateInitiator()
		{
			KR.Scriptable.CreateInitiator<AudioInitiator>();
		}

		[MenuItem("KR/Audio/Settings")]
		public static void CreateAsset()
		{
			KR.Scriptable.CreateAsset<Audio>(defaultResourcePath);
		}



#endif


		public List<AudioData> audioList = new List<AudioData>();


		public AudioSource backgrounMusicdConfig, soundEffectConfig;
        //[]
		//public string generated;
		public static AudioSource soundEffect { get; set; }
		public static AudioSource backgroundMusic { get; set; }

		public override void OnInitialized()
		{
			base.OnInitialized();

			soundEffect = soundEffectConfig ? Instantiate(soundEffectConfig) :  new GameObject("SoundEffect").AddComponent<AudioSource>();
			backgroundMusic = backgrounMusicdConfig ? Instantiate(backgrounMusicdConfig) : new GameObject("BackgroundMusic").AddComponent<AudioSource>();
			DontDestroyOnLoad(backgroundMusic.gameObject);
			DontDestroyOnLoad(soundEffect.gameObject);
		}
#if UNITY_EDITOR

		[Button]
		private void GenerateAudio()
		{
			StringBuilder builder = new StringBuilder();
			builder.AppendLine("namespace KR ");
            builder.AppendLine("{");
            builder.AppendLine("\tpublic enum AudioList");
            builder.AppendLine("\t{");
			for (int i = 0; i < audioList.Count; i++)
			{
				if (audioList[i].clip != null && !string.IsNullOrEmpty(audioList[i].key))
				{
					builder.AppendLine("\t\t "+audioList[i].key + ",");
				}
			}
			builder.AppendLine("\t} ");
            builder.AppendLine("} ");

			//Debug.Log(builder.ToString());
			System.IO.File.WriteAllText(Application.dataPath + "/KR/Audio/Resources/" + "AudioList.cs", builder.ToString());
			//Debug.Log(Application.dataPath + "/Ea/Audio/Resources/EaAudioList.cs");
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}
#endif
		public static void Play(GeneratedAudio key, AudioCategory category = AudioCategory.SoundEffect)
		{

			for (int i = 0; i < instance.audioList.Count; i++)
			{
				if (instance.audioList[i].key == key.ToString())
				{
					//Debug.Log(instance.audioList[i]);
					//Debug.Log(source);
					switch(category){
						case AudioCategory.BackgroundMusic:
							backgroundMusic.clip = instance.audioList[i].clip;
							backgroundMusic.Play();
							break;
						case AudioCategory.SoundEffect:
							soundEffect.PlayOneShot(instance.audioList[i].clip);

							break;
					}
					return;
				}
			}
		}
	}

}
