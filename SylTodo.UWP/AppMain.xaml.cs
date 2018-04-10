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

        public BitmapImage BackgroundImage {
            set {
                if (value == null) {
                    CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = false;
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
