using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace Allison.Converters
{
    public class MicToSendIconConverters : IValueConverter
    {
        public object Convert(object value, Type targetType, object paramter, string language)
        {
            if (value is bool && (bool)value)
                return new Uri("ms-appx:///Assets/Icons/microphone.png");
            return new Uri("ms-appx:///Assets/Icons/send.png");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return (value is Uri && (Uri)value == new Uri("ms-appx:///Assets/Icons/microphone.png"));
        }
    }
}
