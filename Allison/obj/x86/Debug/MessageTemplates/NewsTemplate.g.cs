﻿#pragma checksum "E:\Projects\C#\Allison\Allison\MessageTemplates\NewsTemplate.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "5BE5A4468CB4CE3F28D6DE6CF14EBAE1C5ADCF0E7B85BB82F8BB45297855147B"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Allison.MessageTemplates
{
    partial class NewsTemplate : 
        global::Windows.UI.Xaml.Controls.UserControl, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.19041.1")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 18: // MessageTemplates\NewsTemplate.xaml line 197
                {
                    global::Windows.UI.Xaml.Controls.Grid element18 = (global::Windows.UI.Xaml.Controls.Grid)(target);
                    ((global::Windows.UI.Xaml.Controls.Grid)element18).Loaded += this.MainGrid_Loaded;
                }
                break;
            case 19: // MessageTemplates\NewsTemplate.xaml line 203
                {
                    this.MainGrid = (global::Windows.UI.Xaml.Controls.Grid)(target);
                }
                break;
            case 20: // MessageTemplates\NewsTemplate.xaml line 206
                {
                    this.ShadowHost = (global::Windows.UI.Xaml.Controls.Canvas)(target);
                }
                break;
            case 21: // MessageTemplates\NewsTemplate.xaml line 207
                {
                    this.GetVideoShadow = (global::Windows.UI.Xaml.Shapes.Rectangle)(target);
                }
                break;
            case 22: // MessageTemplates\NewsTemplate.xaml line 208
                {
                    this.LeftMessageGrid = (global::Windows.UI.Xaml.Controls.Grid)(target);
                    ((global::Windows.UI.Xaml.Controls.Grid)this.LeftMessageGrid).RightTapped += this.Grid_RightTapped;
                    ((global::Windows.UI.Xaml.Controls.Grid)this.LeftMessageGrid).PointerEntered += this.Grid_PointerEntered;
                    ((global::Windows.UI.Xaml.Controls.Grid)this.LeftMessageGrid).PointerExited += this.Grid_PointerExited;
                }
                break;
            case 23: // MessageTemplates\NewsTemplate.xaml line 219
                {
                    this.DeleteMenu = (global::Windows.UI.Xaml.Controls.MenuFlyout)(target);
                }
                break;
            case 24: // MessageTemplates\NewsTemplate.xaml line 231
                {
                    global::Windows.UI.Xaml.Controls.MenuFlyoutItem element24 = (global::Windows.UI.Xaml.Controls.MenuFlyoutItem)(target);
                    ((global::Windows.UI.Xaml.Controls.MenuFlyoutItem)element24).Click += this.Delete_Click;
                }
                break;
            case 25: // MessageTemplates\NewsTemplate.xaml line 238
                {
                    global::Windows.UI.Xaml.Controls.MenuFlyoutItem element25 = (global::Windows.UI.Xaml.Controls.MenuFlyoutItem)(target);
                    ((global::Windows.UI.Xaml.Controls.MenuFlyoutItem)element25).Click += this.ClearAll_Click;
                }
                break;
            case 26: // MessageTemplates\NewsTemplate.xaml line 251
                {
                    this.VideoButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.VideoButton).Click += this.VideoButton_Click;
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        /// <summary>
        /// GetBindingConnector(int connectionId, object target)
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.19041.1")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}
