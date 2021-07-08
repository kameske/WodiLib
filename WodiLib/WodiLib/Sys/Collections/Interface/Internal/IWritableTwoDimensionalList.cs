// ========================================
// Project Name : WodiLib
// File Name    : IWritableTwoDimensionalList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     書込み可能な二次元リストであることを表すインタフェース。
    ///     サイズ変更はできない。
    /// </summary>
    /// <typeparam name="TItem">リスト要素型</typeparam>
    /// <typeparam name="TImpl">実装型</typeparam>
    /// <typeparam name="TReadable"><see cref="IReadableTwoDimensionalList{TItem,TImpl}"/>実装型</typeparam>
    internal interface
        IWritableTwoDimensionalList<TItem, out TImpl, out TReadable> : IReadableTwoDimensionalList<TItem, TReadable>
        where TImpl : IWritableTwoDimensionalList<TItem, TImpl, TReadable>,
        IReadableTwoDimensionalList<TItem, TReadable>
        where TReadable : IReadableTwoDimensionalList<TItem, TReadable>
    {
        /// <summary>
        ///     インデクサによるアクセス
        /// </summary>
        /// <param name="row">[Range(0, <see cref="IReadableTwoDimensionalList{TItem,TImpl}.RowCount"/> - 1)] 行インデックス</param>
        /// <returns>指定したインデックスの要素</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="row"/>が指定範囲外の場合。</exception>
        /// <exception cref="ArgumentNullException"><see lanword="null"/> をセットしようとした場合。</exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="value"/> の要素数が
        ///     <see cref="IReadableTwoDimensionalList{TItem, TImpl}.ColumnCount"/> と一致しない場合。
        /// </exception>
        public new IEnumerable<TItem> this[int row] { get; set; }

        /// <summary>
        ///     インデクサによるアクセス
        /// </summary>
        /// <param name="row">[Range(0, <see cref="IReadableTwoDimensionalList{TItem,TImpl}.RowCount"/> - 1)] 行インデックス</param>
        /// <param name="column">[Range(0, <see cref="IReadableTwoDimensionalList{TItem,TImpl}.ColumnCount"/> - 1)] 列インデックス</param>
        /// <returns>指定したインデックスの要素</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="row"/>が指定範囲外の場合。</exception>
        /// <exception cref="ArgumentNullException"><see lanword="null"/> をセットしようとした場合。</exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="value"/> の要素数が
        ///     <see cref="IReadableTwoDimensionalList{TItem, TImpl}.ColumnCount"/> と一致しない場合。
        /// </exception>
        public new TItem this[int row, int column] { get; set; }

        /// <summary>
        ///     リストの連続した行要素を更新する。
        /// </summary>
        /// <param name="row">[Range(0, <see cref="IReadableTwoDimensionalList{TItem, TImpl}.RowCount"/> - 1)] 更新開始行インデックス</param>
        /// <param name="items">更新要素</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="items"/> が <see langword="null"/> の場合、
        ///     または <paramref name="items"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="row"/>が指定範囲外の場合。</exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="items"/> のいずれかの要素要素について 要素数が
        ///     <see cref="IReadableTwoDimensionalList{TItem, TImpl}.ColumnCount"/> と一致しない場合、
        ///     または 有効な範囲外の要素を編集しようとした場合。
        /// </exception>
        public void SetRowRange(int row, IEnumerable<IEnumerable<TItem>> items);

        /// <summary>
        ///     リストの列要素を更新する。
        /// </summary>
        /// <param name="column">[Range(0, <see cref="IReadableTwoDimensionalList{TItem, TImpl}.ColumnCount"/> - 1)] 更新列インデックス</param>
        /// <param name="items">更新要素</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="items"/> が <see langword="null"/> の場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="column"/>が指定範囲外の場合。</exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="items"/> の要素数が
        ///     <see cref="IReadableTwoDimensionalList{TItem, TImpl}.RowCount"/> と一致しない場合。
        /// </exception>
        public void SetColumn(int column, IEnumerable<TItem> items);

        /// <summary>
        ///     リストの連続した列要素を更新する。
        /// </summary>
        /// <param name="column">[Range(0, <see cref="IReadableTwoDimensionalList{TItem, TImpl}.ColumnCount"/> - 1)] 更新開始列インデックス</param>
        /// <param name="items">更新要素</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="items"/> が <see langword="null"/> の場合、
        ///     または <paramref name="items"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="column"/>が指定範囲外の場合。</exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="items"/> のいずれかの要素要素について 要素数が
        ///     <see cref="IReadableTwoDimensionalList{TItem, TImpl}.RowCount"/> と一致しない場合、
        ///     または 有効な範囲外の要素を編集しようとした場合。
        /// </exception>
        public void SetColumnRange(int column, IEnumerable<IEnumerable<TItem>> items);

        /// <summary>
        ///     指定した行番号にある項目をコレクション内の新しい場所へ移動する。
        /// </summary>
        /// <param name="oldRow">[Range(0, <see cref="IReadableTwoDimensionalList{TItem,TImpl}.RowCount"/> - 1)] 移動する項目の行番号</param>
        /// <param name="newRow">[Range(0, <see cref="IReadableTwoDimensionalList{TItem,TImpl}.RowCount"/> - 1)] 移動先の行番号</param>
        /// <exception cref="InvalidOperationException">
        ///     自身の要素数が0の場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="oldRow"/>, <paramref name="newRow"/> が指定範囲外の場合。
        /// </exception>
        public void MoveRow(int oldRow, int newRow);

        /// <summary>
        ///     指定した行番号から始まる連続した項目をコレクション内の新しい場所へ移動する。
        /// </summary>
        /// <param name="oldRow">
        ///     [Range(0, <see cref="IReadableTwoDimensionalList{TItem,TImpl}.RowCount"/> - 1)]
        ///     移動する項目の行番号開始位置
        /// </param>
        /// <param name="newRow">
        ///     [Range(0, <see cref="IReadableTwoDimensionalList{TItem,TImpl}.RowCount"/> - 1)]
        ///     移動先の行番号開始位置
        /// </param>
        /// <param name="count">
        ///     [Range(0, <see cref="IReadableTwoDimensionalList{TItem,TImpl}.RowCount"/>)]
        ///     移動させる要素数
        /// </param>
        /// <exception cref="InvalidOperationException">
        ///     自身の要素数が0の場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="oldRow"/>, <paramref name="newRow"/>, <paramref name="count"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">有効な範囲外の要素を移動しようとした場合。</exception>
        public void MoveRowRange(int oldRow, int newRow, int count);

        /// <summary>
        ///     指定した列番号にある項目をコレクション内の新しい場所へ移動する。
        /// </summary>
        /// <param name="oldColumn">[Range(0, <see cref="IReadableTwoDimensionalList{TItem,TImpl}.ColumnCount"/> - 1)] 移動する項目の列番号</param>
        /// <param name="newColumn">[Range(0, <see cref="IReadableTwoDimensionalList{TItem,TImpl}.ColumnCount"/> - 1)] 移動先の列番号</param>
        /// <exception cref="InvalidOperationException">
        ///     自身の要素数が0の場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="oldColumn"/>, <paramref name="newColumn"/> が指定範囲外の場合。
        /// </exception>
        public void MoveColumn(int oldColumn, int newColumn);

        /// <summary>
        ///     指定した列番号から始まる連続した項目をコレクション内の新しい場所へ移動する。
        /// </summary>
        /// <param name="oldColumn">
        ///     [Range(0, <see cref="IReadableTwoDimensionalList{TItem,TImpl}.ColumnCount"/> - 1)]
        ///     移動する項目の列番号開始位置
        /// </param>
        /// <param name="newColumn">
        ///     [Range(0, <see cref="IReadableTwoDimensionalList{TItem,TImpl}.ColumnCount"/> - 1)]
        ///     移動先の列番号開始位置
        /// </param>
        /// <param name="count">
        ///     [Range(0, <see cref="IReadableTwoDimensionalList{TItem,TImpl}.ColumnCount"/>)]
        ///     移動させる要素数
        /// </param>
        /// <exception cref="InvalidOperationException">
        ///     自身の要素数が0の場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="oldColumn"/>, <paramref name="newColumn"/>, <paramref name="count"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">有効な範囲外の要素を移動しようとした場合。</exception>
        public void MoveColumnRange(int oldColumn, int newColumn, int count);

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
        public void Reset(IEnumerable<IEnumerable<TItem>> initItems);

        /// <summary>
        ///     自身を読み取り可能型にキャストする。
        /// </summary>
        /// <returns><typeparamref name="TReadable"/> にキャストした自分自身</returns>
        public TReadable AsReadableList();
    }
}
