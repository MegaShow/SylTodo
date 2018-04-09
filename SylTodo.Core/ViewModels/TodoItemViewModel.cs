using SylTodo.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Windows.UI.Xaml.Media.Imaging;

namespace SylTodo.Core.ViewModels {
    public class TodoItemViewModel {
        private ObservableCollection<TodoItem> collection = new ObservableCollection<TodoItem>();
        public ObservableCollection<TodoItem> Collection {
            get { return collection; }
        }

        public TodoItemViewModel() {
            this.Add("欢迎加入希娃清单", "从今天起，希娃清单将伴你开启时间管理之旅。", DateTime.Now.Date);
            this.Add("你好，世界", "Never underestimate the ability of a small group of dedicated people to " +
                "change the world.Indeed, it is the only thing that ever has.", DateTime.Now.Date);
        }

        public void Add(string title, string description, DateTime dueDate, string category = "收集箱", bool isChecked = false, BitmapImage image = null) {
            collection.Add(new TodoItem(title, description, dueDate, category, isChecked, image));
        }

        public void Remove() {

        }

        public void Update(int itemIndex, string title, string description, DateTime dueDate, string category, BitmapImage image) {

        }

        public void UpdateTitle(int selectedIndex, string title) {
            collection[selectedIndex].Title = title;
        }

        public void UpdateDescription(int selectedIndex, string description) {
            collection[selectedIndex].Description = description;
        }
    }
}
