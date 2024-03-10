using System;
using System.Globalization;
using System.Windows.Data;
using AlarmClock.Managers;

namespace AlarmClock.Converters
{
    [ValueConversion(typeof(bool), typeof(string))]
    public class StatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => ResourceManager.GetResource(bool.Parse(value.ToString()) ? Resource.MW_ListViewStatusEnabled : Resource.MW_ListViewStatusDisabled);

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}
