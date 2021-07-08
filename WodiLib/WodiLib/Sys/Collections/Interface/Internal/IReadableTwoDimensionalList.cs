// ========================================
// Project Name : WodiLib
// File Name    : IReadableTwoDimensionalList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     読み取り可能な二次元リストであることを表すインタフェース
    /// </summary>
    /// <typeparam name="TItem">リスト要素型</typeparam>
    /// <typeparam name="TImpl">実装型</typeparam>
    internal interface IReadableTwoDimensionalList<TItem, out TImpl> : IEnumerable<IEnumerable<TItem>>,
        INotifiableTwoDimensionalListChangeInternal<TItem>, IDeepCloneableTwoDimensionalListInternal<TImpl, TItem>
        where TImpl : IReadableTwoDimensionalList<TItem, TImpl>, IEnumerable<IEnumerable<TItem>>
    {
        /// <summary>
        ///     インデクサによるアクセス
        /// </summary>
        /// <param name="row">[Range(0, <see cref="RowCount"/> - 1)] 行インデックス</param>
        /// <returns>指定したインデックスの要素</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="row"/>が指定範囲外の場合。</exception>
        public IEnumerable<TItem> this[int row] { get; }

        /// <summary>
        ///     インデクサによるアクセス
        /// </summary>
        /// <param name="row">[Range(0, <see cref="RowCount"/> - 1)] 行インデックス</param>
        /// <param name="column">[Range(0, <see cref="ColumnCount"/> - 1)] 列インデックス</param>
        /// <returns>指定したインデックスの要素</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="row"/>が指定範囲外の場合。</exception>
        public TItem this[int row, int column] { get; }

        /// <summary>
        ///     空フラグ
        /// </summary>
        /// <remarks>
        ///     <see cref="RowCount"/> == 0 かつ <see cref="ColumnCount"/> == 0 の場合に <see langword="true"/> を、
        ///     それ以外の場合に <see langword="false"/> を返す。
        /// </remarks>
        public bool IsEmpty { get; }

        /// <summary>行数</summary>
        public int RowCount { get; }

        /// <summary>列数</summary>
        public int ColumnCount { get; }

        /// <summary>
        ///     総数
        /// </summary>
        /// <remarks>
        ///     <see cref="RowCount"/> * <see cref="ColumnCount"/> を返す。
        /// </remarks>
        public int AllCount { get; }

        /// <summary>
        ///     指定の要素が含まれているか判断する。
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         <paramref name="comparer"/> が <see langword="null"/> の場合、
        ///         要素の等価判定は <typeparamref name="TItem"/>.<see cref="object.Equals(object)"/> メソッドで行う。
        ///         <paramref name="comparer"/> が <see langword="null"/> ではない場合、
        ///         要素の等価判定は <see cref="IEqualityComparer{T}"/>.<see cref="IEqualityComparer{T}.Equals(T,T)"/> メソッドで行う。
        ///     </para>
        ///     <para>
        ///         要素の走査は [0, 0] の要素から開始する。<br/>
        ///         走査方向は <paramref name="shouldScanRowDirection"/> によって変化する。<br/>
        ///         <paramref name="shouldScanRowDirection"/> が <see langword="false"/> の場合、行方向に走査する。<br/>
        ///         <paramref name="shouldScanRowDirection"/> が <see langword="true"/> の場合、列方向に走査する。<br/>
        ///     </para>
        /// </remarks>
        /// <param name="item">対象要素</param>
        /// <param name="comparer">対象要素</param>
        /// <param name="shouldScanRowDirection">要素走査方向 = 列フラグ</param>
        /// <returns>指定の要素が含まれる場合は <see langword="true"/>、含まれない場合は <see langword="false"/>。</returns>
        public bool Contains([AllowNull] TItem item, IEqualityComparer<TItem>? comparer = null,
            bool shouldScanRowDirection = false);

        /// <summary>
        ///     指定の要素が含まれているか判断する。
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         <paramref name="comparer"/> が <see langword="null"/> の場合、
        ///         要素の等価判定は <typeparamref name="TItem"/>.<see cref="object.Equals(object)"/> メソッドで行う。
        ///         <paramref name="comparer"/> が <see langword="null"/> ではない場合、
        ///         要素の等価判定は <see cref="IEqualityComparer{T}"/>.<see cref="IEqualityComparer{T}.Equals(T,T)"/> メソッドで行う。
        ///     </para>
        ///     <para>
        ///         比較は行単位で行う。
        ///     </para>
        /// </remarks>
        /// <param name="item">対象要素</param>
        /// <param name="comparer">等価比較メソッド実装</param>
        /// <returns>指定の要素と一致する行が含まれる場合は <see langword="true"/>、含まれない場合は <see langword="false"/>。</returns>
        public bool ContainsRow(IEnumerable<TItem>? item, IEqualityComparer<TItem>? comparer = null);

        /// <summary>
        ///     指定の要素が含まれているか判断する。
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         <paramref name="comparer"/> が <see langword="null"/> の場合、
        ///         要素の等価判定は <typeparamref name="TItem"/>.<see cref="object.Equals(object)"/> メソッドで行う。
        ///         <paramref name="comparer"/> が <see langword="null"/> ではない場合、
        ///         要素の等価判定は <see cref="IEqualityComparer{T}"/>.<see cref="IEqualityComparer{T}.Equals(T,T)"/> メソッドで行う。
        ///     </para>
        ///     <para>
        ///         比較は列単位で行う。
        ///     </para>
        /// </remarks>
        /// <param name="item">対象要素</param>
        /// <param name="comparer">等価比較メソッド実装</param>
        /// <returns>指定の要素と一致する列が含まれる場合は <see langword="true"/>、含まれない場合は <see langword="false"/>。</returns>
        public bool ContainsColumn(IEnumerable<TItem>? item, IEqualityComparer<TItem>? comparer = null);

        /// <summary>
        ///     指定範囲の行要素を簡易コピーしたリストを取得する。
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         取得結果の外側シーケンスは行要素を、内側シーケンスは列要素を示す。
        ///     </para>
        /// </remarks>
        /// <param name="row">[Range(0, <see cref="RowCount"/> - 1)] 行インデックス</param>
        /// <param name="count">[Range(0, <see cref="RowCount"/>)] 行数</param>
        /// <returns>指定行範囲の要素簡易コピーリスト</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="row"/>, <paramref name="count"/>が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">有効な範囲外の行要素を取得しようとした場合</exception>
        public IEnumerable<IEnumerable<TItem>> GetRowRange(int row, int count);

        /// <summary>
        ///     指定範囲の要素を簡易コピーしたリストを取得する。
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         取得結果の外側シーケンスは行要素を、内側シーケンスは列要素を示す。
        ///     </para>
        /// </remarks>
        /// <param name="row">[Range(0, <see cref="RowCount"/> - 1)] 行インデックス</param>
        /// <param name="rowCount">[Range(0, <see cref="RowCount"/>)] 行数</param>
        /// <param name="column">[Range(0, <see cref="ColumnCount"/> - 1] 列インデックス</param>
        /// <param name="columnCount">[Range(0, <see cref="ColumnCount"/>)] 列数</param>
        /// <returns>指定行範囲の要素簡易コピーリスト</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="row"/>, <paramref name="rowCount"/>,
        ///     <paramref name="column"/>, <paramref name="columnCount"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">有効な範囲外の要素を取得しようとした場合</exception>
        public IEnumerable<IEnumerable<TItem>> GetRowRange(int row, int rowCount, int column, int columnCount);

        /// <summary>
        ///     指定した列の要素を簡易コピーしたリストを取得する。
        /// </summary>
        /// <param name="column">[Range(0, <see cref="ColumnCount"/> - 1] 列インデックス</param>
        /// <returns>指定列の要素簡易コピーリスト</returns>
        /// <exception cref="ArgumentOutOfRangeException"><see cref="column"/> が 指定範囲外の場合</exception>
        public IEnumerable<TItem> GetColumn(int column);

        /// <summary>
        ///     指定範囲の列要素を簡易コピーしたリストを取得する。
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         取得結果の外側シーケンスは列要素を、内側シーケンスは行要素を示す。
        ///     </para>
        /// </remarks>
        /// <param name="column">[Range(0, <see cref="ColumnCount"/> - 1)] 列インデックス</param>
        /// <param name="count">[Range(0, <see cref="ColumnCount"/>)] 列数</param>
        /// <returns>指定列範囲の要素簡易コピーリスト</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="column"/>, <paramref name="count"/>が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">有効な範囲外の列要素を取得しようとした場合</exception>
        public IEnumerable<IEnumerable<TItem>> GetColumnRange(int column, int count);

        /// <summary>
        ///     指定範囲の要素を簡易コピーしたリストを取得する。
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         取得結果の外側シーケンスは列要素を、内側シーケンスは行要素を示す。
        ///     </para>
        /// </remarks>
        /// <param name="column">[Range(0, <see cref="ColumnCount"/> - 1] 列インデックス</param>
        /// <param name="columnCount">[Range(0, <see cref="ColumnCount"/>)] 列数</param>
        /// <param name="row">[Range(0, <see cref="RowCount"/> - 1)] 行インデックス</param>
        /// <param name="rowCount">[Range(0, <see cref="RowCount"/>)] 行数</param>
        /// <returns>指定範囲の要素簡易コピーリスト</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="column"/>, <paramref name="columnCount"/>,
        ///     <paramref name="row"/>, <paramref name="rowCount"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">有効な範囲外の要素を取得しようとした場合</exception>
        public IEnumerable<IEnumerable<TItem>> GetColumnRange(int column, int columnCount, int row, int rowCount);

        /// <summary>
        ///     指定したオブジェクトを検索し、最初に出現する位置のインデックスを返す。
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         <paramref name="comparer"/> が <see langword="null"/> の場合、
        ///         要素の等価判定は <typeparamref name="TItem"/>.<see cref="object.Equals(object)"/> メソッドで行う。
        ///         <paramref name="comparer"/> が <see langword="null"/> ではない場合、
        ///         要素の等価判定は <see cref="IEqualityComparer{T}"/>.<see cref="IEqualityComparer{T}.Equals(T,T)"/> メソッドで行う。
        ///     </para>
        ///     <para>
        ///         要素の走査は [0, 0] の要素から開始する。<br/>
        ///         走査方向は <paramref name="shouldScanRowDirection"/> によって変化する。<br/>
        ///         <paramref name="shouldScanRowDirection"/> が <see langword="false"/> の場合、行方向に走査する。<br/>
        ///         <paramref name="shouldScanRowDirection"/> が <see langword="true"/> の場合、列方向に走査する。<br/>
        ///     </para>
        /// </remarks>
        /// <param name="item">対象要素</param>
        /// <param name="comparer">等価比較メソッド実装</param>
        /// <param name="shouldScanRowDirection">要素走査方向 = 列フラグ</param>
        /// <returns>
        ///     要素が含まれている場合、 (row: 行インデックス, column: 列インデックス)。
        ///     要素が含まれていない場合、(row: -1, column: -1)。
        /// </returns>
        public (int row, int column) IndexOf([AllowNull] TItem item, IEqualityComparer<TItem>? comparer = null,
            bool shouldScanRowDirection = false);

        /// <summary>
        ///     指定したオブジェクトを検索し、最初に出現する行のインデックスを返す。
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         <paramref name="comparer"/> が <see langword="null"/> の場合、
        ///         要素の等価判定は <typeparamref name="TItem"/>.<see cref="object.Equals(object)"/> メソッドで行う。
        ///         <paramref name="comparer"/> が <see langword="null"/> ではない場合、
        ///         要素の等価判定は <see cref="IEqualityComparer{T}"/>.<see cref="IEqualityComparer{T}.Equals(T,T)"/> メソッドで行う。
        ///     </para>
        ///     <para>
        ///         比較は行単位で行う。
        ///     </para>
        /// </remarks>
        /// <param name="item">対象要素</param>
        /// <returns>要素が含まれていない場合、-1</returns>
        /// <param name="comparer">等価比較メソッド実装</param>
        public int RowIndexOf(IEnumerable<TItem>? item, IEqualityComparer<TItem>? comparer = null);

        /// <summary>
        ///     指定したオブジェクトを検索し、最初に出現する列のインデックスを返す。
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         <paramref name="comparer"/> が <see langword="null"/> の場合、
        ///         要素の等価判定は <typeparamref name="TItem"/>.<see cref="object.Equals(object)"/> メソッドで行う。
        ///         <paramref name="comparer"/> が <see langword="null"/> ではない場合、
        ///         要素の等価判定は <see cref="IEqualityComparer{T}"/>.<see cref="IEqualityComparer{T}.Equals(T,T)"/> メソッドで行う。
        ///     </para>
        ///     <para>
        ///         比較は列単位で行う。
        ///     </para>
        /// </remarks>
        /// <param name="item">対象要素</param>
        /// <returns>要素が含まれていない場合、-1</returns>
        /// <param name="comparer">等価比較メソッド実装</param>
        public int ColumnIndexOf(IEnumerable<TItem>? item, IEqualityComparer<TItem>? comparer = null);

        /// <summary>
        ///     すべての要素を、指定された配列のインデックスから始まる部分にコピーする。
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         コピーする要素の順序は <paramref name="shouldTakeRowDirection"/> によって変化する。<br/>
        ///         <paramref name="shouldTakeRowDirection"/> が <see langword="false"/> の場合、行方向に順次コピーする。<br/>
        ///         <paramref name="shouldTakeRowDirection"/> が <see langword="true"/> の場合、列方向に順次コピーする。<br/>
        ///     </para>
        /// </remarks>
        /// <param name="array">コピー先の配列</param>
        /// <param name="index">[Range(0, <paramref name="array"/>.<see cref="Array.Length"/> - 1)] コピー開始インデックス</param>
        /// <param name="shouldTakeRowDirection">要素コピー順序 = 列->行フラグ</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="array"/> が <see langword="null"/> の場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="index"/> が 指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">コピー先の領域が不足する場合</exception>
        public void CopyTo(TItem[] array, int index, bool shouldTakeRowDirection = false);

        /// <summary>
        ///     すべての要素を、指定された配列のインデックスから始まる部分にコピーする。
        /// </summary>
        /// <param name="array">コピー先の配列</param>
        /// <param name="row">[Range(0, <paramref name="array"/>.<see cref="Array.Length"/> - 1)] コピー開始行インデックス</param>
        /// <param name="column">[Range(0, <paramref name="array"/>.<see cref="Array.Length"/> - 1)] コピー開始列インデックス</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="array"/> が <see langword="null"/> の場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="row"/>, <paramref name="column"/> が 指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">コピー先の領域が不足する場合</exception>
        public void CopyTo(TItem[,] array, int row, int column);

        /// <summary>
        ///     すべての要素を、指定された配列のインデックスから始まる部分にコピーする。
        /// </summary>
        /// <param name="array">コピー先の配列</param>
        /// <param name="row">[Range(0, <paramref name="array"/>.<see cref="Array.Length"/> - 1)] コピー開始行インデックス</param>
        /// <param name="column">[Range(0, <paramref name="array"/>[0].<see cref="Array.Length"/> - 1)] コピー開始列インデックス</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="array"/> が <see langword="null"/> の場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="row"/>, <paramref name="column"/> が 指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">コピー先の領域が不足する場合</exception>
        public void CopyTo(TItem[][] array, int row, int column);

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
        public TItem[][] ToTwoDimensionalArray(bool isTranspose = false);

        /// <summary>
        ///     現在のオブジェクトが、別のオブジェクトと同値であるかどうかを示す。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>
        ///     同値 または 同一 である場合 <see langword="true"/>。
        /// </returns>
        public bool ItemEquals(IEnumerable<IEnumerable<TItem>>? other);

        /// <summary>
        ///     現在のオブジェクトが、別のオブジェクトと同値であるかどうかを示す。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <param name="itemComparer">子要素比較処理</param>
        /// <returns>
        ///     同値 または 同一 である場合 <see langword="true"/>。
        /// </returns>
        public bool ItemEquals(IEnumerable<IEnumerable<TItem>>? other, IEqualityComparer<TItem>? itemComparer);
    }
}
