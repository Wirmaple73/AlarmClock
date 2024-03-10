using System;

namespace AlarmClock
{
    public class AlarmSoundedEventArgs : EventArgs
    {
        public Alarm Alarm { get; }

        public AlarmSoundedEventArgs(Alarm alarm) => Alarm = alarm;
    }
}
