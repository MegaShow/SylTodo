using Newtonsoft.Json;
using SylTodo.Core.Models;
using SylTodo.Core.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace SylTodo.Core {
    public class Database {
        public static TodoItemViewModel ViewModel = new TodoItemViewModel();

        public static string GetCollectionJson() {
            //JsonSerializerSettings microsoftDateFormatSettings = new JsonSerializerSettings {
            //    DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
            //};
            return JsonConvert.SerializeObject(ViewModel.Collection);
        }

        public static void RebuildCollection(string json) {
            try {
                ViewModel.Collection = JsonConvert.DeserializeObject<ObservableCollection<TodoItem>>(json);
            } catch (Exception e) {
                Debug.WriteLine($"{e.ToString()}");
                ViewModel = new TodoItemViewModel();
            }
        }
    }
}
