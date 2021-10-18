// ========================================
// Project Name : WodiLib
// File Name    : IFixedLengthList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.ObjectModel;

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
    /// <typeparam name="T">リスト内包クラス</typeparam>
    [Obsolete]
    public interface IFixedLengthList<T> : IWritableList<T, IFixedLengthList<T>, IReadOnlyExtendedList<T>>
    {
    }
}
