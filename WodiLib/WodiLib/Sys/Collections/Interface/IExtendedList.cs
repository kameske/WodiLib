// ========================================
// Project Name : WodiLib
// File Name    : IExtendedList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace WodiLib.Sys
{
    /// <summary>
    /// WodiLib 独自リストインタフェース
    /// </summary>
    /// <remarks>
    /// <see cref="IList{T}"/> のメソッドと <see cref="ObservableCollection{T}"/> の機能を融合した機能。
    /// <see cref="ObservableCollection{T}"/> のCRUD各種処理に範囲指定バージョン（XXXRange メソッド）を追加している。
    /// それ以外にもいくつかメソッドを追加している。<br/>
    /// 範囲操作メソッド実行時に通知される <see cref="CollectionChanging"/> および <see cref="INotifyCollectionChanged.CollectionChanged"/> は
    /// 要素をいくつ変更してもそれぞれ1度だけ呼ばれる。たとえ操作した要素数が0個であっても呼ばれる。<br/>
    /// 操作前後の要素は <see cref="NotifyCollectionChangedEventArgs"/> の Items を
    /// <see cref="IList{T}"/> にキャストすることで取り出せる。
    /// この弊害として、WPFのUIにバインドした状態で範囲操作するメソッドを実行すると例外が発生するため注意。
    /// </remarks>
    /// <typeparam name="T">リスト内包クラス</typeparam>
    public interface IExtendedList<T> : IModelBase<IExtendedList<T>>, IList<T>, IReadOnlyExtendedList<T>
    {
        /*
         * IList<T> と IReadOnlyList<T>、
         * IList<T> と IReadOnlyExtendedList<T> で重複する定義はこのインタフェースで再定義する。
         */

        /// <inheritdoc cref="IList{T}.this" />
        public new T this[int index] { get; set; }

        /// <inheritdoc cref="IList{T}.Count" />
        public new int Count { get; }

        /// <summary>
        /// 要素変更前通知
        /// </summary>
        /// <remarks>
        ///     同じイベントを重複して登録することはできない。
        /// </remarks>
        public event NotifyCollectionChangedEventHandler CollectionChanging;

        /// <summary>
        /// リストの連続した要素を更新する。
        /// </summary>
        /// <param name="index">[Range(0, <see cref="IReadOnlyList{T}.Count"/> - 1)] 更新開始インデックス</param>
        /// <param name="items">更新要素</param>
        public void SetRange(int index, IEnumerable<T> items);

        /// <summary>
        /// リストの末尾に要素を追加する。
        /// </summary>
        /// <param name="items">追加する要素</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="items" /> が <see langword="null" /> の場合、
        ///     または <paramref name="items" /> に <see langword="null" /> 要素が含まれる場合
        /// </exception>
        public void AddRange(IEnumerable<T> items);

        /// <summary>
        /// 指定したインデックスの位置に要素を挿入する。
        /// </summary>
        /// <param name="index">[Range(0, <see cref="IReadOnlyList{T}.Count"/>)] インデックス</param>
        /// <param name="items">追加する要素</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="index" /> が指定範囲外の場合
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="items" /> が <see langword="null" /> の場合、
        ///     または <paramref name="items" /> に <see langword="null" /> 要素が含まれる場合
        /// </exception>
        public void InsertRange(int index, IEnumerable<T> items);

        /// <summary>
        /// 指定したインデックスを起点として、要素の上書き/追加を行う。
        /// </summary>
        /// <param name="index">[Range(0, <see cref="IReadOnlyList{T}.Count"/>)] インデックス</param>
        /// <param name="items">上書き/追加リスト</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="index" /> が指定範囲外の場合
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="items" /> が <see langword="null" /> の場合
        ///     または <paramref name="items" /> に <see langword="null" /> 要素が含まれる場合
        /// </exception>
        /// <example>
        ///     <code>
        ///     var target = new List&lt;int&gt; { 0, 1, 2, 3 };
        ///     var dst = new List&lt;int&gt; { 10, 11, 12 };
        ///     target.Overwrite(2, dst);
        ///     // target is { 0, 1, 10, 11, 12 }
        ///     </code>
        ///     <code>
        ///     var target = new List&lt;int&gt; { 0, 1, 2, 3 };
        ///     var dst = new List&lt;int&gt; { 10 };
        ///     target.Overwrite(2, dst);
        ///     // target is { 0, 1, 10, 3 }
        ///     </code>
        /// </example>
        public void Overwrite(int index, IEnumerable<T> items);

        /// <summary>
        /// 指定したインデックスにある項目をコレクション内の新しい場所へ移動する。
        /// </summary>
        /// <param name="oldIndex">[Range(0, <see cref="IReadOnlyList{T}.Count"/> - 1)] 移動する項目のインデックス</param>
        /// <param name="newIndex">[Range(0, <see cref="IReadOnlyList{T}.Count"/> - 1)] 移動先のインデックス</param>
        /// <exception cref="InvalidOperationException">
        ///    自身の要素数が0の場合
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="oldIndex"/>, <paramref name="newIndex"/> が指定範囲外の場合
        /// </exception>
        public void Move(int oldIndex, int newIndex);

        /// <summary>
        /// 指定したインデックスから始まる連続した項目をコレクション内の新しい場所へ移動する。
        /// </summary>
        /// <param name="oldIndex">
        ///     [Range(0, <see cref="IReadOnlyList{T}.Count"/> - 1)]
        ///     移動する項目のインデックス開始位置
        /// </param>
        /// <param name="newIndex">
        ///     [Range(0, <see cref="IReadOnlyList{T}.Count"/> - <paramref name="count"/>)]
        ///     移動先のインデックス開始位置
        /// </param>
        /// <param name="count">
        ///     [Range(0, <see cref="IReadOnlyList{T}.Count"/> - <paramref name="oldIndex"/>)]
        ///     移動させる要素数
        /// </param>
        /// <exception cref="InvalidOperationException">
        ///    自身の要素数が0の場合
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="oldIndex"/>, <paramref name="newIndex"/>, <paramref name="count"/> が指定範囲外の場合
        /// </exception>
        /// <exception cref="ArgumentException">有効な範囲外の要素を移動しようとした場合</exception>
        public void MoveRange(int oldIndex, int newIndex, int count);

        /// <summary>
        /// 要素の範囲を削除する。
        /// </summary>
        /// <param name="index">[Range(0, <see cref="IReadOnlyList{T}.Count"/> - 1)] インデックス</param>
        /// <param name="count">[Range(0, <see cref="IReadOnlyList{T}.Count"/>)] 削除する要素数</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="index" />, <paramref name="count" /> が指定範囲外の場合</exception>
        /// <exception cref="ArgumentException">有効な範囲外の要素を削除しようとした場合</exception>
        public void RemoveRange(int index, int count);

        /// <summary>
        /// 要素数を指定の数に合わせる。
        /// </summary>
        /// <param name="length">[Range(0, <see cref="int.MaxValue"/>)] 調整する要素数</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="length" /> が指定範囲外の場合
        /// </exception>
        public void AdjustLength(int length);

        /// <summary>
        /// 要素数が不足している場合、要素数を指定の数に合わせる。
        /// </summary>
        /// <param name="length">[Range(0, <see cref="int.MaxValue"/>)] 調整する要素数</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="length" /> が指定範囲外の場合
        /// </exception>
        public void AdjustLengthIfShort(int length);

        /// <summary>
        /// 要素数が超過している場合、要素数を指定の数に合わせる。
        /// </summary>
        /// <param name="length">[Range(0, <see cref="int.MaxValue"/>)] 調整する要素数</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="length" /> が指定範囲外の場合
        /// </exception>
        public void AdjustLengthIfLong(int length);

        /// <summary>
        /// 要素を与えられた内容で一新する。
        /// </summary>
        /// <param name="initItems">リストに詰め直す要素</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="initItems" /> が <see langword="null" /> の場合、
        ///     または<paramref name="initItems" /> に <see langword="null" /> 要素が含まれる場合
        /// </exception>
        public void Reset(IEnumerable<T> initItems);

        /// <inheritdoc cref="IReadOnlyExtendedList{T}.Contains" />
        public new bool Contains([AllowNull] T item);

        /// <inheritdoc cref="IReadOnlyExtendedList{T}.IndexOf" />
        public new int IndexOf([AllowNull] T item);

        /// <inheritdoc cref="IReadOnlyExtendedList{T}.CopyTo" />
        public new void CopyTo(T[] array, int index);
    }

    /// <summary>
    /// WodiLib 独自リ読み取り専用ストインタフェース
    /// </summary>
    /// <typeparam name="T">リスト内包クラス</typeparam>
    public interface IReadOnlyExtendedList<T> : IModelBase<IReadOnlyExtendedList<T>>,
        IReadOnlyList<T>, IEquatable<IReadOnlyList<T>>, IEquatable<IEnumerable<T>>,
        INotifyCollectionChanged, ISerializable
    {
        /// <summary>
        /// 指定範囲の要素を簡易コピーしたリストを取得する。
        /// </summary>
        /// <param name="index">[Range(0, <see cref="IReadOnlyList{T}.Count"/> - 1)] インデックス</param>
        /// <param name="count">[Range(0, <see cref="IReadOnlyList{T}.Count"/>)] 要素数</param>
        /// <returns>指定範囲の要素簡易コピーリスト</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="index" />, <paramref name="count" />が指定範囲外の場合
        /// </exception>
        /// <exception cref="ArgumentException">有効な範囲外の要素を取得しようとした場合</exception>
        public IEnumerable<T> GetRange(int index, int count);

        /// <summary>
        /// 指定の要素が含まれているか判断する。
        /// </summary>
        /// <param name="item">対象要素</param>
        /// <returns>指定の要素が含まれる場合は <see langword="true" />、含まれない場合は <see langword="false" />。</returns>
        public bool Contains([AllowNull] T item);

        /// <summary>
        /// 指定したオブジェクトを検索し、最初に出現する位置のインデックスを返す。
        /// </summary>
        /// <param name="item">対象要素</param>
        /// <returns>要素が含まれていない場合、-1</returns>
        public int IndexOf([AllowNull] T item);

        /// <summary>
        /// すべての要素を、指定された配列のインデックスから始まる部分にコピーする。
        /// </summary>
        /// <param name="array">コピー先の配列</param>
        /// <param name="index">[Range(0, <see cref="IReadOnlyCollection{T}.Count"/> - 1)] コピー開始インデックス</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="array" /> が <see langword="null" /> の場合
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="index" /> が 0 未満の場合
        /// </exception>
        /// <exception cref="ArgumentException">コピー先の領域が不足する場合</exception>
        public void CopyTo(T[] array, int index);
    }
}
