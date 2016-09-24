using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PasswordDatabase
{
    //class EncrypterConverter : IValueConverter
    //{
    //    public static string Password { get; set; }

    //    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        if (value == null)
    //        {
    //            return null;
    //        }
    //        if (string.IsNullOrEmpty(value.ToString()))
    //        {
    //            return null;
    //        }
    //        return Encrypter.Decrypt(Password, value.ToString());
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        if (value == null)
    //        {
    //            return null;
    //        }
    //        if (string.IsNullOrEmpty(value.ToString()))
    //        {
    //            return null;
    //        }
    //        return Encrypter.Encrypt(Password, value.ToString());
    //    }
    //}
}
