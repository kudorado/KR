using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;
namespace KR
{
	public abstract class Node<T> : KR.Singleton<T> where T : Node<T>
	{
		[ShowInInspector,DictionaryDrawerSettings,ReadOnly]
		public  Dictionary<string, List<Action>> events = new Dictionary<string, List<Action>>();
		public static void Emit(string eventKey)
		{
			if (instance.events.ContainsKey(eventKey))
				for (int i = 0; i < instance.events[eventKey].Count; i++)
					instance.events[eventKey][i].InvokeSafe();
		}

		private void OnDisable()
		{
			events.Clear();
		}

		public static void On(string eventKey, Action callback)
		{
			if (!instance.events.ContainsKey(eventKey))
				instance.events.Add(eventKey, new List<Action>());


			instance.events[eventKey].Add(callback);
		}
		public static void On(string[] eventKeys, Action callback)
		{


			for (int i = 0; i < eventKeys.Length; i++)
			{
				if (!instance.events.ContainsKey(eventKeys[i]))
					instance.events.Add(eventKeys[i], new List<Action>());

				instance.events[eventKeys[i]].Add(callback);
				//			Debug.Log ("added event:" + eventKeys [i]);

			}
		}
		public static void On(Action callback, params string[] eventKeys)
		{


			for (int i = 0; i < eventKeys.Length; i++)
			{
				if (!instance.events.ContainsKey(eventKeys[i]))
					instance.events.Add(eventKeys[i], new List<Action>());

				instance.events[eventKeys[i]].Add(callback);

			}
		}
	}
}