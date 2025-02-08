using System;
using System.Globalization;
using System.Linq;
using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;

namespace SchoolManage.App.Converters
{
    public class ContainsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ObservableCollection<Guid> collection && parameter is Guid id)
            {
                return collection.Contains(id);
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}

