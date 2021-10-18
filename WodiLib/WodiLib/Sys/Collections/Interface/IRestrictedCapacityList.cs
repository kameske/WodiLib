// ========================================
// Project Name : WodiLib
// File Name    : IRestrictedCapacityList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.ObjectModel;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     容量制限のあるListインタフェース
    /// </summary>
    /// <remarks>
    ///     <see cref="ObservableCollection{T}"/> をベースに、容量制限を設けた機能。
    ///     <see cref="ObservableCollection{T}"/> のCRUD各種処理に範囲指定バージョン（XXXRange メソッド）を追加している。
    ///     それ以外にもいくつかメソッドを追加している。
    /// </remarks>
    /// <typeparam name="TIn">リスト要素入力型</typeparam>
    /// <typeparam name="TOut">リスト要素出力型</typeparam>
    public interface IRestrictedCapacityList<TIn, TOut> :
        IFixedLengthList<TIn, TOut>,
        ISizeChangeableList<TIn, TOut>
        where TOut : TIn
    {
    }

    /// <summary>
    ///     容量制限のあるListインタフェース
    /// </summary>
    /// <remarks>
    ///     <see cref="ObservableCollection{T}"/> をベースに、容量制限を設けた機能。
    ///     <see cref="ObservableCollection{T}"/> のCRUD各種処理に範囲指定バージョン（XXXRange メソッド）を追加している。
    ///     それ以外にもいくつかメソッドを追加している。
    /// </remarks>
    /// <typeparam name="T">リスト内包クラス</typeparam>
    [Obsolete]
    public interface IRestrictedCapacityList<T> : ISizeChangeableList<T, IRestrictedCapacityList<T>, IFixedLengthList<T>
        , IReadOnlyExtendedList<T>>
    {
    }
}
