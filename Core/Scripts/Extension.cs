using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace KR
{
	#region EaSerializable
	[Serializable]
	public struct EaCurrencyFormatter
	{
		public int generation, value, length;
		public string format, spacing;
		public EaCurrencyFormatter(int value, int generation = 1, string format = "", int leng = 1, string spacing = ",")
		{
			this.value = value;
			this.format = format;
			this.generation = generation;
			this.length = leng;
			this.spacing = spacing;
		}
	}
	#endregion
	public static class EaDelegate
	{

		public static void InvokeSafe(this Action @this)
		{
			if (@this != null)
				@this.Invoke();
		}
	}
	public static class EaArray
	{
		public static T[] clear<T>(this T[] @this, T dflt = default(T))
		{
			int leng = @this.Length;
			for (int i = 0; i < leng; i++)
			{
				@this[i] = dflt;
			}
			return @this;
		}
	}
	public static class EaInt
	{

		public static int log(this int @this, int @base)
		{
			return Mathf.Log(@this, @base).round();
		}
		public static int log2(this int @this)
		{
			return Mathf.Log(@this, 2).round();
		}
		public static int abs(this int @this)
		{
			return @this < 0 ? Mathf.Abs(@this) : @this;
		}
		public static int round(this float @this)
		{
			return Mathf.RoundToInt(@this);
		}

		public static int floor(this float @this)
		{
			return Mathf.FloorToInt(@this);
		}
		public static int toInt(this byte @this)
		{
			return (int)@this;
		}
		public static int floor(this int value, int target)
		{
			return (target * (value / target) - (value % target));
		}
		/// <summary>
		/// Clamp the specified value, min and max.
		/// </summary>
		public static int clamp(this int @this, int min = int.MinValue, int max = int.MaxValue)
		{
			return @this < min ? min : @this > max ? max : @this;
		}

	}
	public static class EaBool
	{
		public static bool nullPtr<T>(this T[] @this)
		{
			return !(@this != null && @this.Length > 0);
		}
		public static bool nullPtr<T>(this T @this)
		{
			return !(@this != null);
		}
		public static bool nullPtr<T>(this T @this, Action callback)
		{
			var result = @this.nullPtr();
			callback();
			return result;
		}
		public static bool nullPtr<T>(this T[] @this, Action<bool> callback)
		{
			var result = @this.nullPtr();
			callback(result);
			return (result);
		}
	}
	public static class EaCorountine
	{

		public static Coroutine Invoke(this IEnumerator enumerator, MonoBehaviour behaviour)
		{
			return behaviour.StartCoroutine(enumerator);

		}
		//public static void CallDelay(this MonoBehaviour behaviour, IEnumerator enumerator, float delay){
		//behaviour.StartCoroutine(enumerator);
		//}
	}
	public static class EaFloat
	{
		/// <summary>
		/// Clamp the specified value, min and max.
		/// </summary>
		public static float abs(this float @this)
		{
			return @this > 0 ? @this : -@this;
		}
		public static float ceil(this float @this)
		{
			return Mathf.Ceil(@this);
		}
		public static int pow(this float @this, float pow)
		{
			return Mathf.Pow(@this, pow).round();
		}

		public static float log(this float @this, int @base)
		{
			return Mathf.Log(@this, @base);
		}
		public static float log2(this float @this)
		{
			return Mathf.Log(@this, 2).round();
		}
		public static float clamp(this float @this, float min = float.MinValue, float max = float.MaxValue)
		{
			return @this < min ? min : @this > max ? max : @this;
		}
		public static float floor(this float value, float target)
		{
			return (target * (value / target) - (value % target));
		}
		public static float toFloat(this double value)
		{
			return (value > float.MaxValue ? float.MaxValue : value < float.MinValue ? float.MinValue : (float)value);
		}
		public static float roundTo(this float value, float round)
		{
			if (round == 0) return value;

			return (round * (value / round) - (value % round) + (value % round == 0 ? 0 : round));
		}

	}
	public static class EaVec2
	{

		public static Vector2 set(this Vector2 vector, float x = float.NaN, float y = float.NaN)
		{
			vector.x = !float.IsNaN(x) ? x : vector.x;
			vector.y = !float.IsNaN(y) ? y : vector.y;

			return vector;
		}
		public static Vector2 abs(this Vector2 @this, bool x = true, bool y = true)
		{
			@this.x = x ? Mathf.Abs(@this.x) : @this.x;
			@this.y = y ? Mathf.Abs(@this.y) : @this.y;
			return @this;
		}
		public static Vector2 neg(this Vector2 @this, bool x = true, bool y = true)
		{
			@this.x = x ? -Mathf.Abs(@this.x) : @this.x;
			@this.y = y ? -Mathf.Abs(@this.y) : @this.y;
			return @this;

		}
		public static Vector2 lerp(this Vector2 @this, Vector2 target, float t)
		{
			return Vector2.Lerp(@this, target, t);
		}
		public static Vector2 round(this Vector2 @this, bool x = true, bool y = true)
		{
			@this.x = x ? Mathf.Round(@this.x) : @this.x;
			@this.y = y ? Mathf.Round(@this.y) : @this.y;
			return @this;
		}
		public static Vector2 floor(this Vector2 @this, bool x = true, bool y = true)
		{
			@this.x = x ? Mathf.Floor(@this.x) : @this.x;
			@this.y = y ? Mathf.Floor(@this.y) : @this.y;
			return @this;
		}

	}
	public static class EaVec3
	{
		public static Vector3 clamp(this Vector3 vector, Vector3 min, Vector3 max)
		{
			vector.x = vector.x < min.x ? min.x : vector.x > max.x ? max.x : vector.x;
			vector.y = vector.y < min.y ? min.y : vector.y > max.y ? max.y : vector.y;
			vector.z = vector.z < min.z ? min.z : vector.z > max.z ? max.z : vector.z;


			return vector;
		}
		public static Vector3 set(this Vector3 vector, float x = float.NaN, float y = float.NaN, float z = float.NaN)
		{
			vector.x = !float.IsNaN(x) ? x : vector.x;
			vector.y = !float.IsNaN(y) ? y : vector.y;
			vector.z = !float.IsNaN(z) ? z : vector.z;
			return vector;
		}
		public static Vector3 abs(this Vector3 @this, bool x = true, bool y = true, bool z = true)
		{
			@this.x = x ? Mathf.Abs(@this.x) : @this.x;
			@this.y = y ? Mathf.Abs(@this.y) : @this.y;
			@this.z = z ? Mathf.Abs(@this.z) : @this.z;
			return @this;
		}
		public static Vector3 lerp(this Vector3 @this, Vector3 target, float t)
		{
			return Vector3.Lerp(@this, target, t);
		}
		public static Vector3 round(this Vector3 @this, bool x = true, bool y = true, bool z = true)
		{
			@this.x = x ? Mathf.Round(@this.x) : @this.x;
			@this.y = y ? Mathf.Round(@this.y) : @this.y;
			@this.z = z ? Mathf.Round(@this.z) : @this.z;
			return @this;
		}
		public static Vector3 floor(this Vector3 @this, bool x = true, bool y = true, bool z = true)
		{
			@this.x = x ? Mathf.Floor(@this.x) : @this.x;
			@this.y = y ? Mathf.Floor(@this.y) : @this.y;
			@this.z = z ? Mathf.Floor(@this.z) : @this.z;
			return @this;
		}


	}
	public static class EaColor { }
	public static class EaColor32 { }
	public static class EaString
	{
		public static string format(this string input, string result)
		{
			return string.Format(input, result);

		}
		public static string color(this string stringInput, Color color)
		{
			byte maxBytes = 255;
			string hex = (color.r * maxBytes).round().toHex() + (color.g * maxBytes).round().toHex() + (color.b * maxBytes).round().toHex() + (color.a * maxBytes).round().toHex();
			return string.Format("<color=#{1}>{0}</color>", stringInput, hex);
		}
		public static string color(this string @this, Color32 color)
		{
			string hex = color.r.toInt().toHex() + color.g.toInt().toHex() + color.b.toInt().toHex() + color.a.toInt().toHex();
			return string.Format("<color=#{1}>{0}</color>", @this, hex);
		}
		public static string color(this string @this, string color)
		{
			return string.Format("<color=#{1}>{0}</color>", @this, color);
		}
		public static string toCurrency(this int @this, params EaCurrencyFormatter[] formats)
		{
			int leng = formats.Length;
			for (int i = 0; i < leng; i++)
			{
				if (formats[i].value > @this)
				{
					var balance = @this % formats[i].generation;
					var result = (@this - balance) / formats[i].generation;
					//Debug.Log(balance);
					return result.ToString() + formats[i].spacing + (formats[i].length == 0 ? "" : balance.ToString().Substring(0, formats[i].length.clamp(1, balance.ToString().Length))) + formats[i].format;
				}
			}
			throw new FormatException();

		}
		//      public static string toHex (this Color color){
		//
		//
		//      }

		private static string getHexString(this int @this, bool doubleZero = false)
		{
			var hexChar = "abcdef";
			if (doubleZero && @this == 0)
				return "00";

			var result = @this <= 9 ? @this.ToString() : hexChar[(hexChar.Length - 1) - (15 - @this)].ToString();
			return result;
		}
		public static string endline(this string @this)
		{
			return @this + "\n";
		}
		public static string toHex(this int @this, string result = "")
		{
			if (@this < 16)
			{
				result = result.insertBefore(@this.getHexString(true));
				return result;
			}
			var odd = (@this % 16f).round();

			result = result.insertBefore(odd.getHexString());
			var newValue = ((@this - odd) / 16f).round();
			return toHex(newValue, result);
		}
		public static string insertBefore(this string @this, string insertor)
		{
			return insertor + @this;
		}
		public static string insertAfter(this string @this, string insertor)
		{
			return @this + insertor;
		}
		public static string insert(this string @this, string before, string after)
		{
			return before + @this + after;
		}

	}


}