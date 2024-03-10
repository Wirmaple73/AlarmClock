using System.Diagnostics;

namespace AlarmClock
{
	public static class URLOpener
	{
		public static void Open(string url) => Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
	}
}
