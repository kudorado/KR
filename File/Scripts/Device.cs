
using UnityEngine;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace KR
{
	public class Device : KR.ScriptableSingleton<Device>
	{

#if UNITY_EDITOR
		[MenuItem("KR/File")]
		public static void FileSetting()
		{
			KR.Scriptable.CreateAsset<Device>("Assets/KR/File/Resources/");
		}
#endif


		public string fileDirectory = "/KR/File/Files";
		public string fileType = ".ea";
        

		public static string filePath<T>() where T : ISerializable
		{
			return (Path.Combine(path(instance.fileDirectory), typeof(T).Name.insert("(", ")") + instance.fileType));
		}
		public static string filePath(string fileName)
		{
			return (Path.Combine(path(instance.fileDirectory), fileName));
		}


		public static string path(string directory)
		{

#if !UNITY_EDITOR
		return Application.persistentDataPath;
#else
			if (!Directory.Exists(Application.dataPath + directory))
				Directory.CreateDirectory(Application.dataPath + directory);
			return Application.dataPath + directory;
#endif

		}
	}
}