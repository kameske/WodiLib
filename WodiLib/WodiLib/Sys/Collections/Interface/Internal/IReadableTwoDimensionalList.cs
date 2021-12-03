// ========================================
// Project Name : WodiLib
// File Name    : IReadableTwoDimensionalList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Linq;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     読み取り可能な二次元リストであることを表すインタフェース
    /// </summary>
    /// <typeparam name="TOutRow">リスト行データ出力型</typeparam>
    /// <typeparam name="TOutItem">リスト要素出力型</typeparam>
    internal interface IReadableTwoDimensionalList<out TOutRow, out TOutItem> :
        ITwoDimensionalListProperty,
        IEnumerable<TOutRow>
        where TOutRow : IEnumerable<TOutItem>
    {
        /// <summary>
        ///     インデクサによるアクセス
        /// </summary>
        /// <param name="rowIndex">[Range(0, <see cref="ITwoDimensionalListProperty.RowCount"/> - 1)] 行インデックス</param>
        /// <param name="columnIndex">[Range(0, <see cref="ITwoDimensionalListProperty.ColumnCount"/> - 1)] 列インデックス</param>
        /// <returns>指定したインデックスの要素</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="rowIndex"/>が指定範囲外の場合。</exception>
        public TOutItem this[int rowIndex, int columnIndex] { get; }

        /// <summary>
        ///     指定範囲の要素を簡易コピーしたリストを取得する。
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         取得結果の外側シーケンスは行要素を、内側シーケンスは列要素を示す。
        ///     </para>
        /// </remarks>
        /// <param name="rowIndex">[Range(0, <see cref="ITwoDimensionalListProperty.RowCount"/> - 1)] 行インデックス</param>
        /// <param name="rowCount">[Range(0, <see cref="ITwoDimensionalListProperty.RowCount"/>)] 行数</param>
        /// <returns>指定行範囲の要素簡易コピーリスト</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="rowIndex"/>, <paramref name="rowCount"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">有効な範囲外の要素を取得しようとした場合</exception>
        public IEnumerable<TOutRow> GetRow(int rowIndex, int rowCount);

        /// <summary>
        ///     指定範囲の要素を簡易コピーしたリストを取得する。
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         取得結果の外側シーケンスは列要素を、内側シーケンスは行要素を示す。
        ///     </para>
        /// </remarks>
        /// <param name="columnIndex">[Range(0, <see cref="ITwoDimensionalListProperty.ColumnCount"/> - 1] 列インデックス</param>
        /// <param name="columnCount">[Range(0, <see cref="ITwoDimensionalListProperty.ColumnCount"/>)] 列数</param>
        /// <returns>指定列範囲の要素簡易コピーリスト</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="columnIndex"/>, <paramref name="columnCount"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">有効な範囲外の要素を取得しようとした場合</exception>
        public IEnumerable<IEnumerable<TOutItem>> GetColumn(int columnIndex, int columnCount);

        /// <summary>
        ///     指定範囲の要素を簡易コピーしたリストを取得する。
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         取得結果の外側シーケンスは列要素を、内側シーケンスは行要素を示す。
        ///     </para>
        /// </remarks>
        /// <param name="rowIndex">[Range(0, <see cref="ITwoDimensionalListProperty.RowCount"/> - 1)] 行インデックス</param>
        /// <param name="rowCount">[Range(0, <see cref="ITwoDimensionalListProperty.RowCount"/>)] 行数</param>
        /// <param name="columnIndex">[Range(0, <see cref="ITwoDimensionalListProperty.ColumnCount"/> - 1] 列インデックス</param>
        /// <param name="columnCount">[Range(0, <see cref="ITwoDimensionalListProperty.ColumnCount"/>)] 列数</param>
        /// <returns>指定範囲の要素簡易コピーリスト</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="columnIndex"/>, <paramref name="columnCount"/>,
        ///     <paramref name="rowIndex"/>, <paramref name="rowCount"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">有効な範囲外の要素を取得しようとした場合</exception>
        public IEnumerable<IEnumerable<TOutItem>> GetItem(int rowIndex, int rowCount, int columnIndex, int columnCount);
    }

    internal static class ReadableTwoDimensionalListExtension
    {
        /// <summary>
        ///     指定行のデータを取得する。
        /// </summary>
        /// <param name="src">対象二次元リスト</param>
        /// <param name="rowIndex">[Range(0, <see cref="ITwoDimensionalListProperty.RowCount"/> - 1)] 行インデックス</param>
        /// <returns>指定行範囲の要素簡易コピーリスト</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="src"/> が <see langword="null"/> の場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="rowIndex"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">有効な範囲外の要素を取得しようとした場合</exception>
        public static TOutRow GetRow<TOutRow, TOutItem>(
            this IReadableTwoDimensionalList<TOutRow, TOutItem> src, int rowIndex)
            where TOutRow : IEnumerable<TOutItem>
        {
            ThrowHelper.ValidateArgumentNotNull(src is null, nameof(src));

            return src.GetRow(rowIndex, 1).First();
        }

        /// <summary>
        ///     指定列の要素を簡易コピーしたリストを取得する。
        /// </summary>
        /// <param name="src">対象二次元リスト</param>
        /// <param name="columnIndex">[Range(0, <see cref="ITwoDimensionalListProperty.ColumnCount"/> - 1] 列インデックス</param>
        /// <returns>指定列範囲の要素簡易コピーリスト</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="src"/> が <see langword="null"/> の場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="columnIndex"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">有効な範囲外の要素を取得しようとした場合</exception>
        public static IEnumerable<TOutItem> GetColumn<TOutRow, TOutItem>(
            this IReadableTwoDimensionalList<TOutRow, TOutItem> src,
            int columnIndex
        )
            where TOutRow : IEnumerable<TOutItem>
        {
            ThrowHelper.ValidateArgumentNotNull(src is null, nameof(src));

            return src.GetColumn(columnIndex, 1).First();
        }
    }
}
