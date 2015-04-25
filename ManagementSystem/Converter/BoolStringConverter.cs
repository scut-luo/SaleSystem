using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Globalization;
using System.Windows.Data;

namespace ManagementSystem
{
    //将布尔值与字符串互转
    // 0 = "否"
    // 1 = "是"
    [System.Windows.Data.ValueConversion(typeof(bool), typeof(string))]
    public class BoolStringConverter : IValueConverter
    {
        //由 bool -> string
        public object Convert(
            object value, Type targetType, object parameter, CultureInfo culture)
        {
            string str = "NULL";

            if ((bool)value)
                str = "是";
            else
                str = "否";            
            return str;
        }

        //由 string -> bool
        public object ConvertBack(
            object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool bvalue = false;

            switch ((string)value)
            {
                case "否":
                    bvalue = false;
                    break;

                case "是":
                    bvalue = true;
                    break;
            }
            return bvalue;
        }
    }
}
