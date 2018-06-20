
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.Threading;


namespace KR
{


	public class Serializable<T>: Serializable where T : Serializable , new(){
		public static T instance{
			get{
				return _instance ?? (_instance = File.Open<T>());
			}
		}
		private static T _instance;
	}
	[System.Serializable]
	public class Serializable : ISerializable
	{
		public string path { get; set; }
		public bool cryption { get; set; }
	}

	public interface ISerializable
	{
		string path { get; set; }
		bool cryption { get; set; }
	}

	public static class File
	{
		private static void delete(this string path)
		{
			System.IO.File.Delete(path);
		}

		public static T Open<T>(string fileName = "") where T : ISerializable, new()
		{
			string path = Device.filePath(typeof(T).Name.insert("(", ")") + fileName + Device.instance.fileType);
			T @out = new T();
			KR.Scripton<FileDelegate>.Init();

			ThreadStart fileResult = new ThreadStart(() =>
			{
				BinaryFormatter bf = new BinaryFormatter();
				if (System.IO.File.Exists(path))
				{
					using (FileStream fs = System.IO.File.Open(path, FileMode.Open))
					{
						try
						{

							string decryptor = (string)bf.Deserialize(fs);
							@out = JsonUtility.FromJson<T>(Cryptography.Decrypt(decryptor));

						}
						catch (Exception fileFormatException)
						{
							Debug.LogError("Can't deserialize, file format not found!");
							throw fileFormatException;
						}

					}
				}
				else
					using (FileStream fs = System.IO.File.Create(path))
					{
						try
						{
							string encryptor = Cryptography.Encrypt(JsonUtility.ToJson(@out));
							bf.Serialize(fs, encryptor);
						}
						catch (Exception failedSerializeException)
						{
							Debug.LogError("Can't serialize object,make sure the object have attribute [System.Serializable]");
							throw failedSerializeException;
						}
					}
				@out.cryption = true;
				@out.path = path;
				if (!FileDelegate.instance.openedFiles.Contains(path))
				{
					FileDelegate.instance.openedFiles.Add(path);
					FileDelegate.instance.onQuit += delegate
					{
						Thread microThread = new Thread(new ThreadStart(() =>
						{
							@out.Save();

						}));
						microThread.Start();
						microThread.Join();

					};

					FileDelegate.instance.onPause += status =>
					{
						Thread microThread = new Thread(new ThreadStart(() =>
						{
							if (status)
								@out.Save();
						}));
						microThread.Start();
						microThread.Join();


					};
				}
			});
			Thread fileThread = new Thread(fileResult);
			fileThread.Start();
			fileThread.Join();

			return @out;
		}
		public static void Save<T>(this T file) where T : ISerializable
		{
			bool encryption = file.cryption;
			if (System.IO.File.Exists(file.path))
				file.path.delete();
			Thread fileSave = new Thread(() =>
			{
				using (FileStream fs = System.IO.File.Open(file.path, FileMode.Create))
				{
					BinaryFormatter bf = new BinaryFormatter();
					string cryptor = Cryptography.Encrypt(JsonUtility.ToJson(file));
					bf.Serialize(fs, cryptor);

				}

			});
			fileSave.Start();
			fileSave.Join();
		}


	}




}