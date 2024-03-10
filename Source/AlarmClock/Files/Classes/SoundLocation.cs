using System;
using System.IO;
using AlarmClock.Properties;

namespace AlarmClock
{
    public readonly struct SoundLocation
    {
        public static readonly SoundLocation Default = new(AlarmSound.DefaultAlarm);

        public AlarmSound AlarmSound { get; } = Default.AlarmSound;
        public string FilePath { get; } = null;

		public bool IsFilePathDefined => FilePath is not null;

		public Stream AlarmSoundStream
        {
            get
            {
                if (IsFilePathDefined)
                    throw new InvalidOperationException("Unable to fetch the stream of a path-supplied sound location.");

                return AlarmSound switch
                {
                    AlarmSound.DefaultAlarm => Resources.DefaultAlarm,
                    _ => throw new InvalidOperationException()
                };
            }
        }

        public SoundLocation(AlarmSound sound)
            => AlarmSound = Enum.IsDefined(sound) ? sound : throw new ArgumentOutOfRangeException(nameof(sound), "The specified alarm sound could not be resolved.");

		public SoundLocation(string filePath)
            => FilePath = !string.IsNullOrWhiteSpace(filePath) ? filePath : throw new ArgumentException("The specified path is empty, or only consists of whitespace.", nameof(filePath));
	}
}
