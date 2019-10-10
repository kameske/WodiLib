// ========================================
// Project Name : WodiLib
// File Name    : KeyValuePairExtension.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;

namespace WodiLib.Sys
{
    /// <summary>
    /// KeyValuePair拡張メソッド
    /// </summary>
    internal static class KeyValuePairExtension
    {
        /// <summary>
        /// nullかどうかを返す。
        /// </summary>
        /// <param name="src">対象</param>
        /// <typeparam name="TKey">Key</typeparam>
        /// <typeparam name="TValue">Value</typeparam>
        /// <returns>nullの場合true</returns>
        public static bool IsNull<TKey, TValue>(this KeyValuePair<TKey, TValue> src)
        {
            return src.Equals(default(KeyValuePair<TKey, TValue>));
        }
    }
}