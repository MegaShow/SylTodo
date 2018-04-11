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
        private int selectedIndex = -1;
        public int SelectedIndex {
            get { return selectedIndex; }
        }

        public TodoDetail() {
            this.InitializeComponent();
            Current = this;
        }

        public void StateChange(string newState) {
            VisualStateManager.GoToState(this, newState, true);
        }

        public async void EditInit(TodoItem item, int index) {
            if (selectedIndex != -1) {
                if (title.Text == String.Empty) {
                    MessageDialog msg = new MessageDialog("标题不能为空");
                    await msg.ShowAsync();
                    title.Text = viewModel.Collection[selectedIndex].Title;
                    return;
                }
                viewModel.UpdateAll(selectedIndex, title.Text, description.Text, dueDate.Date.Date);
            }
            selectedIndex = index;
            title.Text = item.Title;
            description.Text = item.Description;
            dueDate.Date = item.DueDate;
        }

        private async void title_LostFocus(object sender, RoutedEventArgs e) {
            if (title.Text == String.Empty) {
                MessageDialog msg = new MessageDialog("标题不能为空");
                await msg.ShowAsync();
                title.Text = viewModel.Collection[selectedIndex].Title;
            } else {
                viewModel.UpdateTitle(selectedIndex, title.Text);
            }
            TileGenerator.Update(Core.Database.ViewModel.Collection);
        }

        private void description_LostFocus(object sender, RoutedEventArgs e) {
            viewModel.UpdateDescription(selectedIndex, description.Text);
            TileGenerator.Update(Core.Database.ViewModel.Collection);
        }

        private void dueDate_LostFocus(object sender, RoutedEventArgs e) {
            viewModel.UpdateDueDate(selectedIndex, dueDate.Date.Date);
            TileGenerator.Update(Core.Database.ViewModel.Collection);
        }

        private void AppBarButton_Click_Delete(object sender, RoutedEventArgs e) {
            viewModel.Remove(selectedIndex);
            selectedIndex = -1;
            StateChange("Init");
            TodoList.Current.UpdateListViewEmptyVisibility();
            TodoMain.Current.BackgroundChange(null);
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
                // Application now has read/write access to the picked file
                try {
                    //BitmapImage bitmapImage = new BitmapImage();
                    //FileRandomAccessStream stream = (FileRandomAccessStream)await file.OpenAsync(FileAccessMode.Read);
                    //bitmapImage.SetSource(stream);
                    //viewModel.UpdateImage(selectedIndex, bitmapImage);
                    //TodoMain.Current.BackgroundChange(bitmapImage);
                    var bitmap = await Commons.Convert.ConvertImageToByte(file);
                    viewModel.UpdateImage(selectedIndex, bitmap);
                    TodoMain.Current.BackgroundChange(await Commons.Convert.ConvertByteToImage(bitmap));
                    TileGenerator.Update(Core.Database.ViewModel.Collection);
                } catch (Exception) {
                    MessageDialog msg = new MessageDialog("发生了些小问题，稍后试试吧", "Oops!");
                    await msg.ShowAsync();
                }
            }
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e) {
            base.OnNavigatedTo(e);
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("TodoMain.State")
                    && ApplicationData.Current.LocalSettings.Values["TodoMain.State"] as string == "OnlyDetailState"
                    && ApplicationData.Current.LocalSettings.Values.ContainsKey("SelectedIndex")) {
                int index = Convert.ToInt32(ApplicationData.Current.LocalSettings.Values["SelectedIndex"]);
                StateChange("Edit");
                EditInit(viewModel.Collection[index], index);
                TodoMain.Current.BackgroundChange(await Commons.Convert.ConvertByteToImage(viewModel.Collection[index].Bitmap));
            }
        }

        private void AppBarButton_Click_Share(object sender, RoutedEventArgs e) {
            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += DataTransferManager_DataRequested;
            DataTransferManager.ShowShareUI();
        }

        private async void DataTransferManager_DataRequested(DataTransferManager sender, DataRequestedEventArgs args) {
            args.Request.Data.Properties.Title = "分享你的清单";
            var streamReference = RandomAccessStreamReference.CreateFromStream(await Commons.Convert.ConvertByteToRandomAccessStream(viewModel.Collection[selectedIndex].Bitmap));
            args.Request.Data.SetBitmap(streamReference);
            args.Request.Data.SetText($"{title.Text}:\n\n{description.Text}");
        }
    }
}
