using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace SylTodo.UWP.Converter {
    public class BooleanToColorsConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            bool flag = false;
            if (value is bool) {
                flag = (bool)value;
            } else if (value is bool?) {
                bool? nullable = (bool?)value;
                flag = nullable.GetValueOrDefault();
            }
            if (parameter != null) {
                if (bool.Parse((string)parameter)) {
                    flag = !flag;
                }
            }
            if (flag) {
                return new SolidColorBrush(Colors.Gray);
            } else {
                return new SolidColorBrush(Colors.Black);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotImplementedException();
        }
    }
}
