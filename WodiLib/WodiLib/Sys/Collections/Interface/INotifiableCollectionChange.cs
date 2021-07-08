// ========================================
// Project Name : WodiLib
// File Name    : INotifiableCollectionChange.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

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
    ///         変更通知にかかるコストは <see cref="NotifyCollectionChangingEventType"/> および
    ///         <see cref="NotifyCollectionChangedEventType"/> どちらにも
    ///         <see cref="NotifyCollectionChangeEventType.None"/> を指定している場合が最も小さく、<br/>
    ///         次いで <see cref="NotifyCollectionChangingEventType"/>,
    ///         <see cref="NotifyCollectionChangedEventType"/> どちらかに
    ///         <see cref="NotifyCollectionChangeEventType.None"/> 以外を指定している場合に小さくなる。<br/>
    ///         <see cref="NotifyCollectionChangingEventType"/> および
    ///         <see cref="NotifyCollectionChangedEventType"/> に同じ種別を設定している場合
    ///         どちらか一方に <see cref="NotifyCollectionChangeEventType.None"/> をしている場合より
    ///         わずかに大きなコストで実行可能。<br/>
    ///         <see cref="NotifyCollectionChangingEventType"/> と
    ///         <see cref="NotifyCollectionChangedEventType"/> に異なる種別を設定している場合
    ///         それぞれの通知で個別にイベント引数インスタンスを生成するため処理コストが大きくなる。
    ///     </para>
    ///     <para>
    ///         <see cref="NotifyCollectionChangingEventType"/> や <see cref="NotifyCollectionChangedEventType"/> に
    ///         <see cref="NotifyCollectionChangeEventType.Once"/>, <see cref="NotifyCollectionChangeEventType.Simple"/>,
    ///         <see cref="NotifyCollectionChangeEventType.Single"/> のいずれかを指定した状態で
    ///         WPFのUIにバインドし、範囲操作するメソッドを実行して複数の要素を操作すると例外が発生するため注意。
    ///     </para>
    /// </remarks>
    public interface INotifiableCollectionChange : INotifiablePropertyChange,
        INotifyCollectionChanged
    {
        /// <summary>
        ///     要素変更前通知
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanging;

        /// <summary>
        ///     <see cref="INotifiableCollectionChange.CollectionChanging"/> を通知するか否か。<br/>
        ///     <see langword="true"/> の場合、このインスタンスはプロパティが変化する際に
        ///     <see cref="INotifiableCollectionChange.CollectionChanging"/> イベントを通知する。
        /// </summary>
        public NotifyCollectionChangeEventType NotifyCollectionChangingEventType { get; set; }

        /// <summary>
        ///     <see cref="INotifyCollectionChanged.CollectionChanged"/> を通知するか否か。<br/>
        ///     <see langword="true"/> の場合、このインスタンスはプロパティが変化した際に
        ///     <see cref="INotifyCollectionChanged.CollectionChanged"/> イベントを通知する。
        /// </summary>
        public NotifyCollectionChangeEventType NotifyCollectionChangedEventType { get; set; }
    }
}
