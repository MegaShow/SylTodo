using SylTodo.Core.Commons;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using Windows.UI.Xaml.Media.Imaging;

namespace SylTodo.Core.Models {
    public class TodoItem : BindableBase {
        private string title;
        public string Title {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        private string description;
        public string Description {
            get { return description; }
            set { SetProperty(ref description, value); }
        }

        private DateTime dueDate;
        public DateTime DueDate {
            get { return dueDate; }
            set { SetProperty(ref dueDate, value); }
        }

        private string category;
        public string Category {
            get { return category; }
            set { SetProperty(ref category, value); }
        }

        private bool isChecked;
        public bool IsChecked {
            get { return isChecked; }
            set { SetProperty(ref isChecked, value); }
        }

        private BitmapImage image;
        public BitmapImage Image {
            get { return image; }
            set { SetProperty(ref image, value); }
        }

        public TodoItem(string title, string description, DateTime dueDate, string category, bool isChecked, BitmapImage image) {
            this.title = title;
            this.description = description;
            this.dueDate = dueDate;
            this.category = category;
            this.isChecked = isChecked;
            if (image == null) {
                this.image = new BitmapImage(new Uri("ms-appx:///Assets/background.png"));
            } else {
                this.image = image;
            }
        }
    }
}
