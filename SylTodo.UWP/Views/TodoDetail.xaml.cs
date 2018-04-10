using SylTodo.Core;
using SylTodo.Core.Models;
using SylTodo.Core.ViewModels;
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
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SylTodo.UWP.Views {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TodoDetail : Page {
        public static TodoDetail Current;
        private TodoItemViewModel viewModel = Database.ViewModel;
        private int selectedIndex = -1;

        public TodoDetail() {
            this.InitializeComponent();
            Current = this;
        }

        public void StateChange(string newState) {
            VisualStateManager.GoToState(this, newState, true);
        }

        public void EditInit(TodoItem item, int index) {
            if (selectedIndex != -1) {
                viewModel.UpdateAll(selectedIndex, title.Text, description.Text, dueDate.Date.Date);
            }
            selectedIndex = index;
            title.Text = item.Title;
            description.Text = item.Description;
            dueDate.Date = item.DueDate;
        }

        private void title_LostFocus(object sender, RoutedEventArgs e) {
            viewModel.UpdateTitle(selectedIndex, title.Text);
        }

        private void description_LostFocus(object sender, RoutedEventArgs e) {
            viewModel.UpdateDescription(selectedIndex, description.Text);
        }

        private void dueDate_LostFocus(object sender, RoutedEventArgs e) {
            viewModel.UpdateDueDate(selectedIndex, dueDate.Date.Date);
        }
    }
}
