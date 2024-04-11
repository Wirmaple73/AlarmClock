using System;
using System.Windows.Media;

namespace AlarmClock
{
	public static class AudioPlayer
	{
		private static readonly MediaPlayer player = new();

		private static bool isLooped;

		static AudioPlayer() => player.MediaEnded += Player_MediaEnded;

		// AudioPlayer can currently play only one sound simultaneously
		public static void Play(string soundPath, double volume = 100, bool isLooped = false)
		{
			if (volume < 0 || volume > 100)
				throw new ArgumentOutOfRangeException(nameof(volume), "Volume must be between 0 and 100, inclusive.");

			player.Volume = volume / 100;  // Transfer the volume range from [0, 100] to [0, 1]

			player.Open(new Uri(soundPath, UriKind.Absolute));
			player.Play();

			AudioPlayer.isLooped = isLooped;
		}

		public static void Stop()
		{
			player.Stop();
			player.Close();
		}

		private static void Player_MediaEnded(object sender, EventArgs e)
		{
			// Code for looping taken from: https://stackoverflow.com/a/24320649/18954775

			if (isLooped)
			{
				player.Position = TimeSpan.Zero;
				player.Play();
			}
		}
	}
}
