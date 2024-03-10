using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace AlarmClock.Managers
{
	public static class VersionManager
	{
		public static readonly Version CurrentVersion = new(1, 0, 0);
		public static readonly DateTime BuildDate = new(2024, 3, 10);

		public const string RepositoryURL = "https://www.github.com/Wirmaple73/AlarmClock";
		private const string LookupURL = "https://raw.githubusercontent.com/Wirmaple73/AlarmClock/main/CurrentVersion.txt";

		private static readonly HttpClient client = new();

		public static async Task<VersionLookupState> GetVersionLookupStateAsync()
		{
			try
			{
				return new Version(await client.GetStringAsync(LookupURL)) > CurrentVersion ?
					VersionLookupState.UpdateAvailable : VersionLookupState.UpToDate;
			}
			catch
			{
				return VersionLookupState.ConnectionError;
			}
		}
	}
}
