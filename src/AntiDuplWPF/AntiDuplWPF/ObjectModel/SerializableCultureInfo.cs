﻿using System;
using System.Globalization;

namespace AntiDuplWPF.ObjectModel
{
    [Serializable]
    public class SerializableCultureInfo
    {
        public string LanguageName { get; set; }

        /// <summary>
        /// Intended for xml serialization purposes only
        /// </summary>
        private SerializableCultureInfo() { }

        public SerializableCultureInfo(string languageName)
        {
            LanguageName = languageName;
        }

        /* public static SerializableCultureInfo FromRGB(int red, int green, int blue)
         {
             return new SerializableColor(Color.FromArgb(red, green, blue));
         }*/

        public static SerializableCultureInfo FromCultureInfo(CultureInfo c)
        {
            return new SerializableCultureInfo(c.Name);
        }

        public CultureInfo ToCultureInfo()
        {
            return new CultureInfo(LanguageName);
        }
    }
}
