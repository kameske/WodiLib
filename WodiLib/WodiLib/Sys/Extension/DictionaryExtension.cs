// ========================================
// Project Name : WodiLib
// File Name    : DictionaryExtension.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;

namespace WodiLib.Sys
{
    /// <summary>
    ///     <see cref="IDictionary{TKey,TValue}"/> 拡張クラス
    /// </summary>
    internal static class DictionaryExtension
    {
        /// <summary>
        ///     指定したキーの要素を取得する。要素が存在しない場合、新規インスタンスを作成して自分自身に登録した上で新規作成したインスタンスを返却する。
        /// </summary>
        /// <param name="self">対象</param>
        /// <param name="key">キー</param>
        /// <param name="defaultValueFactory">キーに要素が存在しない場合に新規インスタンスを作成する処理</param>
        /// <typeparam name="TKey">キー型</typeparam>
        /// <typeparam name="TValue">値型</typeparam>
        /// <returns></returns>
        public static TValue GetOrCreate<TKey, TValue>(this IDictionary<TKey, TValue> self, TKey key,
            Func<TValue> defaultValueFactory)
            where TValue : class
        {
            if (self.ContainsKey(key))
            {
                return self[key];
            }

            var newValue = defaultValueFactory.Invoke();
            self[key] = newValue;

            return newValue;
        }
    }
}
