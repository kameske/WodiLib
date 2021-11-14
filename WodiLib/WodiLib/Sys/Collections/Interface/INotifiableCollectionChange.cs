// ========================================
// Project Name : WodiLib
// File Name    : INotifiableCollectionChange.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Specialized;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     <see cref="CollectionChanging"/> および
    ///     <see cref="INotifyCollectionChanged.CollectionChanged"/> を実装していることを示すインタフェース
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         <see cref="NotifyCollectionChangingEventType"/> および
    ///         <see cref="NotifyCollectionChangedEventType"/> の設定値によって
    ///         通知イベントの種類を決定できる。
    ///         通知イベント種類の詳細については <see cref="NotifyCollectionChangeEventType"/> 参照。
    ///     </para>
    ///     <para>
    ///         <see cref="NotifyCollectionChangingEventType"/> や <see cref="NotifyCollectionChangedEventType"/> に
    ///         <see cref="NotifyCollectionChangeEventType.Once"/>, <see cref="NotifyCollectionChangeEventType.Simple"/>,
    ///         <see cref="NotifyCollectionChangeEventType.Single"/> のいずれかを指定した状態で
    ///         WPFのUIにバインドし、範囲操作するメソッドを実行して複数の要素を操作すると例外が発生するため注意。
    ///     </para>
    /// </remarks>
    /// <typeparam name="T">コレクション要素型</typeparam>
    public interface INotifiableCollectionChange<T> : INotifiablePropertyChange,
        INotifyCollectionChanged
    {
        /// <summary>
        ///     要素変更前通知
        /// </summary>
        public event EventHandler<NotifyCollectionChangedEventArgsEx<T>> CollectionChanging;

        /// <summary>
        ///     要素変更後通知
        /// </summary>
        public new event EventHandler<NotifyCollectionChangedEventArgsEx<T>> CollectionChanged;

        /// <summary>
        ///     <see cref="INotifiableCollectionChange{T}.CollectionChanging"/> 通知種別
        /// </summary>
        public NotifyCollectionChangeEventType NotifyCollectionChangingEventType { get; set; }

        /// <summary>
        ///     <see cref="INotifyCollectionChanged.CollectionChanged"/> 通知種別
        /// </summary>
        public NotifyCollectionChangeEventType NotifyCollectionChangedEventType { get; set; }
    }
}
