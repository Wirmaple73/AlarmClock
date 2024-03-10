using System;

namespace AlarmClock
{
	public class Alarm
	{
		public bool IsEnabled { get; set; }

		public DaysToRepeat DaysToRepeat { get; set; }
		public TimeSpan Time { get; set; }

		public string Description { get; set; }
		public SoundLocation SoundLocation { get; set; }

		public Alarm() { }

		public Alarm(bool isEnabled, DaysToRepeat daysToRepeat, TimeSpan time, string description, SoundLocation soundLocation)
		{
			IsEnabled	  = isEnabled;
			DaysToRepeat  = daysToRepeat;
			Time		  = time;
			Description   = description;
			SoundLocation = soundLocation;
		}

		public bool HasToBeSoundedToday
			=> DaysToRepeat.HasFlag(Enum.Parse<DaysToRepeat>(DateTime.Now.DayOfWeek.ToString()));

		public bool HasToBeSoundedNow
			=> Time.Hours == DateTime.Now.Hour && Time.Minutes == DateTime.Now.Minute;
	}
}
