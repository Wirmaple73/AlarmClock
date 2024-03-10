using System.Media;

namespace AlarmClock
{
	public static class AudioPlayer
    {
        private static readonly SoundPlayer player = new();

        public static void Play(SoundLocation soundLocation, bool loop = false)
        {
            using (player)
            {
                if (soundLocation.IsFilePathDefined)
                    player.SoundLocation = soundLocation.FilePath;
                else
                    player.Stream = soundLocation.AlarmSoundStream;

                if (loop)
                    player.PlayLooping();
                else
                    player.Play();
            }
        }

        public static void Stop()
        {
            using (player)
                player.Stop();
        }
    }
}
