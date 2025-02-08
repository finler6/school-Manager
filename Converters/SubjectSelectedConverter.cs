using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using Microsoft.Maui.Controls;

namespace SchoolManage.App.Converters
{
    public class SubjectSelectedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var selectedSubjectIds = value as ObservableCollection<Guid>;
            var subjectId = (Guid)parameter;

            return selectedSubjectIds != null && selectedSubjectIds.Contains(subjectId);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
