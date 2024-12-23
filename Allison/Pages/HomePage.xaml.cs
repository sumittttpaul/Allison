using Microsoft.Toolkit.Uwp.UI.Animations;
using System;
using System.Numerics;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace Allison.Pages
{
    public sealed partial class HomePage : Page
    {
        private bool AnimationBool = false;

        public static bool PageBool = false;

        public HomePage()
        {
            this.InitializeComponent();
            if (AddTaskButton.IsPressed == true || AddNotesButton.IsPressed == true || ShowListButton.IsPressed == true)
            {
                return;
            }
            else
            {
                AddTaskButton.LostFocus += AddTaskButton_LostFocus;
            }
        }

        private void AddTaskButton_LostFocus(object sender, RoutedEventArgs e)
        {
            BottomAnimation();
        }

        private void TextButton_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            TextButton.Foreground = new SolidColorBrush(Colors.Gray);
        }

        private void TextButton_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            TextButton.Foreground = new SolidColorBrush(Colors.LightGray);
        }

        private void MicButton_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            MicButton.Foreground = new SolidColorBrush(Colors.Gray);
        }

        private void MicButton_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            MicButton.Foreground = new SolidColorBrush(Colors.LightGray);
        }

        private void BottomGrid_Loaded(object sender, RoutedEventArgs e)
        {
            var compositor = ElementCompositionPreview.GetElementVisual(Host).Compositor;
            var dropShadow = compositor.CreateDropShadow();
            dropShadow.Color = Colors.DarkGray;
            dropShadow.BlurRadius = 20;
            dropShadow.Opacity = 20.0f;
            var mask = BottomRectangleShadow.GetAlphaMask();
            dropShadow.Mask = mask;
            var spriteVisual = compositor.CreateSpriteVisual();
            spriteVisual.Size = new Vector2((float)Host.ActualWidth, (float)Host.ActualHeight);
            spriteVisual.Shadow = dropShadow;
            ElementCompositionPreview.SetElementChildVisual(Host, spriteVisual);
        }

        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            BottomAnimation();
        }

        private async void BottomAnimation()
        {
            if (AnimationBool == true)
            {
                if (FirstCircleRotate.Angle == 180 && SecondCircleRotate.Angle == 180 && ThirdCircleRotate.Angle == 180)
                {
                    AddTaskButton.Rotate(value: 0.0f, centerX: 0.0f, centerY: 0.0f, duration: 500, delay: 10, easingType: EasingType.Default).Start();
                    AddTaskButton.Scale(scaleX: 1, scaleY: 1, centerX: 0, centerY: 0, duration: 500, delay: 10, easingType: EasingType.Default).Start();

                    thirdAnimateProgress(ThirdCircleRotate.Angle - 180);
                    await Task.Delay(TimeSpan.FromSeconds(0.1));
                    SecondAnimateProgress(SecondCircleRotate.Angle - 180);
                    await Task.Delay(TimeSpan.FromSeconds(0.1));
                    AnimateProgress(FirstCircleRotate.Angle - 180);
                }
                AnimationBool = false;
                return;
            }
            if (AnimationBool == false)
            {
                if ((FirstCircleRotate.Angle == 0 && SecondCircleRotate.Angle == 0 && ThirdCircleRotate.Angle == 0) || (FirstCircleRotate.Angle == -180 && SecondCircleRotate.Angle == -180 && ThirdCircleRotate.Angle == -180))
                {
                    AddTaskButton.Rotate(value: 45.0f, centerX: 0.0f, centerY: 0.0f, duration: 500, delay: 10, easingType: EasingType.Default).Start();
                    AddTaskButton.Scale(scaleX: 0.8f, scaleY: 0.8f, centerX: 0, centerY: 0, duration: 500, delay: 10, easingType: EasingType.Default).Start();

                    AnimateProgress(FirstCircleRotate.Angle + 180);
                    await Task.Delay(TimeSpan.FromSeconds(0.1));
                    SecondAnimateProgress(SecondCircleRotate.Angle + 180);
                    await Task.Delay(TimeSpan.FromSeconds(0.1));
                    thirdAnimateProgress(ThirdCircleRotate.Angle + 180);
                }
                AnimationBool = true;
            }
        }

        private void AddTaskButton_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            AddTaskButton.Foreground = new SolidColorBrush(Colors.White);
        }

        private void AddTaskButton_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            AddTaskButton.Foreground = new SolidColorBrush(Colors.White);
        }

        private void AnimateProgress(double to, double miliseconds = 350)
        {
            var storyboard = new Storyboard();
            var doubleAnimation = new DoubleAnimation();
            doubleAnimation.Duration = TimeSpan.FromMilliseconds(miliseconds);
            doubleAnimation.EnableDependentAnimation = true;
            doubleAnimation.To = to;
            Storyboard.SetTargetProperty(doubleAnimation, "Angle");
            Storyboard.SetTarget(doubleAnimation, FirstCircleRotate);
            storyboard.Children.Add(doubleAnimation);
            storyboard.Begin();
        }

        private void SecondAnimateProgress(double to, double miliseconds = 350)
        {
            var storyboard = new Storyboard();
            var doubleAnimation = new DoubleAnimation();
            doubleAnimation.Duration = TimeSpan.FromMilliseconds(miliseconds);
            doubleAnimation.EnableDependentAnimation = true;
            doubleAnimation.To = to;
            Storyboard.SetTargetProperty(doubleAnimation, "Angle");
            Storyboard.SetTarget(doubleAnimation, SecondCircleRotate);
            storyboard.Children.Add(doubleAnimation);
            storyboard.Begin();
        }

        private void thirdAnimateProgress(double to, double miliseconds = 350)
        {
            var storyboard = new Storyboard();
            var doubleAnimation = new DoubleAnimation();
            doubleAnimation.Duration = TimeSpan.FromMilliseconds(miliseconds);
            doubleAnimation.EnableDependentAnimation = true;
            doubleAnimation.To = to;
            Storyboard.SetTargetProperty(doubleAnimation, "Angle");
            Storyboard.SetTarget(doubleAnimation, ThirdCircleRotate);
            storyboard.Children.Add(doubleAnimation);
            storyboard.Begin();
        }

        private void ShowListButton_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            ShowListButton.Foreground = new SolidColorBrush(Colors.DimGray);
        }

        private void ShowListButton_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            ShowListButton.Foreground = new SolidColorBrush(Colors.DarkGray);
        }

        private void SetReminderButton_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            SetReminderButton.Foreground = new SolidColorBrush(Colors.DimGray);
        }

        private void SetReminderButton_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            SetReminderButton.Foreground = new SolidColorBrush(Colors.DarkGray);
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 1);
        }

        private void AddNotesButton_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            AddNotesButton.Foreground = new SolidColorBrush(Colors.DimGray);
        }

        private void AddNotesButton_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            AddNotesButton.Foreground = new SolidColorBrush(Colors.DarkGray);
        }

        private void AddNotesButton_Click(object sender, RoutedEventArgs e)
        {
            BottomAnimation();
        }

        private void SetReminderButton_Click(object sender, RoutedEventArgs e)
        {
            BottomAnimation();
        }

        private void ShowListButton_Click(object sender, RoutedEventArgs e)
        {
            BottomAnimation();
        }

        private void TextButton_Click(object sender, RoutedEventArgs e)
        {
            PageBool = true;
            Frame frame = Window.Current.Content as Frame;
            frame.Navigate(typeof(MainCenterPage), null, new SuppressNavigationTransitionInfo());
        }

        private void MicButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}