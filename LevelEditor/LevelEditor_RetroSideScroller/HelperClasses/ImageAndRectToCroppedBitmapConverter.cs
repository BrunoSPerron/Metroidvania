using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ProjetJeuxVideo_LevelEditor_Metroidvania.HelperClasses
{
    public class ImageAndRectToCroppedBitmapConverter : IMultiValueConverter
    {
        public static readonly ImageAndRectToCroppedBitmapConverter Default = new ImageAndRectToCroppedBitmapConverter();

        private static readonly ImageSourceConverter Converter = new ImageSourceConverter();

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] as string != null)
            {
                return new CroppedBitmap((BitmapSource)Converter.ConvertFrom(values[0]), (Int32Rect)values[1]);
            }

            return new CroppedBitmap((BitmapSource)values[0], (Int32Rect)values[1]);
        }

        object[] IMultiValueConverter.ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
