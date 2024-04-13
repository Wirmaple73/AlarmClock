using System;

namespace AlarmClock
{
	public class Alarm
	{
		private double volume;

		public bool IsEnabled { get; set; }

		public DaysToRepeat DaysToRepeat { get; set; }
		public TimeSpan Time { get; set; }

		public string Description { get; set; }

		public string SoundPath { get; set; }

		public double Volume
		{
			get => volume;
			set => volume = (value >= 0 && value <= 100) ? value : throw new ArgumentOutOfRangeException(nameof(value), "Volume must be between 0 and 100, inclusive.");
		}

		public Alarm() { }

		public Alarm(bool isEnabled, DaysToRepeat daysToRepeat, TimeSpan time, string description, string soundPath, double volume)
		{
			IsEnabled	  = isEnabled;
			DaysToRepeat  = daysToRepeat;
			Time		  = time;
			Description   = description;
			SoundPath	  = soundPath;
			Volume		  = volume;
		}

		public bool HasToBeSoundedToday
			=> DaysToRepeat.HasFlag(Enum.Parse<DaysToRepeat>(DateTime.Now.DayOfWeek.ToString()));

		public bool HasToBeSoundedNow
			=> Time.Hours == DateTime.Now.Hour && Time.Minutes == DateTime.Now.Minute;
	}
}
