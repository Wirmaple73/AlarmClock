using System;
using System.Windows;

namespace AlarmClock.Managers
{
	public static class ResourceManager
	{
		public static string GetResource(Resource resourceKey)
			=> Enum.IsDefined(resourceKey)
			? (string)Application.Current.FindResource(resourceKey.ToString())
			: throw new ArgumentOutOfRangeException(nameof(resourceKey), "The specified resource key could not be resolved.");

		public static string GetResourceFormatted(Resource resourceKey, params object[] arguments)
			=> string.Format(GetResource(resourceKey), arguments);
	}
}
