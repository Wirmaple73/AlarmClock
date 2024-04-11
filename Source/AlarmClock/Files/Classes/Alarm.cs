using System;

namespace AlarmClock
{
	public class Alarm
	{
		public bool IsEnabled { get; set; }

		public DaysToRepeat DaysToRepeat { get; set; }
		public TimeSpan Time { get; set; }

		public string Description { get; set; }

		public string SoundPath { get; set; }
		public double Volume { get; set; }

		public Alarm() { }

		public Alarm(bool isEnabled, DaysToRepeat daysToRepeat, TimeSpan time, string description, string soundPath, double volume)
		{
			IsEnabled	  = isEnabled;
			DaysToRepeat  = daysToRepeat;
			Time		  = time;
			Description   = description;
			SoundPath	  = soundPath;
			Volume		  = volume >= 0 && volume <= 100 ? volume : throw new ArgumentOutOfRangeException(nameof(volume), "Volume must be between 0 and 100, inclusive.");
		}

		public bool HasToBeSoundedToday
			=> DaysToRepeat.HasFlag(Enum.Parse<DaysToRepeat>(DateTime.Now.DayOfWeek.ToString()));

		public bool HasToBeSoundedNow
			=> Time.Hours == DateTime.Now.Hour && Time.Minutes == DateTime.Now.Minute;
	}
}
