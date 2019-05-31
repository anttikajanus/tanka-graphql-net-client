using System;
using System.Globalization;
using System.Windows.Data;

namespace Tanka.GraphQL.Sample.Chat.Client.Wpf.Converters
{
    public class ProfilePictureConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var profilePicture = (string)value;
            if (!string.IsNullOrEmpty(profilePicture))
                return new Uri(profilePicture);

            return new Uri("pack://application:,,,/Resources/account_circle_grey_192x192.png");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
