//
//  CountdownConverter.cs
//  Created by Alexey Kinev on 11 Jan 2015.
//
//    Licensed under The MIT License (MIT)
//    http://opensource.org/licenses/MIT
//
//    Copyright (c) 2015 Alexey Kinev <alexey.rudy@gmail.com>
//
using System;
using Xamarin.Forms;

namespace TriviaTraverse.Converters
{
    /// <summary>
    /// Converts countdown seconds double value to string "HH : MM : SS"
    /// </summary>
    public class CountdownConverter : IValueConverter
    {
        #region IValueConverter implementation

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Rectangle retVal = new Rectangle(1, 1, 1, (double)value / 10);

            return retVal;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
