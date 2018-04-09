using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using Windows.UI.Xaml.Media.Imaging;

namespace SylTodo.Core.Models {
    public class TodoItem : INotifyPropertyChanged {
        private string title;
        public string Title {
            get { return title; }
            set {
                if (title != value) {
                    title = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string description;
        public string Description {
            get { return description; }
            set {
                if (description != value) {
                    description = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private DateTime dueDate;
        public DateTime DueDate {
            get { return dueDate; }
            set {
                if (dueDate != value.Date) {
                    dueDate = value.Date;
                    NotifyPropertyChanged();
                }
            }
        }

        private string category;
        public string Category {
            get { return category; }
            set {
                if (category != value) {
                    category = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private bool isChecked;
        public bool IsChecked {
            get { return isChecked; }
            set {
                if (isChecked != value) {
                    isChecked = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private BitmapImage image;
        public BitmapImage Image {
            get { return image; }
            set {
                if (image != value) {
                    image = value;
                    NotifyPropertyChanged();
                }
            }
        }

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

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "") {
            Debug.WriteLine($"{propertyName} - Change");
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
