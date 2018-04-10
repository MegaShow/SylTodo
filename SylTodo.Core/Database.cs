using Newtonsoft.Json;
using SylTodo.Core.Models;
using SylTodo.Core.ViewModels;
using System;
using System.Collections.ObjectModel;

namespace SylTodo.Core {
    public class Database {
        public static TodoItemViewModel ViewModel = new TodoItemViewModel();

        public static string GetCollectionJson() {
            return JsonConvert.SerializeObject(ViewModel.Collection);
        }

        public static void RebuildCollection(string json) {
            ViewModel.Collection = JsonConvert.DeserializeObject<ObservableCollection<TodoItem>>(json);
        }
    }
}
