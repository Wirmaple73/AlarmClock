using System;
using System.IO;
using System.Windows;

namespace AlarmClock
{
	public partial class AlarmWindow : Window
	{
		private Alarm alarm;

		public Alarm Alarm
		{
			get => alarm;
			init
			{
				alarm = value;
				AudioPlayer.Play(alarm.SoundPath, alarm.Volume, true);
			}
		}

		public AlarmWindow() => InitializeComponent();

		private void Window_Closed(object sender, EventArgs e) => AudioPlayer.Stop();
	}
}
