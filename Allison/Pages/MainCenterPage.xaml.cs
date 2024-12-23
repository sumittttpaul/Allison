using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace Allison.Pages
{
    public sealed partial class MainCenterPage : Page
    {
        private bool AnimationBool = false;

        private bool HomeBool;

        private bool MessageBool;

        private bool SettingBool;

        private bool AboutBool;

        public MainCenterPage()
        {
            this.InitializeComponent();
            if (HomePage.PageBool == true)
            {
                PageChangingGridFrame.Navigate(typeof(MainPage), null, new EntranceNavigationTransitionInfo());
                MessageBool = true;
            }
            else
            {
                PageChangingGridFrame.Navigate(typeof(HomePage), null, new EntranceNavigationTransitionInfo());
                HomeBool = true;
            }
            MenuButton.LostFocus += MenuButton_LostFocus;
            SubMenuButtonFocusColor();
        }

        private void MenuButton_LostFocus(object sender, RoutedEventArgs e)
        {
            if (AnimationBool == true)
            {
                MenuPopup.IsOpen = false;
                if (MenuButtonRotate.Angle == -90)
                {
                    MenuButtonAnimation(MenuButtonRotate.Angle + 90);
                }
                if (MenuButtonRotate.Angle == 90)
                {
                    MenuButtonAnimation(MenuButtonRotate.Angle - 90);
                }
                if (MenuButtonRotate.Angle == -180)
                {
                    MenuButtonAnimation(MenuButtonRotate.Angle + 180);
                }
                if (MenuButtonRotate.Angle == 180)
                {
                    MenuButtonAnimation(MenuButtonRotate.Angle - 180);
                }
                AnimationBool = false;
            }
        }

        private void MenuButton_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            MenuButton.Foreground = new SolidColorBrush(Colors.Black);
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Hand, 1);
            if (MenuButtonRotate.Angle == 0 || MenuButtonRotate.Angle == -180)
            {
                MenuButtonAnimation(MenuButtonRotate.Angle - 90);
            }
        }

        private void MenuButton_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            MenuButton.Foreground = new SolidColorBrush(Colors.DarkGray);
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 1);
            if (MenuButtonRotate.Angle == -90 || MenuButtonRotate.Angle == 180)
            {
                MenuButtonAnimation(MenuButtonRotate.Angle + 90);
            }
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            if (AnimationBool == true)
            {
                MenuPopup.IsOpen = false;
                if (MenuButtonRotate.Angle == 90)
                {
                    MenuButtonAnimation(MenuButtonRotate.Angle - 180);
                }
                AnimationBool = false;
                return;
            }
            if (AnimationBool == false)
            {
                if (MenuButtonRotate.Angle == -90)
                {
                    MenuButtonAnimation(MenuButtonRotate.Angle + 180);
                }
                MenuPopup.IsOpen = true;
                AnimationBool = true;
            }

        }

        private void MenuButtonAnimation(double to, double miliseconds = 350)
        {
            var storyboard = new Storyboard();
            var doubleAnimation = new DoubleAnimation();
            doubleAnimation.Duration = TimeSpan.FromMilliseconds(miliseconds);
            doubleAnimation.EnableDependentAnimation = true;
            doubleAnimation.To = to;
            Storyboard.SetTargetProperty(doubleAnimation, "Angle");
            Storyboard.SetTarget(doubleAnimation, MenuButtonRotate);
            storyboard.Children.Add(doubleAnimation);
            storyboard.Begin();
        }

        private void Homebutton_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            Homebutton.Foreground = new SolidColorBrush(Colors.Black);
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Hand, 1);
        }

        private void Homebutton_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 1);
            if (HomeBool == true)
            {
                Homebutton.Foreground = new SolidColorBrush(Colors.Black);
            }
            if (HomeBool == false)
            {
                Homebutton.Foreground = new SolidColorBrush(Colors.DarkGray);
            }
        }

        private void Messagebutton_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            Messagebutton.Foreground = new SolidColorBrush(Colors.Black);
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Hand, 1);
        }

        private void Messagebutton_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 1);
            if (MessageBool == true)
            {
                Messagebutton.Foreground = new SolidColorBrush(Colors.Black);
            }
            if (MessageBool == false)
            {
                Messagebutton.Foreground = new SolidColorBrush(Colors.DarkGray);
            }
        }

        private void Settingbutton_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            Settingbutton.Foreground = new SolidColorBrush(Colors.Black);
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Hand, 1);
        }

        private void Settingbutton_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 1);
            if (SettingBool == true)
            {
                Settingbutton.Foreground = new SolidColorBrush(Colors.Black);
            }
            if (SettingBool == false)
            {
                Settingbutton.Foreground = new SolidColorBrush(Colors.DarkGray);
            }
        }

        private void Aboutbutton_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            Aboutbutton.Foreground = new SolidColorBrush(Colors.Black);
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Hand, 1);
        }

        private void Aboutbutton_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 1);
            if (AboutBool == true)
            {
                Aboutbutton.Foreground = new SolidColorBrush(Colors.Black);
            }
            if (AboutBool == false)
            {
                Aboutbutton.Foreground = new SolidColorBrush(Colors.DarkGray);
            }
        }

        private void Homebutton_Click(object sender, RoutedEventArgs e)
        {
            HomeBool = false;
            PageChangingGridFrame.Navigate(typeof(HomePage), null, new EntranceNavigationTransitionInfo());
            MenuButtonAnimation();
            MenuPopup.IsOpen = false;
            AnimationBool = false;
            if (HomeBool == false)
            {
                HomeBool = true;
                MessageBool = false;
                SettingBool = false;
                AboutBool = false;
            }
            SubMenuButtonFocusColor();
        }

        private void Messagebutton_Click(object sender, RoutedEventArgs e)
        {
            MessageBool = false;
            PageChangingGridFrame.Navigate(typeof(MainPage), null, new EntranceNavigationTransitionInfo());
            MenuButtonAnimation();
            MenuPopup.IsOpen = false;
            AnimationBool = false;
            if (MessageBool == false)
            {
                HomeBool = false;
                MessageBool = true;
                SettingBool = false;
                AboutBool = false;
            }
            SubMenuButtonFocusColor();
        }

        private void Settingbutton_Click(object sender, RoutedEventArgs e)
        {
            SettingBool = false;
            MenuButtonAnimation();
            MenuPopup.IsOpen = false;
            AnimationBool = false;
            if (SettingBool == false)
            {
                HomeBool = false;
                MessageBool = false;
                SettingBool = true;
                AboutBool = false;
            }
            SubMenuButtonFocusColor();
        }

        private void Aboutbutton_Click(object sender, RoutedEventArgs e)
        {
            AboutBool = false;
            MenuButtonAnimation();
            MenuPopup.IsOpen = false;
            AnimationBool = false;
            if (AboutBool == false)
            {
                HomeBool = false;
                MessageBool = false;
                SettingBool = false;
                AboutBool = true;
            }
            SubMenuButtonFocusColor();
        }

        private void MenuButtonAnimation()
        {
            if (MenuButtonRotate.Angle == -90)
            {
                MenuButtonAnimation(MenuButtonRotate.Angle + 90);
            }
            if (MenuButtonRotate.Angle == 90)
            {
                MenuButtonAnimation(MenuButtonRotate.Angle - 90);
            }
            if (MenuButtonRotate.Angle == -180)
            {
                MenuButtonAnimation(MenuButtonRotate.Angle + 180);
            }
            if (MenuButtonRotate.Angle == 180)
            {
                MenuButtonAnimation(MenuButtonRotate.Angle - 180);
            }
        }

        private void SubMenuButtonFocusColor()
        {
            if (HomeBool == true)
            {
                Homebutton.Foreground = new SolidColorBrush(Colors.Black);
                Messagebutton.Foreground = new SolidColorBrush(Colors.DarkGray);
                Settingbutton.Foreground = new SolidColorBrush(Colors.DarkGray);
                Aboutbutton.Foreground = new SolidColorBrush(Colors.DarkGray);
            }

            if (MessageBool == true)
            {
                Homebutton.Foreground = new SolidColorBrush(Colors.DarkGray);
                Messagebutton.Foreground = new SolidColorBrush(Colors.Black);
                Settingbutton.Foreground = new SolidColorBrush(Colors.DarkGray);
                Aboutbutton.Foreground = new SolidColorBrush(Colors.DarkGray);
            }

            if (SettingBool == true)
            {
                Homebutton.Foreground = new SolidColorBrush(Colors.DarkGray);
                Messagebutton.Foreground = new SolidColorBrush(Colors.DarkGray);
                Settingbutton.Foreground = new SolidColorBrush(Colors.Black);
                Aboutbutton.Foreground = new SolidColorBrush(Colors.DarkGray);
            }

            if (AboutBool == true)
            {
                Homebutton.Foreground = new SolidColorBrush(Colors.DarkGray);
                Messagebutton.Foreground = new SolidColorBrush(Colors.DarkGray);
                Settingbutton.Foreground = new SolidColorBrush(Colors.DarkGray);
                Aboutbutton.Foreground = new SolidColorBrush(Colors.Black);
            }
        }
    }
}
