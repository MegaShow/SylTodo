using SylTodo.Core;
using SylTodo.Core.Models;
using SylTodo.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SylTodo.UWP.Views {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TodoList : Page {
        public static TodoList Current;

        private TodoItemViewModel viewModel = Database.ViewModel;

        public TodoList() {
            this.InitializeComponent();
            Current = this;
            UpdateListViewEmptyVisibility();
        }

        public void SetHeaderVisibility(string displayMode) {
            VisualStateManager.GoToState(this, displayMode, true);
        }

        public void UpdateListViewEmptyVisibility() {
            if (viewModel.Collection != null && viewModel.Collection.Count != 0) {
                init.Visibility = Visibility.Collapsed;
            } else {
                init.Visibility = Visibility.Visible;
            }
        }

        private async void listView_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            TodoItem item = listView.SelectedItem as TodoItem;
            if (item != null) {
                TodoMain.Current.StateFromListToDetail(item, listView.SelectedIndex);
                TodoMain.Current.BackgroundChange(await Commons.Convert.ConvertByteToImage(item.Bitmap));
            }
        }

        private async void title_KeyDown(object sender, KeyRoutedEventArgs e) {
            if (e.Key == VirtualKey.Enter) {
                if (title.Text == String.Empty) {
                    MessageDialog msg = new MessageDialog("标题不能为空");
                    await msg.ShowAsync();
                    return;
                }
                viewModel.Add(title.Text);
                title.Text = String.Empty;
                UpdateListViewEmptyVisibility();
            }
        }
    }
}
