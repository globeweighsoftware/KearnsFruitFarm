using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using Globeweigh.Model;

namespace Globeweigh.UI.Shared.Converters
{
    public sealed class FalseToNavigationStyleNoneConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                if (!(bool) value) return DevExpress.Xpf.Grid.GridViewNavigationStyle.None;
            }

            return DevExpress.Xpf.Grid.GridViewNavigationStyle.Row;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
