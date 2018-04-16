using SylTodo.Core;
using SylTodo.Core.Models;
using SylTodo.Core.ViewModels;
using SylTodo.UWP.Tile;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SylTodo.UWP.Views {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TodoDetail : Page {
        public static TodoDetail Current;
        private TodoItemViewModel viewModel = Database.ViewModel;
        private TodoItem selectedItem = null;

        public TodoDetail() {
            this.InitializeComponent();
            Current = this;
        }

        public void StateChange(string newState) {
            VisualStateManager.GoToState(this, newState, true);
        }

        public async void EditInit(TodoItem item, int index) {
            if (selectedItem != null) {
                if (title.Text == String.Empty) {
                    MessageDialog msg = new MessageDialog("标题不能为空");
                    await msg.ShowAsync();
                    title.Text = selectedItem.Title;
                    return;
                }
                viewModel.UpdateAll(selectedItem, title.Text, description.Text, dueDate.Date.Date);
            }
            selectedItem = item;
            title.Text = item.Title;
            description.Text = item.Description;
            dueDate.Date = item.DueDate;
        }

        private async void title_LostFocus(object sender, RoutedEventArgs e) {
            if (title.Text == String.Empty) {
                MessageDialog msg = new MessageDialog("标题不能为空");
                await msg.ShowAsync();
                title.Text = selectedItem.Title;
            } else {
                viewModel.UpdateTitle(selectedItem, title.Text);
            }
            TileGenerator.Update(Database.ViewModel.Collection);
        }

        private void description_LostFocus(object sender, RoutedEventArgs e) {
            viewModel.UpdateDescription(selectedItem, description.Text);
            TileGenerator.Update(Database.ViewModel.Collection);
        }

        private void dueDate_LostFocus(object sender, RoutedEventArgs e) {
            viewModel.UpdateDueDate(selectedItem, dueDate.Date.Date);
            TileGenerator.Update(Database.ViewModel.Collection);
        }

        private void AppBarButton_Click_Delete(object sender, RoutedEventArgs e) {
            viewModel.Remove(selectedItem);
            selectedItem = null;
            StateChange("Init");
            TodoList.Current.UpdateListViewEmptyVisibility();
            TodoMain.Current.BackgroundChange(null);
            TileGenerator.Update(Database.ViewModel.Collection);
        }

        private async void AppBarButton_Click_UploadAsync(object sender, RoutedEventArgs e) {
            FileOpenPicker picker = new FileOpenPicker();
            picker.ViewMode = PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");

            StorageFile file = await picker.PickSingleFileAsync();
            if (file != null) {
                try {
                    var bitmap = await Commons.Convert.ConvertImageToByte(file);
                    viewModel.UpdateBitmap(selectedItem, bitmap);
                    TodoMain.Current.BackgroundChange(await Commons.Convert.ConvertByteToImage(bitmap));
                    TileGenerator.Update(Database.ViewModel.Collection);
                } catch (Exception) {
                    MessageDialog msg = new MessageDialog("发生了些小问题，稍后试试吧", "Oops!");
                    await msg.ShowAsync();
                }
            }
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e) {
            //base.OnNavigatedTo(e);
            //if (ApplicationData.Current.LocalSettings.Values.ContainsKey("TodoMain.State")
            //        && ApplicationData.Current.LocalSettings.Values["TodoMain.State"] as string == "OnlyDetailState"
            //        && ApplicationData.Current.LocalSettings.Values.ContainsKey("SelectedIndex")) {
            //    int index = Convert.ToInt32(ApplicationData.Current.LocalSettings.Values["SelectedIndex"]);
            //    StateChange("Edit");
            //    EditInit(viewModel.Collection[index]);
            //    TodoMain.Current.BackgroundChange(await Commons.Convert.ConvertByteToImage(viewModel.Collection[index].Bitmap));
            //}
        }

        private void AppBarButton_Click_Share(object sender, RoutedEventArgs e) {
            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += DataTransferManager_DataRequested;
            DataTransferManager.ShowShareUI();
        }

        private async void DataTransferManager_DataRequested(DataTransferManager sender, DataRequestedEventArgs args) {
            args.Request.Data.Properties.Title = "分享你的清单";
            var streamReference = RandomAccessStreamReference.CreateFromStream(await Commons.Convert.ConvertByteToRandomAccessStream(selectedItem.Bitmap));
            args.Request.Data.SetBitmap(streamReference);
            args.Request.Data.SetText($"{title.Text}:\n\n{description.Text}");
        }
    }
}
