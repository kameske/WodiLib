// ========================================
// Project Name : WodiLib
// File Name    : IFixedLengthList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     長さが固定されたListインタフェース
    /// </summary>
    /// <remarks>
    ///     <see cref="ObservableCollection{T}"/> をベースに、容量を固定した機能。
    ///     <see cref="ObservableCollection{T}"/> の Read, Update 各種処理に範囲指定バージョン（XXXRange メソッド）を追加している。
    ///     それ以外にもいくつかメソッドを追加している。
    ///     固定しているのは容量のみで、要素の入れ替えや更新は可能。
    /// </remarks>
    /// <typeparam name="TIn">リスト要素入力型</typeparam>
    /// <typeparam name="TOut">リスト要素出力型</typeparam>
    public interface IFixedLengthList<TIn, TOut> :
        IReadOnlyExtendedList<TIn, TOut>,
        IWritableList<TIn, TOut>
        where TOut : TIn
    {
        /// <inheritdoc cref="IWritableList{TIn, TOut}.this[int]"/>
        public new TIn this[int index] { get; set; }
    }

    /// <summary>
    ///     長さが固定されたListインタフェース
    /// </summary>
    /// <remarks>
    ///     <see cref="ObservableCollection{T}"/> をベースに、容量を固定した機能。
    ///     <see cref="ObservableCollection{T}"/> の Read, Update 各種処理に範囲指定バージョン（XXXRange メソッド）を追加している。
    ///     それ以外にもいくつかメソッドを追加している。
    ///     固定しているのは容量のみで、要素の入れ替えや更新は可能。
    /// </remarks>
    /// <typeparam name="T">リスト要素入力型</typeparam>
    public interface IFixedLengthList<T> : IFixedLengthList<T, T>,
        IWritableList<T>,
        IReadOnlyExtendedList<T>
    {
    }

    /// <summary>
    /// <see cref="IFixedLengthList{TIn,TOut}"/> および
    /// <see cref="IFixedLengthList{T}"/> の拡張クラス
    /// </summary>
    public static class FixedLengthListExtension
    {
        /// <inheritdoc cref="SetValues{TIn, TOut}" select="summary|param"/>
        /// <typeparam name="T">リスト要素型</typeparam>
        public static void SetValues<T>(this IFixedLengthList<T> src, IEnumerable<KeyValuePair<int, T>> values)
            => SetValues<T, T>(src, values);

        /// <summary>
        /// インデックスを指定して複数の要素を更新する。
        /// </summary>
        /// <param name="src">処理対象</param>
        /// <param name="values">更新要素情報</param>
        /// <typeparam name="TIn">リスト入力型</typeparam>
        /// <typeparam name="TOut">リスト出力型</typeparam>
        public static void SetValues<TIn, TOut>(this IFixedLengthList<TIn, TOut> src,
            IEnumerable<KeyValuePair<int, TIn>> values)
            where TOut : TIn
        {
            ThrowHelper.ValidateArgumentNotNull(src is null, nameof(src));
            ThrowHelper.ValidateArgumentNotNull(values is null, nameof(values));
            var valueArray = values.ToArray();
            ThrowHelper.ValidateArgumentItemsHasNotNull(valueArray.Select(pair => pair.Value).HasNullItem(),
                nameof(values));

            valueArray.ForEach(pair =>
            {
                if (pair.Key >= src.Count)
                {
                    return;
                }

                src[pair.Key] = pair.Value;
            });
        }
    }
}
