﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace TCTUI.Converters
{
   public class Guild_IdToName : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var id = (uint)value;
            return TCTData.Data.GuildDictionary[id];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}