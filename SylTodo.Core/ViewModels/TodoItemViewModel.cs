using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SylTodo.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;

namespace SylTodo.Core.ViewModels {
    public class TodoItemViewModel {
        private ObservableCollection<TodoItem> collection;
        public ObservableCollection<TodoItem> Collection {
            get { return collection; }
        }

        private int type = 0; // 0 => 所有, 1 => 今天, 2 => 最近7天, 3 => 收集箱, 4 => 已完成, 5 => 垃圾箱, 

        public TodoItemViewModel() {
            using (var db = new TodoContext()) {
                db.Database.Migrate();
            }
            SyncContext();
        }

        public void SyncContext() {
            using (var db = new TodoContext()) {
                List<TodoItem> list = db.Items.ToList();
                // list.Sort((a, b) => a.DueDate == b.DueDate ? 0 : (a.DueDate > b.DueDate ? 1 : -1));
                collection = new ObservableCollection<TodoItem>(list);
            }
        }

        public void SetFilter(int type, List<TodoItem> list = null) {
            if (this.type == type) {
                return;
            }
            this.type = type;
            switch (type) {
                case 0: // 所有
                    foreach (TodoItem item in collection) {
                        item.Filter = Visibility.Visible;
                    }
                    break;
                case 1: // 今天
                    foreach (TodoItem item in collection) {
                        if (item.DueDate == DateTime.Now.Date) {
                            item.Filter = Visibility.Visible;
                        } else {
                            item.Filter = Visibility.Collapsed;
                        }
                    }
                    break;
                case 2: // 最近7天
                    foreach (TodoItem item in collection) {
                        if (item.DueDate >= DateTime.Now.Date && item.DueDate - DateTime.Now.Date <= new TimeSpan(7, 0, 0, 0)) {
                            item.Filter = Visibility.Visible;
                        } else {
                            item.Filter = Visibility.Collapsed;
                        }
                    }
                    break;
                case 5: // 已完成
                    foreach (TodoItem item in collection) {
                        if (item.IsChecked) {
                            item.Filter = Visibility.Visible;
                        } else {
                            item.Filter = Visibility.Collapsed;
                        }
                    }
                    break;
                case 6: // 搜索
                    foreach (TodoItem item in collection) {
                        if (list.Contains(item)) {
                            item.Filter = Visibility.Visible;
                        } else {
                            item.Filter = Visibility.Collapsed;
                        }
                    }
                    break;
            }
        }

        public TodoItem GetItemById(int id) {
            return collection.Where(i => i.Id == id).FirstOrDefault();
        }

        public List<string> GetTitles() {
            return collection.Select(i => i.Title).ToList();
        }

        public List<TodoItem> GetListByTitle(string title) {
            return collection.Where(i => i.Title == title).ToList();
        }

        public void Add(string title) {
            AddDefault(title, String.Empty, DateTime.Now, "收集箱", false, null);
        }

        public void Add(string title, string description, DateTimeOffset dueDate, string category = "收集箱", bool isChecked = false, byte[] bitmap = null) {
            AddDefault(title, description, dueDate, category, isChecked, bitmap);
        }

        private void AddDefault(string title, string description, DateTimeOffset dueDate, string category, bool isChecked, byte[] bitmap) {
            using (var db = new TodoContext()) {
                var item = new TodoItem(title, description, dueDate, category, isChecked, bitmap);
                db.Items.Add(item);
                collection.Add(item);
                db.SaveChanges();
            }
        }

        public void Remove(TodoItem item) {
            RemoveDefault(item);
        }

        private void RemoveDefault(TodoItem item) {
            using (var db = new TodoContext()) {
                db.Items.Remove(item);
                collection.Remove(item);
                db.SaveChanges();
            }
        }

        public void UpdateAll(TodoItem item, string title, string description, DateTimeOffset dueDate) {
            UpdateDefault(item.Id, title, description, dueDate, item.Category, item.IsChecked, item.Bitmap);
        }

        public void UpdateTitle(TodoItem item, string title) {
            UpdateDefault(item.Id, title, item.Description, item.DueDate, item.Category, item.IsChecked, item.Bitmap);
        }

        public void UpdateDescription(TodoItem item, string description) {
            UpdateDefault(item.Id, item.Title, description, item.DueDate, item.Category, item.IsChecked, item.Bitmap);
        }

        public void UpdateDueDate(TodoItem item, DateTime dueDate) {
            UpdateDefault(item.Id, item.Title, item.Description, dueDate.Date, item.Category, item.IsChecked, item.Bitmap);
        }

        public void UpdateBitmap(TodoItem item, byte[] bitmap) {
            UpdateDefault(item.Id, item.Title, item.Description, item.DueDate, item.Category, item.IsChecked, bitmap);
        }

        private void UpdateDefault(int id, string title, string description, DateTimeOffset dueDate, string category, bool isChecked, byte[] bitmap) {
            using (var db = new TodoContext()) {
                TodoItem item = db.Items.Find(id);
                if (item != null) {
                    item.Title = title;
                    item.Description = description;
                    item.DueDate = dueDate;
                    item.Category = category;
                    item.IsChecked = isChecked;
                    item.Bitmap = bitmap;
                }
                TodoItem itemInColl = collection.Where(i => i.Id == id).FirstOrDefault();
                if (itemInColl != null) {
                    itemInColl.Title = title;
                    itemInColl.Description = description;
                    itemInColl.DueDate = dueDate;
                    itemInColl.Category = category;
                    itemInColl.IsChecked = isChecked;
                    itemInColl.Bitmap = bitmap;
                }
                db.SaveChanges();
            }
        }
    }
}
