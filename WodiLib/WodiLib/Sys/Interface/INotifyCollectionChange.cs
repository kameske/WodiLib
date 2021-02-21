// ========================================
// Project Name : WodiLib
// File Name    : INotifyCollectionChange.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Specialized;

namespace WodiLib.Sys
{
    /// <summary>
    ///     <see cref="IReadOnlyNotifyCollectionChange.CollectionChanging"/> および
    ///     <see cref="INotifyCollectionChanged.CollectionChanged"/> を実装していることを示すインタフェース
    /// </summary>
    /// <remarks>
    ///     <see cref="IsNotifyBeforeCollectionChange"/> および
    ///     <see cref="IsNotifyAfterCollectionChange"/> の設定値によって
    ///     通知イベントの有無を決定できる。
    /// </remarks>
    public interface INotifyCollectionChange : IReadOnlyNotifyCollectionChange
    {
        /// <inheritdoc cref="IReadOnlyNotifyCollectionChange.IsNotifyBeforeCollectionChange"/>
        public new bool IsNotifyBeforeCollectionChange { get; set; }

        /// <inheritdoc cref="IReadOnlyNotifyCollectionChange.IsNotifyAfterCollectionChange"/>
        public new bool IsNotifyAfterCollectionChange { get; set; }
    }

    /// <summary>
    ///     <see cref="CollectionChanging"/> および
    ///     <see cref="INotifyCollectionChanged.CollectionChanged"/> を実装していることを示すインタフェース
    /// </summary>
    /// <remarks>
    ///     実際に各イベントが通知されるかどうかは <see cref="IsNotifyBeforeCollectionChange"/> および
    ///     <see cref="IsNotifyAfterCollectionChange"/> の設定値によって決まる。
    /// </remarks>
    public interface IReadOnlyNotifyCollectionChange : INotifyCollectionChanged
    {
        /// <summary>
        ///     要素変更前通知
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanging;

        /// <summary>
        ///     <see cref="INotifyCollectionChange.CollectionChanging"/> を通知するか否か。<br/>
        ///     <see langword="true"/> の場合、このインスタンスはプロパティが変化する際に
        ///     <see cref="INotifyCollectionChange.CollectionChanging"/> イベントを通知する。
        /// </summary>
        public bool IsNotifyBeforeCollectionChange { get; }

        /// <summary>
        ///     <see cref="INotifyCollectionChanged.CollectionChanged"/> を通知するか否か。<br/>
        ///     <see langword="true"/> の場合、このインスタンスはプロパティが変化した際に
        ///     <see cref="INotifyCollectionChanged.CollectionChanged"/> イベントを通知する。
        /// </summary>
        public bool IsNotifyAfterCollectionChange { get; }
    }
}
