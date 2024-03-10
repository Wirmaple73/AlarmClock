using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;
using AlarmClock.Managers;

namespace AlarmClock.Converters
{
	[ValueConversion(typeof(DaysToRepeat), typeof(string))]
	public class DaysToRepeatConverter : IValueConverter
	{
		private static readonly Dictionary<DaysToRepeat, Resource> DayCaptionResources = new()
		{
			{ DaysToRepeat.Saturday,  Resource.ASW_Saturday  },
			{ DaysToRepeat.Sunday,    Resource.ASW_Sunday    },
			{ DaysToRepeat.Monday,    Resource.ASW_Monday    },
			{ DaysToRepeat.Tuesday,   Resource.ASW_Tuesday   },
			{ DaysToRepeat.Wednesday, Resource.ASW_Wednesday },
			{ DaysToRepeat.Thursday,  Resource.ASW_Thursday  },
			{ DaysToRepeat.Friday,    Resource.ASW_Friday    },
			{ DaysToRepeat.None,      Resource.ASW_Once      }
		};

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var sb = new StringBuilder(((DaysToRepeat)value).ToString());

			// Translate the day captions to the current language
			foreach (var (day, newValue) in DayCaptionResources)
				sb.Replace(day.ToString(), ResourceManager.GetResource(newValue));

			// Replace the comma with the Arabic one if necessary
			return LanguageManager.FlowDirection == FlowDirection.LeftToRight ? sb.ToString() : sb.Replace(',', '،');
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
			=> throw new NotSupportedException();
	}
}
