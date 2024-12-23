using Allison.Model;
using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Input;
using Windows.ApplicationModel.DataTransfer;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Allison.MessageTemplates
{
    public sealed partial class SecondLeftMessageTemplate : UserControl
    {
        public static int MessageToRemoveFromListView = -1;

        public static int MessageToRemoveFromDatabase = -1;

        private string MessageToCopy;

        public SecondLeftMessageTemplate()
        {
            this.InitializeComponent();
            SecondLeftMessageTime.Visibility = Visibility.Collapsed;
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
            SecondLeftMessageTime.Visibility = Visibility.Visible;
            SecondLeftMessageGrid.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 205, 205, 205));
        }

        private void TextBlock_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            SecondLeftMessageTime.Visibility = Visibility.Collapsed;
            SecondLeftMessageGrid.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 230, 230, 230));
        }

        private void CopyText_Click(object sender, RoutedEventArgs e)
        {
            var datapackage = new DataPackage();
            datapackage.SetText(MessageToCopy.ToString());
            Clipboard.SetContent(datapackage);
        }
    }
}
