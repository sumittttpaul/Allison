using Allison.Model;
using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Input;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Composition;
using Windows.System;
using Windows.UI.Xaml.Data;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Allison.MessageTemplates
{
    public sealed partial class YoutubeTemplate : UserControl
    {
        public static int MessageToRemoveFromListView = -1;

        public static int MessageToRemoveFromDatabase = -1;

        private string url;

        public YoutubeTemplate()
        {
            this.InitializeComponent();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            MainPage.Current.Cache1.RemoveAt(MessageToRemoveFromListView);
            DeleteMessageById(MessageToRemoveFromDatabase);
            MessageToRemoveFromListView = -1;
            MessageToRemoveFromDatabase = -1;
        }

        private void ClearAll_Click(object sender, RoutedEventArgs e)
        {
            MainPage.GetPopup.IsOpen = true;
            MainPage.GetOverlayGrid.Visibility = Visibility.Visible;
        }

        public static void DeleteMessageById(int id)
        {
            using (var db = new AllisonContext())
            {
                db.MessageBubble.Remove(db.MessageBubble.Find(id));
                db.SaveChanges();
            }
        }

        private void Grid_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            e.Handled = true;
            var item = ((FrameworkElement)e.OriginalSource).DataContext as MessageBubble;
            MessageToRemoveFromListView = MainPage.Current.Cache1.IndexOf(item);
            MessageToRemoveFromDatabase = Convert.ToInt32(item.MessageBubbleId);
        }

        private void GetShadow(UIElement shadowHost, Shape shadowTarget)
        {
            Visual hostVisual = ElementCompositionPreview.GetElementVisual(shadowHost);
            Compositor compositor = hostVisual.Compositor;
            var sprite = compositor.CreateSpriteVisual();
            sprite.Size = hostVisual.Size;
            sprite.CenterPoint = hostVisual.CenterPoint;
            var dropShadow = compositor.CreateDropShadow();
            sprite.Shadow = dropShadow;
            dropShadow.Color = Colors.Gray;
            dropShadow.BlurRadius = 10;
            dropShadow.Offset = new System.Numerics.Vector3(0, 0, 10);
            dropShadow.Opacity = 10.0f;
            dropShadow.Mask = shadowTarget.GetAlphaMask();
            var shadowVisual = compositor.CreateSpriteVisual();
            shadowVisual.Shadow = dropShadow;
            ElementCompositionPreview.SetElementChildVisual(shadowHost, shadowVisual);
            var bindSizeAnimation = compositor.CreateExpressionAnimation("hostVisual.Size");
            bindSizeAnimation.SetReferenceParameter("hostVisual", hostVisual);
            shadowVisual.StartAnimation("Size", bindSizeAnimation);
        }

        private void MainGrid_Loaded(object sender, RoutedEventArgs e)
        {
            GetShadow(ShadowHost, GetVideoShadow);
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("https://www.youtube.com/"));
        }

        private void Grid_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            var item = ((FrameworkElement)e.OriginalSource).DataContext as MessageBubble;
            url = item.VideoUrl;
        }

        private void Grid_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            url = null;
        }

        private async void VideoButton_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri(url));
        }
    }
}
