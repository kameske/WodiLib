// ========================================
// Project Name : WodiLib
// File Name    : IWritableList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     サイズ変更可能であることを表すインタフェース。
    ///     書き込み可能でもある。
    /// </summary>
    /// <typeparam name="TItem">リスト要素型</typeparam>
    /// <typeparam name="TImpl">実装型</typeparam>
    /// <typeparam name="TWritable"><see cref="IWritableList{TItem,TImpl,TReadable}"/>実装型</typeparam>
    /// <typeparam name="TReadable"><see cref="IReadableList{TItem,TImpl}"/>実装型</typeparam>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface ISizeChangeableList<TItem, out TImpl, out TWritable, out TReadable> :
        IWritableList<TItem, TWritable, TReadable>, IList<TItem>
        where TImpl : ISizeChangeableList<TItem, TImpl, TWritable, TReadable>,
        IWritableList<TItem, TWritable, TReadable>,
        IReadableList<TItem, TReadable>
        where TWritable : IWritableList<TItem, TWritable, TReadable>
        where TReadable : IReadableList<TItem, TReadable>
    {
        /// <inheritdoc cref="IList{T}.this"/>
        public new TItem this[int index] { get; set; }

        /// <inheritdoc cref="IList{T}.Count"/>
        public new int Count { get; }

        /// <summary>
        ///     容量最大値を返す。
        /// </summary>
        /// <returns>容量最大値</returns>
        public int GetMaxCapacity();

        /// <summary>
        ///     容量最小値を返す。
        /// </summary>
        /// <returns>容量最小値</returns>
        public int GetMinCapacity();

        /// <inheritdoc cref="IList{T}.Contains"/>
        public new bool Contains([AllowNull] TItem item);

        /// <inheritdoc cref="IList{T}.IndexOf"/>
        public new int IndexOf([AllowNull] TItem item);

        /// <inheritdoc cref="IReadOnlyExtendedList{T}.CopyTo"/>
        public new void CopyTo(TItem[] array, int index);

        /// <summary>
        ///     リストの末尾に要素を追加する。
        /// </summary>
        /// <param name="item">追加する要素</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="item"/> が <see langword="null"/> の場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     操作によって要素数が <see cref="GetMaxCapacity"/> を上回る場合。
        /// </exception>
        public new void Add(TItem item);

        /// <summary>
        ///     リストの末尾に要素を追加する。
        /// </summary>
        /// <param name="items">追加する要素</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="items"/> が <see langword="null"/> の場合、
        ///     または <paramref name="items"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     操作によって要素数が <see cref="GetMaxCapacity"/> を上回る場合。
        /// </exception>
        public void AddRange(IEnumerable<TItem> items);

        /// <summary>
        ///     指定したインデックスの位置に要素を挿入する。
        /// </summary>
        /// <param name="index">[Range(0, <see cref="Count"/>)] インデックス</param>
        /// <param name="item">追加する要素</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="index"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="item"/> が <see langword="null"/> の場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     操作によって要素数が <see cref="GetMaxCapacity"/> を上回る場合。
        /// </exception>
        public new void Insert(int index, TItem item);

        /// <summary>
        ///     指定したインデックスの位置に要素を挿入する。
        /// </summary>
        /// <param name="index">[Range(0, <see cref="Count"/>)] インデックス</param>
        /// <param name="items">追加する要素</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="index"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="items"/> が <see langword="null"/> の場合、
        ///     または <paramref name="items"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     操作によって要素数が <see cref="GetMaxCapacity"/> を超える場合。
        /// </exception>
        public void InsertRange(int index, IEnumerable<TItem> items);

        /// <summary>
        ///     指定したインデックスを起点として、要素の上書き/追加を行う。
        /// </summary>
        /// <param name="index">[Range(0, <see cref="Count"/>)] インデックス</param>
        /// <param name="items">上書き/追加リスト</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="index"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="items"/> が <see langword="null"/> の場合、
        ///     または <paramref name="items"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     操作によって要素数が <see cref="GetMaxCapacity"/> を超える場合。
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
        public void Overwrite(int index, IEnumerable<TItem> items);

        /// <summary>
        ///     特定のオブジェクトで最初に出現したものを削除する。
        /// </summary>
        /// <param name="item">削除対象オブジェクト</param>
        /// <returns>
        ///     <paramref name="item"/> が存在する場合 <see langword="true"/>。
        ///     それ以外の場合は <see langword="false"/>。
        /// </returns>
        /// <exception cref="ArgumentException">
        ///     操作によって要素数が <see cref="GetMinCapacity"/> を下回る場合。
        /// </exception>
        public new bool Remove([AllowNull] TItem item);

        /// <summary>
        ///     指定したインデックスの要素を削除する。
        /// </summary>
        /// <param name="index">[Range(0, <see cref="Count"/> - 1)] インデックス</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="index"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     操作によって要素数が <see cref="GetMinCapacity"/> を下回る場合。
        /// </exception>
        public new void RemoveAt(int index);

        /// <summary>
        ///     要素の範囲を削除する。
        /// </summary>
        /// <param name="index">[Range(0, <see cref="Count"/> - 1)] インデックス</param>
        /// <param name="count">[Range(0, <see cref="Count"/>)] 削除する要素数</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="index"/>, <paramref name="count"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     有効な範囲外の要素を削除しようとした場合、
        ///     または 操作によって要素数が <see cref="GetMinCapacity"/> を下回る場合。
        /// </exception>
        public void RemoveRange(int index, int count);

        /// <summary>
        ///     要素数を指定の数に合わせる。
        /// </summary>
        /// <param name="length">
        ///     [Range(<see cref="GetMinCapacity"/>, <see cref="GetMaxCapacity"/>)]
        ///     調整する要素数
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="length"/> が指定範囲外の場合。
        /// </exception>
        public void AdjustLength(int length);

        /// <summary>
        ///     要素数が不足している場合、要素数を指定の数に合わせる。
        /// </summary>
        /// <param name="length">
        ///     [Range(<see cref="GetMinCapacity"/>, <see cref="GetMaxCapacity"/>)]
        ///     調整する要素数
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="length"/> が指定範囲外の場合。
        /// </exception>
        public void AdjustLengthIfShort(int length);

        /// <summary>
        ///     要素数が超過している場合、要素数を指定の数に合わせる。
        /// </summary>
        /// <param name="length">
        ///     [Range(<see cref="GetMinCapacity"/>, <see cref="GetMaxCapacity"/>)]
        ///     調整する要素数
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="length"/> が指定範囲外の場合。
        /// </exception>
        public void AdjustLengthIfLong(int length);

        /// <summary>
        ///     自身を書き込み可能型にキャストする。
        /// </summary>
        /// <returns><typeparamref name="TWritable"/> にキャストした自分自身</returns>
        public TWritable AsWritableList();
    }
}
