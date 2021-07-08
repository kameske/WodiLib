// ========================================
// Project Name : WodiLib
// File Name    : IRestrictedCapacityList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
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
    /// <typeparam name="T">リスト内包クラス</typeparam>
    public interface IRestrictedCapacityList<T> : IFixedLengthList<T>,
        ISizeChangeableList<T, IRestrictedCapacityList<T>, IFixedLengthList<T>, IReadOnlyExtendedList<T>>,
        IDeepCloneableList<IRestrictedCapacityList<T>, T>
    {
        /// <inheritdoc cref="IDeepCloneableList{T,TIn}.DeepClone"/>
        public new IRestrictedCapacityList<T> DeepClone();

        /// <inheritdoc cref="IDeepCloneableList{T,TIn}.DeepCloneWith"/>
        public new IRestrictedCapacityList<T> DeepCloneWith(int? length = null,
            IReadOnlyDictionary<int, T>? values = null);
    }

    /// <summary>
    ///     【読み取り専用】容量制限のあるListインタフェース
    /// </summary>
    /// <typeparam name="T">要素の型</typeparam>
    [Obsolete]
    public interface IReadOnlyRestrictedCapacityList<T> : IReadOnlyExtendedList<T>
    {
        /// <summary>
        ///     容量最大値を返す。
        /// </summary>
        /// <returns>容量最大値</returns>
        int GetMaxCapacity();

        /// <summary>
        ///     容量最小値を返す。
        /// </summary>
        /// <returns>容量最小値</returns>
        int GetMinCapacity();
    }
}
