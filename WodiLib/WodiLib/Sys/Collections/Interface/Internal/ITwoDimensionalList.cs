// ========================================
// Project Name : WodiLib
// File Name    : ITwoDimensionalList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     WodiLib内部で使用する二次元リスト
    /// </summary>
    /// <remarks>
    ///     外部公開用の二次元リストはこのインタフェースを継承せずに作成する。
    ///     データベースにおける「データ」と「項目」のような、「行」や「列」の呼び方が変わる場合があることを考慮。
    /// </remarks>
    /// <typeparam name="TInRow">リスト行データ入力型</typeparam>
    /// <typeparam name="TOutRow">リスト行データ出力型</typeparam>
    /// <typeparam name="TInItem">リスト要素入力型</typeparam>
    /// <typeparam name="TOutItem">リスト要素出力型</typeparam>
    internal interface ITwoDimensionalList<TInRow, TOutRow, TInItem, TOutItem> :
        IReadableTwoDimensionalList<TOutRow, TOutItem>,
        IWritableTwoDimensionalList<TInRow, TOutRow, TInItem>,
        ISizeChangeableTwoDimensionalList<TInRow, TOutRow, TInItem>,
        INotifiableCollectionChange<TOutRow>,
        IEqualityComparable<ITwoDimensionalList<TInRow, TOutRow, TInItem, TOutItem>>
        where TInRow : IEnumerable<TInItem>
        where TOutRow : IEnumerable<TOutItem>, TInRow
        where TOutItem : TInItem
    {
        /// <summary>
        ///     インデクサによるアクセス
        /// </summary>
        /// <param name="rowIndex">[Range(0, <see cref="ITwoDimensionalListProperty.RowCount"/> - 1)] 行インデックス</param>
        /// <param name="columnIndex">[Range(0, <see cref="ITwoDimensionalListProperty.ColumnCount"/> - 1)] 列インデックス</param>
        /// <returns>指定したインデックスの要素</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="rowIndex"/>が指定範囲外の場合。</exception>
        public new TInItem this[int rowIndex, int columnIndex] { get; set; }

        /// <summary>
        /// すべての行要素に対し <see cref="INotifyPropertyChanging"/> イベントを登録する。
        /// </summary>
        /// <remarks>
        /// <para>
        ///     このメソッドで登録したイベントは、要素がリストから除去されるときに同時に解除される。
        ///     また、新規行データが追加された場合には自動でイベントが付与される。
        /// </para>
        /// <para>
        ///     <see cref="AddRowPropertyChanging"/> メソッドで登録したイベントを任意のタイミングで解除するには
        ///     <see cref="RemoveRowPropertyChanging"/> を実行する。
        /// </para>
        /// </remarks>
        /// <param name="handler">登録するイベント</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="handler"/> が <see langword="null"/> の場合。
        /// </exception>
        public void AddRowPropertyChanging(PropertyChangingEventHandler handler);

        /// <summary>
        /// 指定したインデックスの要素に登録した <see cref="INotifyPropertyChanging"/> イベントを解除する。
        /// </summary>
        /// <remarks>
        /// <para>
        /// <paramref name="handler"/> が <see cref="AddRowPropertyChanging"/> を通して登録されたものでない場合はなにもしない。
        /// </para>
        /// </remarks>
        /// <param name="handler">解除するイベント</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="handler"/> が <see langword="null"/> の場合。
        /// </exception>
        public void RemoveRowPropertyChanging(PropertyChangingEventHandler handler);

        /// <summary>
        /// すべての行要素に対し <see cref="INotifyPropertyChanged"/> イベントを登録する。
        /// </summary>
        /// <remarks>
        /// <para>
        /// このメソッドで登録したイベントは、要素がリストから除去されるときに同時に解除される。
        ///     また、新規行データが追加された場合には自動でイベントが付与される。
        /// </para>
        /// <para>
        ///     <see cref="AddRowPropertyChanged"/> メソッドで登録したイベントを任意のタイミングで解除するには
        ///     <see cref="RemoveRowPropertyChanged"/> を実行する。
        /// </para>
        /// </remarks>
        /// <param name="handler">登録するイベント</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="handler"/> が <see langword="null"/> の場合。
        /// </exception>
        public void AddRowPropertyChanged(PropertyChangedEventHandler handler);

        /// <summary>
        /// 指定したインデックスの要素に登録した <see cref="INotifyPropertyChanged"/> イベントを解除する。
        /// </summary>
        /// <remarks>
        /// <para>
        /// <paramref name="handler"/> が <see cref="AddRowPropertyChanged"/> を通して登録されたものでない場合はなにもしない。
        /// </para>
        /// </remarks>
        /// <param name="handler">解除するイベント</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="handler"/> が <see langword="null"/> の場合。
        /// </exception>
        public void RemoveRowPropertyChanged(PropertyChangedEventHandler handler);

        /// <summary>
        /// すべての行要素に対し <see cref="INotifiableCollectionChange.CollectionChanging"/> イベントを登録する。
        /// </summary>
        /// <remarks>
        /// <para>
        ///     このメソッドで登録したイベントは、要素がリストから除去されるときに同時に解除される。
        ///     また、新規行データが追加された場合には自動でイベントが付与される。
        /// </para>
        /// <para>
        ///     <see cref="AddRowCollectionChanging(EventHandler{NotifyCollectionChangedEventArgs})"/> メソッドで登録したイベントを任意のタイミングで解除するには
        ///     <see cref="RemoveRowCollectionChanging(EventHandler{NotifyCollectionChangedEventArgs})"/> を実行する。
        /// </para>
        /// </remarks>
        /// <param name="handler">登録するイベント</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="handler"/> が <see langword="null"/> の場合。
        /// </exception>
        public void AddRowCollectionChanging(EventHandler<NotifyCollectionChangedEventArgs> handler);

        /// <summary>
        /// 指定したインデックスの要素に登録した <see cref="INotifiableCollectionChange.CollectionChanging"/> イベントを解除する。
        /// </summary>
        /// <remarks>
        /// <para>
        /// <paramref name="handler"/> が <see cref="AddRowCollectionChanging(EventHandler{NotifyCollectionChangedEventArgs})"/> を通して登録されたものでない場合はなにもしない。
        /// </para>
        /// </remarks>
        /// <param name="handler">解除するイベント</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="handler"/> が <see langword="null"/> の場合。
        /// </exception>
        public void RemoveRowCollectionChanging(EventHandler<NotifyCollectionChangedEventArgs> handler);

        /// <summary>
        /// すべての行要素に対し <see cref="INotifiableCollectionChange{T}.CollectionChanging"/> イベントを登録する。
        /// </summary>
        /// <remarks>
        /// <para>
        ///     このメソッドで登録したイベントは、要素がリストから除去されるときに同時に解除される。
        ///     また、新規行データが追加された場合には自動でイベントが付与される。
        /// </para>
        /// <para>
        ///     <see cref="AddRowCollectionChanging(EventHandler{NotifyCollectionChangedEventArgsEx{TOutItem}})" /> メソッドで登録したイベントを任意のタイミングで解除するには
        ///     <see cref="RemoveRowCollectionChanging(EventHandler{NotifyCollectionChangedEventArgsEx{TOutItem}})" /> を実行する。
        /// </para>
        /// </remarks>
        /// <param name="handler">登録するイベント</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="handler"/> が <see langword="null"/> の場合。
        /// </exception>
        public void AddRowCollectionChanging(EventHandler<NotifyCollectionChangedEventArgsEx<TOutItem>> handler);

        /// <summary>
        /// 指定したインデックスの要素に登録した <see cref="INotifiableCollectionChange{T}.CollectionChanging"/> イベントを解除する。
        /// </summary>
        /// <remarks>
        /// <para>
        /// <paramref name="handler"/> が <see cref="AddRowCollectionChanging(EventHandler{NotifyCollectionChangedEventArgsEx{TOutItem}})"/> を通して登録されたものでない場合はなにもしない。
        /// </para>
        /// </remarks>
        /// <param name="handler">解除するイベント</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="handler"/> が <see langword="null"/> の場合。
        /// </exception>
        public void RemoveRowCollectionChanging(EventHandler<NotifyCollectionChangedEventArgsEx<TOutItem>> handler);

        /// <summary>
        /// すべての行要素に対し <see cref="INotifiableCollectionChange.CollectionChanging"/> イベントを登録する。
        /// </summary>
        /// <remarks>
        /// <para>
        ///     このメソッドで登録したイベントは、要素がリストから除去されるときに同時に解除される。
        ///     また、新規行データが追加された場合には自動でイベントが付与される。
        /// </para>
        /// <para>
        ///     AddRowCollectionChanging メソッドで登録したイベントを任意のタイミングで解除するには
        ///     RemoveRowCollectionChanging を実行する。
        /// </para>
        /// </remarks>
        /// <param name="handler">登録するイベント</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="handler"/> が <see langword="null"/> の場合。
        /// </exception>
        public void AddRowCollectionChanged(NotifyCollectionChangedEventHandler handler);

        /// <summary>
        /// 指定したインデックスの要素に登録した <see cref="INotifiableCollectionChange.CollectionChanging"/> イベントを解除する。
        /// </summary>
        /// <remarks>
        /// <para>
        /// <paramref name="handler"/> が <see cref="AddRowCollectionChanging(EventHandler{NotifyCollectionChangedEventArgs})"/> を通して登録されたものでない場合はなにもしない。
        /// </para>
        /// </remarks>
        /// <param name="handler">解除するイベント</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="handler"/> が <see langword="null"/> の場合。
        /// </exception>
        public void RemoveRowCollectionChanged(NotifyCollectionChangedEventHandler handler);

        /// <summary>
        /// すべての行要素に対し <see cref="INotifiableCollectionChange{T}.CollectionChanged"/> イベントを登録する。
        /// </summary>
        /// <remarks>
        /// <para>
        ///     このメソッドで登録したイベントは、要素がリストから除去されるときに同時に解除される。
        ///     また、新規行データが追加された場合には自動でイベントが付与される。
        /// </para>
        /// <para>
        ///     <see cref="AddRowCollectionChanged(EventHandler{NotifyCollectionChangedEventArgsEx{TOutItem}})" /> メソッドで登録したイベントを任意のタイミングで解除するには
        ///     <see cref="RemoveRowCollectionChanged(EventHandler{NotifyCollectionChangedEventArgsEx{TOutItem}})" /> を実行する。
        /// </para>
        /// </remarks>
        /// <param name="handler">登録するイベント</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="handler"/> が <see langword="null"/> の場合。
        /// </exception>
        public void AddRowCollectionChanged(EventHandler<NotifyCollectionChangedEventArgsEx<TOutItem>> handler);

        /// <summary>
        /// 指定したインデックスの要素に登録した <see cref="INotifiableCollectionChange{T}.CollectionChanged"/> イベントを解除する。
        /// </summary>
        /// <remarks>
        /// <para>
        /// <paramref name="handler"/> が <see cref="AddRowCollectionChanged(EventHandler{NotifyCollectionChangedEventArgsEx{TOutItem}})"/> を通して登録されたものでない場合はなにもしない。
        /// </para>
        /// </remarks>
        /// <param name="handler">解除するイベント</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="handler"/> が <see langword="null"/> の場合。
        /// </exception>
        public void RemoveRowCollectionChanged(EventHandler<NotifyCollectionChangedEventArgsEx<TOutItem>> handler);

        /// <summary>
        ///     自身の全要素を簡易コピーした二次元配列を返す。
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         返す二次元配列の状態は <paramref name="isTranspose"/> によって変化する。<br/>
        ///         <paramref name="isTranspose"/> が <see langword="false"/> の場合、
        ///         自身の要素をそのまま返す。<br/>
        ///         <paramref name="isTranspose"/> が <see langword="true"/> の場合、
        ///         自身を転置した状態の要素を返す。<br/>
        ///     </para>
        /// </remarks>
        /// <param name="isTranspose">転置フラグ</param>
        /// <returns>自身の全要素簡易コピー配列</returns>
        public TOutItem[][] ToTwoDimensionalArray(bool isTranspose = false);
    }
}
