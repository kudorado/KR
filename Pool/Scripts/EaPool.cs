using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace KR
{
	public class Pool : KR.ScriptableSingleton<Pool>
	{
#if UNITY_EDITOR
		[MenuItem("Ea/Pool/Settings")]
		public static void PoolSetting()
		{
			KR.Scriptable.CreateAsset<Pool>("Assets/Ea/Pool/Resources/");
		}

		[MenuItem("Ea/Pool/Init")]
		public static void PoolInit()
		{
			var pool = FindObjectOfType<PoolInitiator>();

			pool = pool ?? (new GameObject(typeof(PoolInitiator).Name).AddComponent<PoolInitiator>());

			Selection.activeObject = pool;
			UnityEditor.SceneManagement.EditorSceneManager.SaveScene(UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
			//EditorUtility.SetDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
		}
#endif
		public PoolData[] initData;
		private Dictionary<string, int> poolHolder = new Dictionary<string, int>();
		private List<GameObject[]> poolList = new List<GameObject[]>();
		private GameObject inst;
		public static bool initialized { get; set; }
		public static void Init()
		{

			if (initialized)
				return;


			initialized = true;
			int length = instance.initData.Length;
			for (int i = 0; i < length; i++)
			{
				instance.poolHolder.Add(instance.initData[i].key, i);
				instance.poolList.Add(new GameObject[instance.initData[i].poolAmount]);

				for (int init = 0; init < instance.initData[i].poolAmount; init++)
				{
					instance.inst = Instantiate(instance.initData[i].source);
					//instance.Sta();
					instance.inst.gameObject.SetActive(false);
					instance.poolList[i][init] = instance.inst;
					DontDestroyOnLoad(instance.inst);

				}
			}
		}

		public static void DisableAll()
		{
			int l1 = instance.poolList.Count;
			for (int i = 0; i < l1; i++)
			{
				int l2 = instance.poolList[i].Length;
				for (int j = 0; j < l2; j++)
				{
					instance.poolList[i][j].SetActive(false);
				}
			}
		}
		public static T GetPool<T>(string key, bool active = true, int reuseAmount = 5, Func<T, bool> reusePredicate = null) where T : Component
		{
			int index = instance.poolHolder[key];
			int length = instance.poolList[index].Length;
			for (int i = 0; i < length; i++)
			{
				if (!instance.poolList[index][i].activeSelf)
				{
					instance.poolList[index][i].SetActive(active);
					return instance.poolList[index][i].GetComponent<T>();
				}
			}
			if (reusePredicate == null) return null;

			CollectPool<T>(key, reuseAmount, reusePredicate);
			return GetPool<T>(key, active);
		}

		public static void CollectPool<T>(string key, int amount, Func<T, bool> exclude) where T : Component
		{
			int index = instance.poolHolder[key];
			int length = amount.clamp(max: instance.poolList[index].Length);
			//Debug.Log(length);

			for (int i = 0; i < length; i++)
			{
				var t = instance.poolList[index][i].GetComponent<T>();
				if (t.gameObject.activeSelf)
				{
					if (exclude(t))
					{
						length = (length + 1).clamp(max: instance.poolList[index].Length);
						continue;
					}

					instance.poolList[index][i].SetActive(false);
				}
			}
		}
	}
	[Serializable]
	public class PoolData
	{
		public string key;
		public int poolAmount;
		public GameObject source;

	}
}