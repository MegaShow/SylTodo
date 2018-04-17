using SylTodo.Core;
using SylTodo.Core.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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

namespace SylTodo.UWP.Views {
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class TodoMain : Page {
        public static TodoMain Current;

        private string state;
        public string State { get { return state; } }

        public TodoMain() {
            this.InitializeComponent();
            Current = this;
            BackgroundChange(null);
            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;
            this.SizeChanged += StateManage;
            listFrame.Navigate(typeof(TodoList));
            detailFrame.Navigate(typeof(TodoDetail));
        }

        public void BackgroundChange(BitmapImage image) {
            AppMain.Current.BackgroundImage = image;
        }

        public bool StateChange(string newState) {
            if (newState == state) {
                return false;
            }
            SystemNavigationManager view = SystemNavigationManager.GetForCurrentView();
            switch (newState) {
                case "OnlyListState":
                    if (view.AppViewBackButtonVisibility == AppViewBackButtonVisibility.Visible) {
                        view.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
                    }
                    listColumn.Width = new GridLength(5, GridUnitType.Star);
                    detailColumn.Width = new GridLength(0);
                    break;
                case "OnlyDetailState":
                    if (view.AppViewBackButtonVisibility == AppViewBackButtonVisibility.Collapsed) {
                        view.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
                    }
                    listColumn.Width = new GridLength(0);
                    detailColumn.Width = new GridLength(4, GridUnitType.Star);
                    break;
                case "ListAndDetailState":
                    if (view.AppViewBackButtonVisibility == AppViewBackButtonVisibility.Visible) {
                        view.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
                    }
                    listColumn.Width = new GridLength(5, GridUnitType.Star);
                    detailColumn.Width = new GridLength(4, GridUnitType.Star);
                    break;
            }
            state = newState;
            return true;
        }

        public void StateFromListToDetail(TodoItem item) {
            TodoDetail.Current.StateChange("Edit");
            TodoDetail.Current.EditInit(item);
            double width = Window.Current.Bounds.Width;
            if (width >= 0 && width < 900) {
                StateChange("OnlyDetailState");
            }
        }

        public void StateFromDetailToList() {
            double width = Window.Current.Bounds.Width;
            if (width >= 0 && width < 900) {
                StateChange("OnlyListState");
            }
        }

        private void StateManage(object sender, SizeChangedEventArgs e) {
            string newState;
            if (e.NewSize.Width >= 0 && e.NewSize.Width < 900) {
                newState = state == "OnlyDetailState" ? "OnlyDetailState" : "OnlyListState";
            } else {
                newState = "ListAndDetailState";
            }
            if (newState != state) {
                StateChange(newState);
            }
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs e) {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                AppViewBackButtonVisibility.Collapsed;
            StateChange("OnlyListState");
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e) {
            base.OnNavigatedTo(e);
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("TodoMain.Current.State")) {
                string state = ApplicationData.Current.LocalSettings.Values["TodoMain.Current.State"] as string;
                if (state == "OnlyDetailState") {
                    ApplicationView.PreferredLaunchViewSize = new Size(800, 600);
                    ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
                    StateChange("OnlyDetailState");
                }
            }
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("SylTodo.Not.First.Open") == false) {
                if (Database.ViewModel.Collection.Count == 0) {
                    StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/background.png"));
                    byte[] bitmap = await Commons.Convert.ConvertImageToByte(file);
                    Database.ViewModel.Add("欢迎加入希娃清单", "从今天起，希娃清单将伴你开启时间管理之旅。", DateTime.Now, "收集箱", false, bitmap);
                    Database.ViewModel.Add("你好，世界", "Never underestimate the ability of a small group of dedicated people to " +
                        "change the world.Indeed, it is the only thing that ever has.", DateTime.Now, "收集箱", false, bitmap);
                    TodoList.Current.UpdateListViewEmptyVisibility();
                }
                ApplicationData.Current.LocalSettings.Values["SylTodo.Not.First.Open"] = true;
            }
        }
    }
}
