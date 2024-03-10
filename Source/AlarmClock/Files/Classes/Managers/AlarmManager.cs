using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Threading;
using System.Xml.Linq;

namespace AlarmClock.Managers
{
	public static class AlarmManager
    {
		private static readonly ObservableCollection<Alarm> alarms = new();
		private static readonly List<Alarm> activatedAlarms = new();

		private static readonly DispatcherTimer timer = new() { Interval = new(0, 0, 1) };

		public static ReadOnlyObservableCollection<Alarm> Alarms { get; } = new(alarms);

		public static event AlarmSoundedHandler AlarmSounded;

		static AlarmManager()
        {
            FileManager.Import();
            timer.Tick += Timer_Tick;
        }

        public static bool IsEnabled
        {
            get => timer.IsEnabled;
            set => timer.IsEnabled = value;
        }

		public static void Add(Alarm alarm) => alarms.Add(alarm);
		public static void Remove(Alarm alarm) => alarms.Remove(alarm);
		public static void Replace(Alarm subject, Alarm target) => alarms[alarms.IndexOf(subject)] = target;

		public static void SwapStatus(Alarm alarm) => alarm.IsEnabled = !alarm.IsEnabled;

		public static void ExportAlarms() => FileManager.Export();

        private static void Timer_Tick(object sender, EventArgs e)
        {
            // Make the recently triggered alarms available again
            for (int i = activatedAlarms.Count - 1; i >= 0; i--)
            {
                if (DateTime.Now.Hour != activatedAlarms[i].Time.Hours || DateTime.Now.Minute != activatedAlarms[i].Time.Minutes)
                    activatedAlarms.Remove(activatedAlarms[i]);
            }

            for (int i = 0; i < alarms.Count; i++)
            {
                var alarm = alarms[i];

                if (!alarm.IsEnabled || !alarm.HasToBeSoundedNow)
                    continue;

				if (alarm.DaysToRepeat != DaysToRepeat.None)
                {
                    // Ensure any 'Days to repeat' flag is set for today and also disallow triggering multiple alarms simultaneously
                    if (!alarm.HasToBeSoundedToday || activatedAlarms.Contains(alarm))
                        continue;

                    activatedAlarms.Add(alarm);
                    InvokeAlarmSoundedEvent(false);
                }
                else
                {
                    InvokeAlarmSoundedEvent(true);
                }

                void InvokeAlarmSoundedEvent(bool disableAlarm)
                {
                    if (disableAlarm)
                        alarm.IsEnabled = false;

                    AlarmSounded?.Invoke(null, new(alarm));
                }
            }
        }

        private static class FileManager
        {
            private static readonly string DirectoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "AlarmClock");
            private static readonly string FilePath = Path.Combine(DirectoryPath, "Alarms.xml");

            private const string IsEnabledCaption     = nameof(Alarm.IsEnabled);
            private const string DaysToRepeatCaption  = nameof(Alarm.DaysToRepeat);
            private const string TimeCaption          = nameof(Alarm.Time);
            private const string DescriptionCaption   = nameof(Alarm.Description);
            private const string SoundLocationCaption = nameof(Alarm.SoundLocation);

            internal static void Import()
            {
                if (!File.Exists(FilePath))
                    return;

                var document = XElement.Load(FilePath);

                foreach (var element in document.Elements())
                {
                    try
                    {
                        string soundLocation = GetElementValue<string>(SoundLocationCaption);

                        alarms.Add(new()
                        {
                            IsEnabled     = GetElementValue<bool>(IsEnabledCaption),
                            DaysToRepeat  = (DaysToRepeat)GetElementValue<int>(DaysToRepeatCaption),
                            Time          = TimeSpan.Parse(GetElementValue<string>(TimeCaption)),
                            Description   = GetElementValue<string>(DescriptionCaption),
                            SoundLocation = Enum.TryParse(soundLocation, out AlarmSound result) ? new(result) : new(soundLocation)
                        });
                    }
                    catch { /* Ignore import errors for now */ }

                    T GetElementValue<T>(string name) where T : IConvertible
                        => (T)Convert.ChangeType(element.Element(name).Value, typeof(T));
                }
            }

            internal static void Export()
            {
                if (!Directory.Exists(DirectoryPath))
                    Directory.CreateDirectory(DirectoryPath);

				// Remove the output file's hidden & read-only attributes
				if (File.Exists(FilePath))
                    File.SetAttributes(FilePath, File.GetAttributes(FilePath) & ~(FileAttributes.ReadOnly | FileAttributes.Hidden));

                new XDocument(
                    new XComment(" One small syntactical error can render the whole file unusable, thus edit it at your own risk "),
                    new XElement("Alarms",
                        GetAlarms()
                    )
                ).Save(FilePath);

                static IEnumerable<XElement> GetAlarms()
                {
                    foreach (var alarm in alarms)
                    {
                        yield return new("Alarm",
                            new XElement(IsEnabledCaption, alarm.IsEnabled.ToString()),
                            new XElement(DaysToRepeatCaption, (int)alarm.DaysToRepeat),
                            new XElement(TimeCaption, alarm.Time.ToString()),
                            new XElement(DescriptionCaption, alarm.Description),
                            new XElement(SoundLocationCaption, alarm.SoundLocation.IsFilePathDefined ? alarm.SoundLocation.FilePath : (int)alarm.SoundLocation.AlarmSound)
                        );
                    }
                }
            }
        }
    }
}
