using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ProjetJeuxVideo_LevelEditor_Metroidvania.HelperClasses
{
    public class BoolToFloatHoverValueConverter : IValueConverter
    {
        public static readonly BoolToFloatHoverValueConverter Default = new BoolToFloatHoverValueConverter();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
                return 1f;
            else return 0.8;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((float)value == 1f)
                return true;
            else return false;
        }
    }
}
