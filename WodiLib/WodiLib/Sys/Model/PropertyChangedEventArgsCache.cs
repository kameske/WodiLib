// ========================================
// Project Name : WodiLib
// File Name    : PropertyChangedEventArgsCache.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using System.ComponentModel;

namespace WodiLib.Sys
{
    /// <summary>
    /// <see cref="PropertyChangedEventArgs"/>のキャッシュクラス
    /// </summary>
    internal static class PropertyChangedEventArgsCache
    {
        private static readonly Dictionary<string, PropertyChangedEventArgs> Cache
            = new Dictionary<string, PropertyChangedEventArgs>();

        public static PropertyChangedEventArgs GetInstance(string propertyName)
        {
            if (!Cache.ContainsKey(propertyName))
            {
                Cache.Add(propertyName, new PropertyChangedEventArgs(propertyName));
            }

            return Cache[propertyName];
        }
    }
}