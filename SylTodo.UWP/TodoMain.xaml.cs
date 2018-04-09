using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class TodoMain : Page {
        public static TodoMain Current;

        private string state;
        public string State { get { return state; } }

        public TodoMain() {
            this.InitializeComponent();
            Current = this;
            this.SizeChanged += StateManage;
            listFrame.Navigate(typeof(TodoList));
            detailFrame.Navigate(typeof(TodoDetail));
        }

        public bool StateChange(string newState) {
            if (newState == state) {
                return false;
            }
            switch (newState) {
                case "OnlyListState":
                    listColumn.Width = new GridLength(5, GridUnitType.Star);
                    detailColumn.Width = new GridLength(0);
                    break;
                case "OnlyDetailState":
                    listColumn.Width = new GridLength(0);
                    detailColumn.Width = new GridLength(4, GridUnitType.Star);
                    break;
                case "ListAndDetailState":
                    listColumn.Width = new GridLength(5, GridUnitType.Star);
                    detailColumn.Width = new GridLength(4, GridUnitType.Star);
                    break;
            }
            state = newState;
            return true;
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
    }
}
