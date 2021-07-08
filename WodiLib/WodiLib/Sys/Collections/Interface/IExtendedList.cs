// ========================================
// Project Name : WodiLib
// File Name    : IExtendedList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     WodiLib 独自リストインタフェース
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         <see cref="IList{T}"/> のメソッドと <see cref="ObservableCollection{T}"/> の機能を融合した機能。
    ///         <see cref="ObservableCollection{T}"/> のCRUD各種処理に範囲指定バージョン（XXXRange メソッド）を追加している。
    ///         それ以外にもいくつかメソッドを追加している。
    ///     </para>
    ///     <para>
    ///         <see cref="ISizeChangeableList{TItem,TImpl,TWritable,TReadable}.GetMinCapacity"/> は 0 固定、
    ///         <see cref="ISizeChangeableList{TItem,TImpl,TWritable,TReadable}.GetMaxCapacity"/> は <see cref="int.MaxValue"/>
    ///         固定。
    ///         そのため要素を追加/削除する操作によって要素数制限による例外は発生しない。
    ///     </para>
    /// </remarks>
    /// <typeparam name="T">リスト内包クラス</typeparam>
    public interface
        IExtendedList<T> : IFixedLengthList<T>,
            ISizeChangeableList<T, IExtendedList<T>, IFixedLengthList<T>, IReadOnlyExtendedList<T>>,
            IDeepCloneableList<IExtendedList<T>, T>
    {
        /// <inheritdoc cref="IDeepCloneableList{T,TIn}.DeepClone"/>
        public new IExtendedList<T> DeepClone();

        /// <inheritdoc cref="IDeepCloneableList{T,TIn}.DeepCloneWith"/>
        public new IExtendedList<T> DeepCloneWith(int? length = null, IReadOnlyDictionary<int, T>? values = null);
    }

    /// <summary>
    ///     WodiLib 独自の読み取り専用リストインタフェース
    /// </summary>
    /// <typeparam name="T">リスト内包クラス</typeparam>
    public interface IReadOnlyExtendedList<T> : IReadableList<T, IReadOnlyExtendedList<T>>,
        IDeepCloneableList<IReadOnlyExtendedList<T>, T>
    {
    }
}
