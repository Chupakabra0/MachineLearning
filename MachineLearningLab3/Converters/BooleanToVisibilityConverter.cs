using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MachineLearningLab3.Converters.BooleanToVisibilityConverter
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            (bool)value ? Visibility.Visible : Visibility.Hidden;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            (Visibility)value == Visibility.Visible;
    }
}