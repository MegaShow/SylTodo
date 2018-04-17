using SylTodo.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.Storage;
using Windows.UI.Notifications;

namespace SylTodo.UWP.Tile {
    class TileGenerator {
        public static async void Update(ObservableCollection<TodoItem> collection) {
            try {
                var updater = TileUpdateManager.CreateTileUpdaterForApplication();
                updater.EnableNotificationQueueForWide310x150(true);
                updater.EnableNotificationQueueForSquare150x150(true);
                updater.EnableNotificationQueueForSquare310x310(true);
                updater.EnableNotificationQueue(true);
                updater.Clear();

                for (int i = 0, count = 0; count < 5 && i < collection.Count; i++) {
                    if (collection[i].IsChecked) {
                        continue;
                    }
                    string path;
                    if (collection[i].Bitmap != null) {
                        StorageFolder folder = await ApplicationData.Current.LocalFolder.CreateFolderAsync("SylTodoFolder", CreationCollisionOption.OpenIfExists);
                        StorageFile file = await folder.CreateFileAsync($"{i}.jpg", CreationCollisionOption.ReplaceExisting);
                        await FileIO.WriteBytesAsync(file, collection[i].Bitmap);
                        path = file.Path;
                    } else {
                        path = "ms-appx:///Assets/LargeTile.scale-400.png";
                    }
                    XmlDocument tileXml = new XmlDocument();
                    string xmlString = await File.ReadAllTextAsync("Tile/Tile.xml");
                    tileXml.LoadXml(string.Format(xmlString, collection[i].Title, collection[i].Description, collection[i].DueDate.ToString("yyyy-MM-dd"), path));
                    updater.Update(new TileNotification(tileXml));
                    count++;
                }
            } catch (Exception e) {
                Debug.WriteLine($"{e.ToString()}");
            }
        }
    }
}
