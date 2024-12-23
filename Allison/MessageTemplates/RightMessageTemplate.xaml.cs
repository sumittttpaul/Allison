using Allison.Model;
using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Input;
using Windows.ApplicationModel.DataTransfer;

namespace Allison.MessageTemplates
{
    public sealed partial class RightMessageTemplate : UserControl
    {
        public static int MessageToRemoveFromListView = -1;

        public static int MessageToRemoveFromDatabase = -1;

        private string MessageToCopy;

        public RightMessageTemplate()
        {
            this.InitializeComponent();
            RightMessageTime.Visibility = Visibility.Collapsed;
        }

        private void TextBlock_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            e.Handled = true;
            var item = ((FrameworkElement)e.OriginalSource).DataContext as MessageBubble;
            MessageToRemoveFromListView = MainPage.Current.Cache1.IndexOf(item);
            MessageToRemoveFromDatabase = Convert.ToInt32(item.MessageBubbleId);
            MessageToCopy = Convert.ToString(item.Message);
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

        private void TextBlock_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            RightMessageTime.Visibility = Visibility.Visible;
            //LeftMessageTextBlock.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
            RightMessageGrid.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 230, 230, 230));
            RightMessageGrid.Background = new SolidColorBrush(Color.FromArgb(255, 230, 230, 230)); 
        }

        private void TextBlock_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            RightMessageTime.Visibility = Visibility.Collapsed;
            //LeftMessageTextBlock.Foreground = new SolidColorBrush(Color.FromArgb(255, 105, 105, 105));
            RightMessageGrid.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 240, 240, 240));
            RightMessageGrid.Background = new SolidColorBrush(Color.FromArgb(255, 240, 240, 240));
        }

        private void CopyText_Click(object sender, RoutedEventArgs e)
        {
            var datapackage = new DataPackage();
            datapackage.SetText(MessageToCopy.ToString());
            Clipboard.SetContent(datapackage);
        }
    }
}
