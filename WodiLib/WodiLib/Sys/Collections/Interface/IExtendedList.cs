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
    ///         <see cref="ISizeChangeableList{TIn, TOut}.GetMinCapacity"/> は 0 固定、
    ///         <see cref="ISizeChangeableList{TIn, TOut}.GetMaxCapacity"/> は <see cref="int.MaxValue"/>
    ///         固定。
    ///         そのため要素を追加/削除する操作によって要素数制限による例外は発生しない。
    ///     </para>
    /// </remarks>
    /// <typeparam name="T">リスト内包クラス</typeparam>
    public interface
        IExtendedList<T> : IRestrictedCapacityList<T>
    {
    }
}
