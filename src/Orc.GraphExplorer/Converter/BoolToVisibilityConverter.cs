using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace Orc.GraphExplorer.Converter
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        static BoolToVisibilityConverter _instance;
        public static BoolToVisibilityConverter Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new BoolToVisibilityConverter();
                return _instance;
            }
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool isRevered = parameter != null ? parameter.ToString() == "R" : false;

            Visibility retVal = Visibility.Collapsed;
            bool locVal = (bool)value;

            if (isRevered)
                locVal = !locVal;

            if (locVal)
                retVal = Visibility.Visible;
            else
                retVal = Visibility.Collapsed;

            return retVal;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool isRevered = parameter != null ? parameter.ToString() == "R" : false;

            Visibility locVal = (Visibility)value;

            bool retVal = false;

            if (locVal == Visibility.Visible)
            {
                retVal = true;

                if (isRevered)
                    retVal = !retVal;
            }
            return retVal;
        }
    }
}
