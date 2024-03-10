using System;
using Microsoft.Win32;

namespace AlarmClock.Managers
{
	public static class StartupManager
	{
		private const string Subkey	= @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
		private const string ProgramName = "Alarm Clock";

		// Source:
		// https://stackoverflow.com/a/675347/18954775
		public static void SetStartWithWindowsEnabled(bool enabled)
		{
			using (var key = Registry.CurrentUser.OpenSubKey(Subkey, true))
			{
				if (key is null)  // This normally shouldn't happen
					throw new RegistrySubkeyNotFoundException(Subkey);

				if (enabled)
					key.SetValue(ProgramName, Environment.ProcessPath);
				else
					key.DeleteValue(ProgramName, false);
			}
		}
	}
}
