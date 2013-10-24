using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;
using System.Globalization;

namespace TimeTrackingApp
{
    public class WorkItemFieldsNameToString : IValueConverter
    {       
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Dictionary<string, object> fields = value as Dictionary<string, object>;
            return fields["Name"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string strValue = value as string;

            return strValue;
        }
    }
}
