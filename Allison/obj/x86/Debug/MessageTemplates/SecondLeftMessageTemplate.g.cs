﻿#pragma checksum "E:\Projects\C#\Allison\Allison\MessageTemplates\SecondLeftMessageTemplate.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "2E635A465909E2EF06478577FF241FBCA2B564601469EFBBB947221C9A89A8C8"
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
    partial class SecondLeftMessageTemplate : 
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
            case 2: // MessageTemplates\SecondLeftMessageTemplate.xaml line 82
                {
                    this.SecondLeftMessageTime = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 3: // MessageTemplates\SecondLeftMessageTemplate.xaml line 18
                {
                    this.SecondLeftMessageGrid = (global::Windows.UI.Xaml.Controls.Grid)(target);
                }
                break;
            case 4: // MessageTemplates\SecondLeftMessageTemplate.xaml line 27
                {
                    this.SecondLeftMessageTextBlock = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                    ((global::Windows.UI.Xaml.Controls.TextBlock)this.SecondLeftMessageTextBlock).PointerEntered += this.TextBlock_PointerEntered;
                    ((global::Windows.UI.Xaml.Controls.TextBlock)this.SecondLeftMessageTextBlock).PointerExited += this.TextBlock_PointerExited;
                    ((global::Windows.UI.Xaml.Controls.TextBlock)this.SecondLeftMessageTextBlock).RightTapped += this.TextBlock_RightTapped;
                }
                break;
            case 5: // MessageTemplates\SecondLeftMessageTemplate.xaml line 39
                {
                    this.DeleteMenu = (global::Windows.UI.Xaml.Controls.MenuFlyout)(target);
                }
                break;
            case 6: // MessageTemplates\SecondLeftMessageTemplate.xaml line 45
                {
                    global::Windows.UI.Xaml.Controls.MenuFlyoutItem element6 = (global::Windows.UI.Xaml.Controls.MenuFlyoutItem)(target);
                    ((global::Windows.UI.Xaml.Controls.MenuFlyoutItem)element6).Click += this.CopyText_Click;
                }
                break;
            case 7: // MessageTemplates\SecondLeftMessageTemplate.xaml line 58
                {
                    global::Windows.UI.Xaml.Controls.MenuFlyoutItem element7 = (global::Windows.UI.Xaml.Controls.MenuFlyoutItem)(target);
                    ((global::Windows.UI.Xaml.Controls.MenuFlyoutItem)element7).Click += this.Delete_Click;
                }
                break;
            case 8: // MessageTemplates\SecondLeftMessageTemplate.xaml line 65
                {
                    global::Windows.UI.Xaml.Controls.MenuFlyoutItem element8 = (global::Windows.UI.Xaml.Controls.MenuFlyoutItem)(target);
                    ((global::Windows.UI.Xaml.Controls.MenuFlyoutItem)element8).Click += this.ClearAll_Click;
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

