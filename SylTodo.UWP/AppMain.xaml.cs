using SylTodo.UWP.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace SylTodo.UWP {
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class AppMain : Page {
        public static AppMain Current;
        public int Type;

        public BitmapImage BackgroundImage {
            set {
                if (value == null) {
                    background.ImageSource = null;
                } else {
                    background.ImageSource = value;
                }
            }
        }

        public AppMain() {
            this.InitializeComponent();
            Current = this;
            contentFrame.Navigate(typeof(TodoMain));
        }

        private async void NavigationView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args) {
            //先判断是否选中了setting
            if (args.IsSettingsInvoked) {
                return;
            } else {
                //选中项的内容
                switch (args.InvokedItem) {
                    case "所有":
                        Type = 0;
                        break;
                    case "今天":
                        Type = 1;
                        break;
                    case "最近7天":
                        Type = 2;
                        break;
                    case "已完成":
                        Type = 5;
                        break;
                    default:
                        Type = 0;
                        break;
                }
            }
        }

        private void AppBarButton_Click_Delete(object sender, RoutedEventArgs e) {
            if (TodoMain.Current.State == "OnlyListState") {
                TodoMain.Current.StateChange("OnlyDetailState");
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                       AppViewBackButtonVisibility.Visible;
            }
        }

        private void DisplayModeChanged(NavigationView sender, NavigationViewDisplayModeChangedEventArgs args) {
            if (args.DisplayMode == NavigationViewDisplayMode.Minimal) {
                TodoList.Current.SetHeaderVisibility("Minimal");
            } else {
                TodoList.Current.SetHeaderVisibility("Other");
            }
        }
    }
}
