using System;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace WPF_File_Explorer
{
    /// <summary>
    /// convert fullpath to a specific image type {drive, folder(open;close), file}
    /// </summary>
    [ValueConversion(typeof(string), typeof(BitmapImage))]
    public class HeaderToImageConverter : IValueConverter
    {
        public static HeaderToImageConverter Instance = new HeaderToImageConverter();

        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var path = (string)value;

            if(path == null)
                return null;

            var name = MainWindow.GetFileFolderName(path);

            var image = "file.png";

            if (string.IsNullOrEmpty(name))
                image = "drive.png";
            else if(new FileInfo(path).Attributes.HasFlag(FileAttributes.Directory))
                image = "folder-closed.png";

            return new BitmapImage(new Uri($"pack://application:,,,/Images/{image}"));
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
