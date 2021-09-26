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
    ///     書込み可能なリストであることを表すインタフェース。
    ///     サイズ変更はできない。
    /// </summary>
    /// <typeparam name="TItem">リスト要素型</typeparam>
    /// <typeparam name="TImpl">実装型</typeparam>
    /// <typeparam name="TReadable"><see cref="IReadableList{TItem,TImpl}"/>実装型</typeparam>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IWritableList<TItem, TImpl, out TReadable> :
        IReadOnlyList<TItem>, INotifiableCollectionChange,
        IEqualityComparable<TImpl>,
        IDeepCloneableList<TImpl, TItem>
        where TImpl : IWritableList<TItem, TImpl, TReadable>
    {
        /// <summary>
        ///     インデクサによるアクセス
        /// </summary>
        /// <param name="index">[Range(0, <see cref="IReadOnlyCollection{T}.Count"/> - 1)] インデックス</param>
        /// <returns>指定したインデックスの要素</returns>
        /// <exception cref="ArgumentNullException"><see lanword="null"/> をセットしようとした場合。</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/>が指定範囲外の場合。</exception>
        public new TItem this[int index] { get; set; }

        /// <summary>
        ///     指定範囲の要素を簡易コピーしたリストを取得する。
        /// </summary>
        /// <param name="index">[Range(0, <see cref="IReadOnlyList{T}.Count"/> - 1)] インデックス</param>
        /// <param name="count">[Range(0, <see cref="IReadOnlyList{T}.Count"/>)] 要素数</param>
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
        /// <returns>指定の要素が含まれる場合は <see langword="true"/>、含まれない場合は <see langword="false"/>。</returns>
        public bool Contains(TItem? item);

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
        /// <returns>要素が含まれていない場合、-1</returns>
        public int IndexOf(TItem? item);

        /// <summary>
        ///     指定したオブジェクトを検索し、最初に出現する位置のインデックスを返す。
        /// </summary>
        /// <param name="item">対象要素</param>
        /// <param name="itemComparer">子要素比較処理</param>
        /// <returns>要素が含まれていない場合、-1</returns>
        public int IndexOf(TItem? item, IEqualityComparer<TItem>? itemComparer);

        /// <summary>
        ///     すべての要素を、指定された配列のインデックスから始まる部分にコピーする。
        /// </summary>
        /// <param name="array">コピー先の配列</param>
        /// <param name="index">[Range(0, <see cref="IReadOnlyCollection{T}.Count"/> - 1)] コピー開始インデックス</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="array"/> が <see langword="null"/> の場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="index"/> が 0 未満の場合。
        /// </exception>
        /// <exception cref="ArgumentException">コピー先の領域が不足する場合</exception>
        public void CopyTo(TItem[] array, int index);

        /// <summary>
        ///     リストの連続した要素を更新する。
        /// </summary>
        /// <param name="index">[Range(0, <see cref="IReadOnlyCollection{T}.Count"/> - 1)] 更新開始インデックス</param>
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
        ///     指定したインデックスにある項目をコレクション内の新しい場所へ移動する。
        /// </summary>
        /// <param name="oldIndex">[Range(0, <see cref="IReadOnlyCollection{T}.Count"/> - 1)] 移動する項目のインデックス</param>
        /// <param name="newIndex">[Range(0, <see cref="IReadOnlyCollection{T}.Count"/> - 1)] 移動先のインデックス</param>
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
        ///     [Range(0, <see cref="IReadOnlyCollection{T}.Count"/> - 1)]
        ///     移動する項目のインデックス開始位置
        /// </param>
        /// <param name="newIndex">
        ///     [Range(0, <see cref="IReadOnlyCollection{T}.Count"/> - 1)]
        ///     移動先のインデックス開始位置
        /// </param>
        /// <param name="count">
        ///     [Range(0, <see cref="IReadOnlyCollection{T}.Count"/>)]
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
        ///     すべての要素を初期化する。
        /// </summary>
        /// <remarks>
        ///     既存の要素はすべて除去され、<see cref="IReadOnlyCollection{T}.Count"/> 個の
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
        ///     <see cref="IReadOnlyCollection{T}.Count"/> と一致しない場合。
        /// </exception>
        public void Reset(IEnumerable<TItem> initItems);

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
