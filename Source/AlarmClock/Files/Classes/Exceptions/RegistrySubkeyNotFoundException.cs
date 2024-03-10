using System;

namespace AlarmClock
{
    public class RegistrySubkeyNotFoundException : Exception
    {
		private const string DefaultMessage = "The specified Registry subkey could not be found.";

		public string Subkey { get; }

		public RegistrySubkeyNotFoundException() : base(DefaultMessage) { }
		public RegistrySubkeyNotFoundException(string subkey) : base(DefaultMessage) => Subkey = subkey;
		public RegistrySubkeyNotFoundException(string message, string subkey) : base(message) => Subkey = subkey;
	}
}
