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
    /// <typeparam name="TIn">リスト要素入力型</typeparam>
    /// <typeparam name="TOut">リスト要素出力型</typeparam>
    internal interface IWritableTwoDimensionalList<in TIn, out TOut> :
        ITwoDimensionalListProperty,
        IEnumerable<IEnumerable<TOut>>
        where TOut : TIn
    {
        /// <summary>
        ///     インデクサによるアクセス
        /// </summary>
        /// <param name="rowIndex">[Range(0, <see cref="ITwoDimensionalListProperty.RowCount"/> - 1)] 行インデックス</param>
        /// <param name="columnIndex">[Range(0, <see cref="ITwoDimensionalListProperty.ColumnCount"/> - 1)] 列インデックス</param>
        /// <returns>指定したインデックスの要素</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="rowIndex"/>が指定範囲外の場合。</exception>
        public TIn this[int rowIndex, int columnIndex] { set; }

        /// <summary>
        ///     リストの連続した行要素を更新する。
        /// </summary>
        /// <param name="rowIndex">[Range(0, <see cref="ITwoDimensionalListProperty.RowCount"/> - 1)] 更新開始行インデックス</param>
        /// <param name="items">更新要素</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="items"/> が <see langword="null"/> の場合、
        ///     または <paramref name="items"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="rowIndex"/>が指定範囲外の場合。</exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="items"/> のいずれかの要素要素について 要素数が
        ///     <see cref="ITwoDimensionalListProperty.ColumnCount"/> と一致しない場合、
        ///     または 有効な範囲外の要素を編集しようとした場合。
        /// </exception>
        public void SetRow(int rowIndex, params IEnumerable<TIn>[] items);

        /// <summary>
        ///     リストの連続した列要素を更新する。
        /// </summary>
        /// <param name="columnIndex">
        ///     [Range(0, <see cref="ITwoDimensionalListProperty.ColumnCount"/> - 1)]
        ///     更新開始列インデックス
        /// </param>
        /// <param name="items">更新要素</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="items"/> が <see langword="null"/> の場合、
        ///     または <paramref name="items"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="columnIndex"/>が指定範囲外の場合。</exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="items"/> のいずれかの要素要素について 要素数が
        ///     <see cref="ITwoDimensionalListProperty.RowCount"/> と一致しない場合、
        ///     または 有効な範囲外の要素を編集しようとした場合。
        /// </exception>
        public void SetColumn(int columnIndex, params IEnumerable<TIn>[] items);

        /// <summary>
        ///     指定した行番号から始まる連続した項目をコレクション内の新しい場所へ移動する。
        /// </summary>
        /// <param name="oldRowIndex">
        ///     [Range(0, <see cref="ITwoDimensionalListProperty.RowCount"/> - 1)]
        ///     移動する項目の行番号開始位置
        /// </param>
        /// <param name="newRowIndex">
        ///     [Range(0, <see cref="ITwoDimensionalListProperty.RowCount"/> - 1)]
        ///     移動先の行番号開始位置
        /// </param>
        /// <param name="count">
        ///     [Range(0, <see cref="ITwoDimensionalListProperty.RowCount"/>)]
        ///     移動させる要素数
        /// </param>
        /// <exception cref="InvalidOperationException">
        ///     自身の要素数が0の場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="oldRowIndex"/>, <paramref name="newRowIndex"/>, <paramref name="count"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">有効な範囲外の要素を移動しようとした場合。</exception>
        public void MoveRow(int oldRowIndex, int newRowIndex, int count = 1);

        /// <summary>
        ///     指定した列番号から始まる連続した項目をコレクション内の新しい場所へ移動する。
        /// </summary>
        /// <param name="oldColumnIndex">
        ///     [Range(0, <see cref="ITwoDimensionalListProperty.ColumnCount"/> - 1)]
        ///     移動する項目の列番号開始位置
        /// </param>
        /// <param name="newColumnIndex">
        ///     [Range(0, <see cref="ITwoDimensionalListProperty.ColumnCount"/> - 1)]
        ///     移動先の列番号開始位置
        /// </param>
        /// <param name="count">
        ///     [Range(0, <see cref="ITwoDimensionalListProperty.ColumnCount"/>)]
        ///     移動させる要素数
        /// </param>
        /// <exception cref="InvalidOperationException">
        ///     自身の要素数が0の場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="oldColumnIndex"/>, <paramref name="newColumnIndex"/>, <paramref name="count"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">有効な範囲外の要素を移動しようとした場合。</exception>
        public void MoveColumn(int oldColumnIndex, int newColumnIndex, int count = 1);

        /// <summary>
        ///     すべての要素を初期化する。
        /// </summary>
        /// <remarks>
        ///     既存の要素はすべて除去され、
        ///     <see cref="ITwoDimensionalListProperty.RowCount"/> 行 *
        ///     <see cref="ITwoDimensionalListProperty.ColumnCount"/> 列 の
        ///     新たなデフォルト要素が編集される。
        /// </remarks>
        public void Reset();
    }
}
