// ========================================
// Project Name : WodiLib
// File Name    : INotifiableTwoDimensionalListChangeInternal.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     <see cref="TwoDimensionalListChanging"/> および
    ///     <see cref="TwoDimensionalListChanged"/> を実装していることを示すインタフェース
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         <see cref="NotifyTwoDimensionalListChangingEventType"/> および
    ///         <see cref="NotifyTwoDimensionalListChangedEventType"/> の設定値によって
    ///         通知イベントの種類を決定できる。
    ///         通知イベント種類の詳細については <see cref="NotifyTwoDimensionalListChangeEventType"/> 参照。
    ///     </para>
    ///     <para>
    ///         <see cref="NotifyTwoDimensionalListChangingEventType"/> や
    ///         <see cref="NotifyTwoDimensionalListChangedEventType"/> に
    ///         <see cref="NotifyCollectionChangeEventType.Once"/>, <see cref="NotifyCollectionChangeEventType.Simple"/>,
    ///         <see cref="NotifyCollectionChangeEventType.Single"/> のいずれかを指定した状態で
    ///         WPFのUIにバインドし、範囲操作するメソッドを実行して複数の要素を操作すると例外が発生するため注意。
    ///     </para>
    /// </remarks>
    /// <typeparam name="T">コレクション要素型</typeparam>
    internal interface INotifiableTwoDimensionalListChangeInternal<T> : INotifiableCollectionChange<IReadOnlyList<T>>
    {
        /// <summary>
        ///     要素変更前通知
        /// </summary>
        public event EventHandler<TwoDimensionalCollectionChangeEventInternalArgs<T>> TwoDimensionalListChanging;

        /// <summary>
        ///     要素変更後通知
        /// </summary>
        public event EventHandler<TwoDimensionalCollectionChangeEventInternalArgs<T>> TwoDimensionalListChanged;

        /// <summary>
        ///     <see cref="TwoDimensionalListChanging"/> 通知種別
        /// </summary>
        public NotifyTwoDimensionalListChangeEventType NotifyTwoDimensionalListChangingEventType { get; set; }

        /// <summary>
        ///     <see cref="TwoDimensionalListChanged"/> 通知種別
        /// </summary>
        public NotifyTwoDimensionalListChangeEventType NotifyTwoDimensionalListChangedEventType { get; set; }
    }
}
