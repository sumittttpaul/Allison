using Allison.Model;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Allison.CustomControls
{
    public class CustomListView : ListView
    {
        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            var listViewItem = (ListViewItem)element;
            var msgContainer = (MessageBubble)item;
           
            if (msgContainer.To)
            {
                listViewItem.CornerRadius = new CornerRadius(0);
                listViewItem.Padding = new Thickness(0);
                listViewItem.Margin = new Thickness(10, 0, 10, 0);
                listViewItem.MaxWidth = 600;
                listViewItem.BorderThickness = new Thickness(0);
                listViewItem.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                listViewItem.AllowFocusOnInteraction = true;
                listViewItem.IsDoubleTapEnabled = true;
                listViewItem.IsHitTestVisible = true;
                listViewItem.IsHoldingEnabled = true;
                listViewItem.IsRightTapEnabled = true;
                listViewItem.IsTapEnabled = true;
                listViewItem.HorizontalAlignment = HorizontalAlignment.Right;
                listViewItem.HorizontalContentAlignment = HorizontalAlignment.Right;
            }
            else
            {
                listViewItem.CornerRadius = new CornerRadius(0);
                listViewItem.Padding = new Thickness(0);
                listViewItem.Margin = new Thickness(10, 0, 10, 0);
                listViewItem.MaxWidth = 700;
                listViewItem.BorderThickness = new Thickness(0);
                listViewItem.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                listViewItem.AllowFocusOnInteraction = true;
                listViewItem.IsDoubleTapEnabled = true;
                listViewItem.IsHitTestVisible = true;
                listViewItem.IsHoldingEnabled = true;
                listViewItem.IsRightTapEnabled = true;
                listViewItem.IsTapEnabled = true;
                listViewItem.HorizontalAlignment = HorizontalAlignment.Left;
                listViewItem.HorizontalContentAlignment = HorizontalAlignment.Stretch;
            }

            base.PrepareContainerForItemOverride(element, item);
        }
    }
}