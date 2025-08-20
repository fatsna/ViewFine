using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace ViewFine.Converters
{
    public class NullToCollapsed : IValueConverter
    {
        public object Convert(object value, Type t, object p, CultureInfo c)
            => value == null || (value is string s && string.IsNullOrWhiteSpace(s))
               ? Visibility.Collapsed : Visibility.Visible;
        public object ConvertBack(object v, Type t, object p, CultureInfo c) => throw new NotImplementedException();
    }

}
