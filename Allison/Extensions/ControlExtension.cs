using System;
using System.Reflection;
using Windows.UI.Xaml.Controls;

namespace Allison.Extensions
{
    public static class ControlExtension
    {
        public static void DoubleBuffering(this Control control, bool enable)
        {
            try
            {
                var doubleBufferPropertyInfo = control.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
                doubleBufferPropertyInfo.SetValue(control, enable);
            }
            catch (Exception)
            {
                return;
            }
        }
    }
}
