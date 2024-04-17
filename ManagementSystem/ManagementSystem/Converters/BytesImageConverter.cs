using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml.Converters;
using Avalonia.Media.Imaging;
using ManagementSystem.Assets;

namespace ManagementSystem.Converters;

public class BytesImageConverter : IValueConverter
{

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not byte[] imageBytes)
            return StaticResources.NoImagePictureImage;
        Bitmap? image = null;
        var ms = new MemoryStream(imageBytes); 
        image = new Bitmap(ms);
        ms.Dispose();
        return image;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}