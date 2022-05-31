using System;
using System.Globalization;
using System.Windows.Data;

namespace NotesWPF.View
{
    public class NoteConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string date = (string)values[0];
            string title = (string)values[1];
            string content = (string)values[2];

            return $"{date}||{title}||{content}";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
