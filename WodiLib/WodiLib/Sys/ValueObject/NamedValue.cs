// ========================================
// Project Name : WodiLib
// File Name    : NamedValue.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace WodiLib.Sys
{
    /// <summary>
    /// 名前をつけた値
    /// </summary>
    /// <typeparam name="T">値型</typeparam>
    [CommonMultiValueObject]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public partial record NamedValue<T>
    {
        /// <summary>名前</summary>
        public string Name { get; }

        /// <summary>値</summary>
        public T Value { get; }

        /// <summary>
        /// 自身を <see cref="TOther"/> 型の名前付き値オブジェクトに変換する。
        /// </summary>
        /// <typeparam name="TOther">変換後の値型</typeparam>
        /// <returns></returns>
        /// <exception cref="InvalidCastException">キャスト不可能な場合。</exception>
        public NamedValue<TOther> Cast<TOther>()
        {
            if (Value is TOther casted)
            {
                return new NamedValue<TOther>(Name, casted);
            }

            throw new InvalidCastException(ErrorMessage.NotCast($"{typeof(T)} は {typeof(TOther)} を実装していないため"));
        }

        /// <summary>
        /// (string, <typeparamref name="T"/>) タプルからの暗黙的型変換
        /// </summary>
        /// <param name="tuple">変換元</param>
        /// <returns>変換結果</returns>
        public static implicit operator NamedValue<T>((string name, T value) tuple)
        {
            var (name, value) = tuple;
            return new NamedValue<T>(name, value);
        }
    };
}
