﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#pragma warning disable
namespace KR
{
	[System.Serializable]
	public struct DateTime
	{

		public int Year, Month, Day, Hour, Minute, Second;



		public DateTime(DateTime dateTime)
		{
			Year = dateTime.Year;
			Month = dateTime.Month;
			Hour = dateTime.Hour;
			Minute = dateTime.Minute;
			Second = dateTime.Second;
			Day = dateTime.Day;
		}

		public DateTime (int Year, int Month, int Day, int Hour, int Minute, int Second)
		{
			this.Year = Year;
			this.Day = Day;
			this.Hour = Hour;
			this.Month = Month;
			this.Minute = Minute;
			this.Second = Second;

		}
        

		public bool IsTheDayPassed()
		{
			return System.DateTime.Now.Day > Day || System.DateTime.Now.Month > Month || System.DateTime.Now.Year > Year;
		}
		public static bool operator ==(DateTime lhs, DateTime rhs)
		{
			return (lhs.Year == rhs.Year) && (lhs.Month == rhs.Month) && (lhs.Day == rhs.Day) && (lhs.Hour == rhs.Hour) && (lhs.Minute == rhs.Minute) && (lhs.Second == rhs.Second);
		}
		public DateTime TimeUntilNextDay()
		{
			var current = this.Second + (this.Minute * 60) + (this.Hour * 3600);
			var day = 24 * 3600;
			var result = day - current;


			var Hour = (result - (result % 3600)) / 3600;
			result = (result % 3600);
			var Minute = (result - (result % 60)) / 60;
			result = (result % 60);
			var Second = result;


			return new DateTime(0, 0, 0, Hour, Minute, Second);

		}

		public override string ToString()
		{
			//return base.ToString();
			return string.Format("{0}/{1}/{2}/{3}:{4}:{5}", Day, Month, Year, Hour, Minute, Second);
		}
		public static bool operator !=(DateTime lhs, DateTime rhs)
		{
			return !(lhs == rhs);
		}
		public static implicit operator DateTime(System.DateTime dateTime)
		{
			return new DateTime(dateTime);
		}

	}
}