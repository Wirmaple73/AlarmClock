using System;
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

				try
				{
					AudioPlayer.Play(alarm.SoundLocation, true);
				}
				catch
				{
					AudioPlayer.Play(SoundLocation.Default, true);
				}
			}
		}

		public AlarmWindow() => InitializeComponent();

		private void Window_Closed(object sender, EventArgs e) => AudioPlayer.Stop();
	}
}
