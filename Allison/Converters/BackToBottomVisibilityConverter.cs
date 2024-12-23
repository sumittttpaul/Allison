using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Allison.Converters
{
    public class BackToBottomVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object paramter, string language)
        {
            if (value is bool && (bool)value)
                return Visibility.Visible;
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value is Visibility && (Visibility)value == Visibility.Visible;
        }
    }
}
