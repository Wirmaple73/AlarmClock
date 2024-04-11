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
		private static readonly TimeSpan DayDuration = new(24, 0, 0);

		private readonly OpenFileDialog fileDialog = new()
		{
			Title  = ResourceManager.GetResource(Resource.ASW_OpenFileDialogTitle),
			Filter = ResourceManager.GetResource(Resource.ASW_OpenFileDialogFilter),
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

					if (Alarm.SoundLocation.IsFilePathDefined)
					{
						fileDialog.FileName	= Alarm.SoundLocation.FilePath;
						SelectedAudioFilePath.Text = fileDialog.FileName;

						UseCustomSoundLocation.IsChecked = true;
						SelectSoundLocation.IsEnabled = false;
						BrowseSoundLocation.IsEnabled = true;
					}
					else
					{
						SelectedAlarmSound = Alarm.SoundLocation.AlarmSound;
					}
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

		private AlarmSound SelectedAlarmSound
		{
			get => SelectSoundLocation.SelectedIndex switch
			{
				0 => AlarmSound.DefaultAlarm,
				_ => throw new InvalidOperationException()
			};

			set => SelectSoundLocation.SelectedIndex = value switch
			{
				AlarmSound.DefaultAlarm => 0,
				_ => throw new ArgumentOutOfRangeException(nameof(value), "The specified alarm sound could not be resolved.")
			};
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

			var timeDifference = selectedTime - currentTime;

			// If the user has selected an earlier time, count the remaining time value toward tommorow
			if (selectedTime < currentTime)
				timeDifference += DayDuration;

			TimeLeft.Text = timeDifference != TimeSpan.Zero
				? ResourceManager.GetResourceFormatted(Resource.ASW_TimeFormat, timeDifference.Hours, timeDifference.Minutes)
				: ResourceManager.GetResource(Resource.ASW_TimeNow);
		}

		private void UseCustomSoundLocation_Click(object sender, RoutedEventArgs e)
		{
			SelectSoundLocation.IsEnabled = !UseCustomSoundLocation.IsChecked.Value;
			BrowseSoundLocation.IsEnabled = UseCustomSoundLocation.IsChecked.Value;

			SelectedAudioFilePath.Text = UseCustomSoundLocation.IsChecked.Value ? fileDialog.FileName : "";
		}

		private void BrowseSoundLocation_Click(object sender, RoutedEventArgs e)
		{
			fileDialog.ShowDialog();
			SelectedAudioFilePath.Text = fileDialog.FileName;
		}

		private void Preview_Click(object sender, RoutedEventArgs e)
		{
			if (!UseCustomSoundLocation.IsChecked.Value)
			{
				AudioPlayer.Play(new(SelectedAlarmSound));
				return;
			}

			if (fileDialog.FileName.Length == 0)
			{
				MessageBoxManager.DisplayInformation(ResourceManager.GetResource(Resource.ASW_SoundNotSelected));
				return;
			}

			try
			{
				AudioPlayer.Play(new(fileDialog.FileName));
			}
			catch (FileNotFoundException)
			{
				MessageBoxManager.DisplayError(ResourceManager.GetResource(Resource.ASW_AudioFileNotFound));
			}
			catch (InvalidOperationException)
			{
				MessageBoxManager.DisplayError(ResourceManager.GetResource(Resource.ASW_AudioFileInvalid));
			}
		}

		private void OK_Click(object sender, RoutedEventArgs e)
		{
			if (UseCustomSoundLocation.IsChecked.Value && fileDialog.FileName.Length == 0)
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
				SoundLocation = UseCustomSoundLocation.IsChecked.Value ? new(fileDialog.FileName) : new(SelectedAlarmSound)
			};

			this.DialogResult = true;
			this.Close();
		}

		private void Window_Closed(object sender, EventArgs e) => AudioPlayer.Stop();
    }
}
