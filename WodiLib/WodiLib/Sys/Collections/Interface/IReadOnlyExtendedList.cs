// ========================================
// Project Name : WodiLib
// File Name    : IReadOnlyExtendedList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     WodiLib 独自読み取り専用リストインタフェース
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         <see cref="IReadOnlyList{T}"/> のメソッドと <see cref="ObservableCollection{T}"/> の機能を融合した機能。
    ///         <see cref="ObservableCollection{T}"/> のCRUD各種処理に範囲指定バージョン（XXXRange メソッド）を追加している。
    ///         それ以外にもいくつかメソッドを追加している。
    ///     </para>
    ///     <para>
    ///         <see cref="ObservableCollection{T}"/> とは異なり、
    ///         <typeparamref name="TOut"/> が変更通知を行うクラスだった場合、
    ///         通知を受け取ると自身の "Items[]" プロパティ変更通知を行う。
    ///     </para>
    /// </remarks>
    /// <typeparam name="TIn">リスト要素入力型</typeparam>
    /// <typeparam name="TOut">リスト要素出力型</typeparam>
    public interface IReadOnlyExtendedList<TIn, TOut> :
        IReadableList<TOut>,
        INotifiableCollectionChange<TOut>,
        IContainerCreatable, IContainerCreatable<ListInitParam<TIn>>,
        IEqualityComparable<IReadOnlyExtendedList<TIn, TOut>>,
        IContainerCreatableParam
        where TOut : TIn
    {
        /// <summary>
        ///     現在のオブジェクトが、同じ型の別のリストと同値であるかどうかを示す。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <param name="itemComparer">比較対象との比較に使用する比較処理</param>
        /// <typeparam name="TOther">比較対象型。</typeparam>
        /// <returns>
        ///     同値 または 同一 である場合 <see langword="true"/>。
        /// </returns>
        public bool ItemEquals<TOther>(IEnumerable<TOther>? other, IEqualityComparer<TOther>? itemComparer = null);
    }

    /// <summary>
    ///     WodiLib 独自読み取り専用リストインタフェース
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         <see cref="IReadOnlyList{T}"/> のメソッドと <see cref="ObservableCollection{T}"/> の機能を融合した機能。
    ///         <see cref="ObservableCollection{T}"/> のCRUD各種処理に範囲指定バージョン（XXXRange メソッド）を追加している。
    ///         それ以外にもいくつかメソッドを追加している。
    ///     </para>
    ///     <para>
    ///         <see cref="ObservableCollection{T}"/> とは異なり、
    ///         <typeparamref name="T"/> が変更通知を行うクラスだった場合、
    ///         通知を受け取ると自身の "Items[]" プロパティ変更通知を行う。
    ///     </para>
    /// </remarks>
    /// <typeparam name="T">リスト要素型</typeparam>
    public interface IReadOnlyExtendedList<T> : IReadOnlyExtendedList<T, T>
    {
    }
}
