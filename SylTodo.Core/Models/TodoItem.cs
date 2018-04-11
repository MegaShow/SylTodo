using SylTodo.Core.Commons;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
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

        private DateTimeOffset dueDate;
        public DateTimeOffset DueDate {
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

        private byte[] bitmap;
        public byte[] Bitmap {
            get { return bitmap; }
            set { SetProperty(ref bitmap, value); }
        }

        public TodoItem(string title, string description, DateTimeOffset dueDate, string category, bool isChecked, byte[] image) {
            this.title = title;
            this.description = description;
            this.dueDate = dueDate;
            this.category = category;
            this.isChecked = isChecked;
            this.bitmap = image;
        }
    }
}
