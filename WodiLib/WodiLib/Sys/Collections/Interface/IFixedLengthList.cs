// ========================================
// Project Name : WodiLib
// File Name    : IFixedLengthList.cs
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
    ///     長さが固定されたListインタフェース
    /// </summary>
    /// <remarks>
    ///     <see cref="ObservableCollection{T}"/> をベースに、容量を固定した機能。
    ///     <see cref="ObservableCollection{T}"/> の Read, Update 各種処理に範囲指定バージョン（XXXRange メソッド）を追加している。
    ///     それ以外にもいくつかメソッドを追加している。
    ///     固定しているのは容量のみで、要素の入れ替えや更新は可能。
    /// </remarks>
    /// <typeparam name="T">リスト内包クラス</typeparam>
    public interface IFixedLengthList<T> : IReadOnlyExtendedList<T>,
        IWritableList<T, IFixedLengthList<T>, IReadOnlyExtendedList<T>>,
        IDeepCloneableList<IFixedLengthList<T>, T>
    {
        /// <inheritdoc cref="IDeepCloneableList{T,TIn}.DeepClone"/>
        public new IFixedLengthList<T> DeepClone();

        /// <inheritdoc cref="IDeepCloneableList{T,TIn}.DeepCloneWith"/>
        public new IFixedLengthList<T> DeepCloneWith(int? length = null, IReadOnlyDictionary<int, T>? values = null);
    }

    /// <summary>
    ///     【読み取り専用】長さが固定されたListインタフェース
    /// </summary>
    /// <typeparam name="T">リスト内包クラス</typeparam>
    [Obsolete]
    public interface IReadOnlyFixedLengthList<T> : IReadOnlyExtendedList<T>
    {
        /// <summary>
        ///     容量を返す。
        /// </summary>
        /// <returns>容量</returns>
        public int GetCapacity();
    }
}
