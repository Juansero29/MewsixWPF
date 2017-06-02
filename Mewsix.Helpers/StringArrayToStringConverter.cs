using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Mewsix.Helpers
{
    public class StringArrayToStringConverter : IValueConverter
    {



        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string[] artists = value as string[];
            if (artists == null) return null;

            return string.Join(", ", artists);
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string artists = value as string;
            if (value == null) return null;

            return artists.Split(',');
        }
    }
}
