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

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     サイズ変更可能であることを表すインタフェース。
    /// </summary>
    /// <typeparam name="TIn">リスト要素入力型</typeparam>
    /// <typeparam name="TOut">リスト要素出力型</typeparam>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface ISizeChangeableList<in TIn, out TOut> :
        IEnumerable<TOut>,
        IListProperty
        where TOut : TIn
    {
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
        public void Add(TIn item);

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
        public void AddRange(IEnumerable<TIn> items);

        /// <summary>
        ///     指定したインデックスの位置に要素を挿入する。
        /// </summary>
        /// <param name="index">[Range(0, <see cref="ICollection{T}.Count"/>)] インデックス</param>
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
        public void Insert(int index, TIn item);

        /// <summary>
        ///     指定したインデックスの位置に要素を挿入する。
        /// </summary>
        /// <param name="index">[Range(0, <see cref="ICollection{T}.Count"/>)] インデックス</param>
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
        public void InsertRange(int index, IEnumerable<TIn> items);

        /// <summary>
        ///     指定したインデックスを起点として、要素の上書き/追加を行う。
        /// </summary>
        /// <param name="index">[Range(0, <see cref="ICollection{T}.Count"/>)] インデックス</param>
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
        public void Overwrite(int index, IEnumerable<TIn> items);

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
        public bool Remove(TIn? item);

        /// <summary>
        ///     指定したインデックスの要素を削除する。
        /// </summary>
        /// <param name="index">[Range(0, <see cref="ICollection{T}.Count"/> - 1)] インデックス</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="index"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     操作によって要素数が <see cref="GetMinCapacity"/> を下回る場合。
        /// </exception>
        public void RemoveAt(int index);

        /// <summary>
        ///     要素の範囲を削除する。
        /// </summary>
        /// <param name="index">[Range(0, <see cref="ICollection{T}.Count"/> - 1)] インデックス</param>
        /// <param name="count">[Range(0, <see cref="ICollection{T}.Count"/>)] 削除する要素数</param>
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
        ///     要素を与えられた内容で一新する。
        /// </summary>
        /// <param name="initItems">リストに詰め直す要素</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="initItems"/> が <see langword="null"/> の場合、
        ///     または <paramref name="initItems"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="initItems"/> の要素数が
        ///     <see cref="IReadOnlyCollection{T}.Count"/> と一致しない場合。
        /// </exception>
        public void Reset(IEnumerable<TIn> initItems);

        /// <summary>
        ///     自身を初期化する。
        /// </summary>
        public void Clear();
    }

    /// <summary>
    ///     サイズ変更可能であることを表すインタフェース。
    ///     書き込み可能でもある。
    /// </summary>
    /// <typeparam name="TItem">リスト要素型</typeparam>
    /// <typeparam name="TImpl">実装型</typeparam>
    /// <typeparam name="TWritable"><see cref="IWritableList{TItem,TImpl,TReadable}"/>実装型</typeparam>
    /// <typeparam name="TReadable"><see cref="IReadableList{TItem,TImpl}"/>実装型</typeparam>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete]
    public interface ISizeChangeableList<TItem, TImpl, out TWritable, out TReadable> :
        IList<TItem>, INotifiableCollectionChange,
        IEqualityComparable<TImpl>,
        IDeepCloneableList<TImpl, TItem>
        where TImpl : ISizeChangeableList<TItem, TImpl, TWritable, TReadable>
    {
        /// <summary>
        ///     インデクサによるアクセス
        /// </summary>
        /// <param name="index">[Range(0, <see cref="ICollection{T}.Count"/> - 1)] インデックス</param>
        /// <returns>指定したインデックスの要素</returns>
        /// <exception cref="ArgumentNullException"><see lanword="null"/> をセットしようとした場合。</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/>が指定範囲外の場合。</exception>
        public new TItem this[int index] { get; set; }

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

        /// <summary>
        ///     指定範囲の要素を簡易コピーしたリストを取得する。
        /// </summary>
        /// <param name="index">[Range(0, <see cref="ICollection{T}.Count"/> - 1)] インデックス</param>
        /// <param name="count">[Range(0, <see cref="ICollection{T}.Count"/>)] 要素数</param>
        /// <returns>指定範囲の要素簡易コピーリスト</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="index"/>, <paramref name="count"/>が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">有効な範囲外の要素を取得しようとした場合</exception>
        public IEnumerable<TItem> GetRange(int index, int count);

        /// <summary>
        ///     指定の要素が含まれているか判断する。
        /// </summary>
        /// <param name="item">対象要素</param>
        /// <param name="itemComparer">子要素比較処理</param>
        /// <returns>指定の要素が含まれる場合は <see langword="true"/>、含まれない場合は <see langword="false"/>。</returns>
        public bool Contains(TItem? item, IEqualityComparer<TItem>? itemComparer);

        /// <summary>
        ///     指定したオブジェクトを検索し、最初に出現する位置のインデックスを返す。
        /// </summary>
        /// <param name="item">対象要素</param>
        /// <param name="itemComparer">子要素比較処理</param>
        /// <returns>要素が含まれていない場合、-1</returns>
        public int IndexOf(TItem? item, IEqualityComparer<TItem>? itemComparer);

        /// <summary>
        ///     リストの連続した要素を更新する。
        /// </summary>
        /// <param name="index">[Range(0, <see cref="IReadableList{TItem, TImpl}.Count"/> - 1)] 更新開始インデックス</param>
        /// <param name="items">更新要素</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="items"/> が <see langword="null"/> の場合、
        ///     または <paramref name="items"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/>が指定範囲外の場合。</exception>
        /// <exception cref="ArgumentException">
        ///     有効な範囲外の要素を編集しようとした場合。
        /// </exception>
        public void SetRange(int index, IEnumerable<TItem> items);

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
        /// <param name="index">[Range(0, <see cref="ICollection{T}.Count"/>)] インデックス</param>
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
        /// <param name="index">[Range(0, <see cref="ICollection{T}.Count"/>)] インデックス</param>
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
        /// <param name="index">[Range(0, <see cref="ICollection{T}.Count"/>)] インデックス</param>
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
        ///     指定したインデックスにある項目をコレクション内の新しい場所へ移動する。
        /// </summary>
        /// <param name="oldIndex">[Range(0, <see cref="IReadableList{TItem, TImpl}.Count"/> - 1)] 移動する項目のインデックス</param>
        /// <param name="newIndex">[Range(0, <see cref="IReadableList{TItem, TImpl}.Count"/> - 1)] 移動先のインデックス</param>
        /// <exception cref="InvalidOperationException">
        ///     自身の要素数が0の場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="oldIndex"/>, <paramref name="newIndex"/> が指定範囲外の場合。
        /// </exception>
        public void Move(int oldIndex, int newIndex);

        /// <summary>
        ///     指定したインデックスから始まる連続した項目をコレクション内の新しい場所へ移動する。
        /// </summary>
        /// <param name="oldIndex">
        ///     [Range(0, <see cref="IReadableList{TItem, TImpl}.Count"/> - 1)]
        ///     移動する項目のインデックス開始位置
        /// </param>
        /// <param name="newIndex">
        ///     [Range(0, <see cref="IReadableList{TItem, TImpl}.Count"/> - 1)]
        ///     移動先のインデックス開始位置
        /// </param>
        /// <param name="count">
        ///     [Range(0, <see cref="IReadableList{TItem, TImpl}.Count"/>)]
        ///     移動させる要素数
        /// </param>
        /// <exception cref="InvalidOperationException">
        ///     自身の要素数が0の場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="oldIndex"/>, <paramref name="newIndex"/>, <paramref name="count"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">有効な範囲外の要素を移動しようとした場合。</exception>
        public void MoveRange(int oldIndex, int newIndex, int count);

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
        public new bool Remove(TItem? item);

        /// <summary>
        ///     指定したインデックスの要素を削除する。
        /// </summary>
        /// <param name="index">[Range(0, <see cref="ICollection{T}.Count"/> - 1)] インデックス</param>
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
        /// <param name="index">[Range(0, <see cref="ICollection{T}.Count"/> - 1)] インデックス</param>
        /// <param name="count">[Range(0, <see cref="ICollection{T}.Count"/>)] 削除する要素数</param>
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
        ///     すべての要素を初期化する。
        /// </summary>
        /// <remarks>
        ///     既存の要素はすべて除去され、<see cref="IReadableList{TItem, TImpl}.Count"/> 個の
        ///     新たなデフォルト要素が編集される。
        /// </remarks>
        public void Reset();

        /// <summary>
        ///     要素を与えられた内容で一新する。
        /// </summary>
        /// <param name="initItems">リストに詰め直す要素</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="initItems"/> が <see langword="null"/> の場合、
        ///     または <paramref name="initItems"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="initItems"/> の要素数が
        ///     <see cref="IReadableList{TItem, TImpl}.Count"/> と一致しない場合。
        /// </exception>
        public void Reset(IEnumerable<TItem> initItems);

        /// <summary>
        ///     自身を書き込み可能型にキャストする。
        /// </summary>
        /// <returns><typeparamref name="TWritable"/> にキャストした自分自身</returns>
        public TWritable AsWritableList();

        /// <summary>
        ///     自身を読み取り可能型にキャストする。
        /// </summary>
        /// <returns><typeparamref name="TReadable"/> にキャストした自分自身</returns>
        public TReadable AsReadableList();

        /// <summary>
        ///     現在のオブジェクトが、別のオブジェクトと同値であるかどうかを示す。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>
        ///     同値 または 同一 である場合 <see langword="true"/>。
        /// </returns>
        public bool ItemEquals(IEnumerable<TItem>? other);

        /// <summary>
        ///     現在のオブジェクトが、別のオブジェクトと同値であるかどうかを示す。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <param name="itemComparer">子要素比較処理</param>
        /// <returns>
        ///     同値 または 同一 である場合 <see langword="true"/>。
        /// </returns>
        public bool ItemEquals(IEnumerable<TItem>? other, IEqualityComparer<TItem>? itemComparer);
    }
}
