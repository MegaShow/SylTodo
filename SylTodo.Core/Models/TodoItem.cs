using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Media.Imaging;

namespace SylTodo.Core.Models {
    class TodoItem {
        public string title;
        public string description;
        public DateTime dueDate;
        public string category;
        public bool isChecked;
        public BitmapImage image;

        public TodoItem(string title, string description, DateTime dueDate, string category, bool isChecked, BitmapImage image) {
            this.title = title;
            this.description = description;
            this.dueDate = dueDate;
            this.category = category;
            this.isChecked = isChecked;
            if (image == null) {
                this.image = new BitmapImage(new Uri("ms-appx:///Assets/Square150x150Logo.scale-200.png"));
            } else {
                this.image = image;
            }
        }
    }
}
