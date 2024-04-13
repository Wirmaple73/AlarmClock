using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using AlarmClock.Managers;

namespace AlarmClock
{
	public partial class AlarmSettingsWindow : Window
	{
		private static readonly string DefaultSoundFilePath = Path.Combine(Environment.CurrentDirectory, @"Resources\Audio\Alarms\Default.wav");
		private static readonly TimeSpan DayDuration = new(24, 0, 0);

		private readonly OpenFileDialog fileDialog = new()
		{
			Title  = ResourceManager.GetResource(Resource.ASW_OpenFileDialogTitle),
			Filter = ResourceManager.GetResource(Resource.ASW_OpenFileDialogFilter),
			InitialDirectory = Path.GetDirectoryName(DefaultSoundFilePath),
			CheckFileExists = true
		};

		public Alarm Alarm { get; set; }

		public bool ModifyAlarm
		{
			get => modifyAlarm;
			init
			{
				this.Title = ResourceManager.GetResource(Resource.ASW_ModifyAlarm);

				if (value)
					ApplyAlarmModificationMode();

				modifyAlarm = value;

				void ApplyAlarmModificationMode()
				{
					// StartEnabled.IsChecked	 = Alarm.IsEnabled;  // Always enable the alarm regardless of its original state
					DaysToRepeat			 = Alarm.DaysToRepeat;
					TimeHour.SelectedIndex	 = Alarm.Time.Hours;
					TimeMinute.SelectedIndex = Alarm.Time.Minutes;
					Description.Text		 = Alarm.Description;

					fileDialog.FileName = SelectedAudioFilePath.Text = Alarm.SoundPath;
					Volume.Value = Alarm.Volume;
				}
			}
		}

		private bool modifyAlarm = false;

		private DaysToRepeat DaysToRepeat
		{
			get
			{
				var result = DaysToRepeat.None;

				foreach (CheckBox checkBox in SelectDaysToRepeat.Items)
				{
					if (checkBox.IsChecked.Value)
						result |= (DaysToRepeat)int.Parse((string)checkBox.Tag);
				}

				return result;
			}

			set
			{
				foreach (CheckBox checkBox in SelectDaysToRepeat.Items)
				{
					if (value.HasFlag((DaysToRepeat)int.Parse((string)checkBox.Tag)))
						checkBox.IsChecked = true;
				}
			}
		}

		public AlarmSettingsWindow()
		{
			InitializeComponent();
			ConfigureControls();

			this.Title = ResourceManager.GetResource(Resource.ASW_NewAlarm);

			void ConfigureControls()
			{
				TimeHour.ItemsSource   = GetTimeValues(23);
				TimeMinute.ItemsSource = GetTimeValues(59);

				TimeHour.SelectedIndex	 = DateTime.Now.Hour;
				TimeMinute.SelectedIndex = DateTime.Now.Minute;

				fileDialog.FileName = SelectedAudioFilePath.Text = DefaultSoundFilePath;

				IEnumerable<string> GetTimeValues(int maxValue)
				{
					for (int i = 0; i <= maxValue; i++)
						yield return i <= 9 ? "0" + i : i.ToString();  // Add a leading zero if necessary
				}
			}
		}

		private void SelectDaysToRepeat_DropDownClosed(object sender, EventArgs e)
		{
			SelectDaysToRepeat.Text = ResourceManager.GetResource(Resource.ASW_SelectDaysToRepeat);
			Time_SelectionChanged(this, null);
		}

		private void Time_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			// Only display the remaining time if no days are specified
			if (DaysToRepeat != DaysToRepeat.None)
			{
				TimeLeft.Text = "";
				return;
			}

			var currentTime  = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, 0);
			var selectedTime = new TimeSpan(TimeHour.SelectedIndex, TimeMinute.SelectedIndex, 0);

			var deltaTime = selectedTime - currentTime;

			// If the user has selected an earlier time, count the remaining time value toward tommorow
			if (selectedTime < currentTime)
				deltaTime += DayDuration;

			TimeLeft.Text = deltaTime != TimeSpan.Zero
				? ResourceManager.GetResourceFormatted(Resource.ASW_TimeFormat, deltaTime.Hours, deltaTime.Minutes)
				: ResourceManager.GetResource(Resource.ASW_TimeNow);
		}

		private void BrowseSoundLocation_Click(object sender, RoutedEventArgs e)
		{
			fileDialog.ShowDialog();
			SelectedAudioFilePath.Text = fileDialog.FileName;
		}

		private void Preview_Click(object sender, RoutedEventArgs e)
		{
			if (fileDialog.FileName.Length == 0)
			{
				MessageBoxManager.DisplayInformation(ResourceManager.GetResource(Resource.ASW_SoundNotSelected));
				return;
			}

			AudioPlayer.Play(fileDialog.FileName, Volume.Value);
		}

		private void OK_Click(object sender, RoutedEventArgs e)
		{
			if (fileDialog.FileName.Length == 0)
			{
				MessageBoxManager.DisplayInformation(ResourceManager.GetResource(Resource.ASW_SoundNotSelected));
				return;
			}

			Alarm = new()
			{
				IsEnabled	  = StartEnabled.IsChecked.Value,
				DaysToRepeat  = DaysToRepeat,
				Time		  = new(TimeHour.SelectedIndex, TimeMinute.SelectedIndex, 0),
				Description   = Description.Text.Length > 0 ? Description.Text : "~",
				SoundPath	  = new(fileDialog.FileName),
				Volume		  = Volume.Value
			};

			this.DialogResult = true;
			this.Close();
		}

		private void Window_Closed(object sender, EventArgs e) => AudioPlayer.Stop();
    }
}
