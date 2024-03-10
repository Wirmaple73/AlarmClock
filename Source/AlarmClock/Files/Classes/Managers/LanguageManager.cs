using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace AlarmClock.Managers
{
	public static class LanguageManager
	{
		// Property changed event source:
		// https://stackoverflow.com/a/52128987/18954775
		public static event PropertyChangedEventHandler StaticPropertyChanged;

		private static readonly PropertyChangedEventArgs FlowDirectionPropertyEventArgs = new(nameof(FlowDirection));

		private static Language language = Language.English;

		public static Language Language
		{
			get => language;
			set
			{
				if (!Enum.IsDefined(value))
					throw new ArgumentOutOfRangeException(nameof(value), "The specified language could not be resolved.");

				language = value;

				SetFlowDirection();
				ApplyLanguage();

				// Trigger the event handler to apply the new flow direction for all windows
				StaticPropertyChanged?.Invoke(null, FlowDirectionPropertyEventArgs);

				static void SetFlowDirection() => FlowDirection = language switch
				{
					Language.Persian => FlowDirection.RightToLeft,
					Language.English or _ => FlowDirection.LeftToRight
				};

				static void ApplyLanguage()
					=> Application.Current.Resources.MergedDictionaries.Add(new() { Source = new($"Resources/Languages/{language}.xaml", UriKind.Relative) });
			}
		}

		public static FlowDirection FlowDirection { get; private set; } = FlowDirection.LeftToRight;
	}
}
