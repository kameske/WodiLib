// ========================================
// Project Name : WodiLib
// File Name    : ITwoDimensionalListValidator.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     二次元リスト 検証者インタフェース
    /// </summary>
    /// <typeparam name="TRow">リスト行データ入力型</typeparam>
    /// <typeparam name="TItem">リスト要素入力型</typeparam>
    internal interface ITwoDimensionalListValidator<in TRow, in TItem>
    {
        /// <summary>
        ///     コンストラクタの検証処理
        /// </summary>
        /// <param name="iniTs">初期要素</param>
        public void Constructor(TRow[] iniTs);

        /// <summary>
        ///     <see cref="IReadableTwoDimensionalList{TOutRow,TOutItem}.GetRow"/> メソッドの検証処理。
        /// </summary>
        /// <param name="rowIndex">行インデックス</param>
        /// <param name="rowCount">行数</param>
        public void GetRow(int rowIndex, int rowCount);

        /// <summary>
        ///     <see cref="IReadableTwoDimensionalList{TOutRow,TOutItem}.GetColumn"/> メソッドの検証処理。
        /// </summary>
        /// <param name="columnIndex">列インデックス</param>
        /// <param name="columnCount">列数</param>
        public void GetColumn(int columnIndex, int columnCount);

        /// <summary>
        ///     <see cref="IReadableTwoDimensionalList{TOutRow,TOutItem}.GetItem"/> メソッドの検証処理。
        /// </summary>
        /// <param name="rowIndex">行インデックス</param>
        /// <param name="rowCount">行数</param>
        /// <param name="columnIndex">列インデックス</param>
        /// <param name="columnCount">列数</param>
        public void GetItem(int rowIndex, int rowCount, int columnIndex, int columnCount);

        /// <summary>
        ///     <see cref="ITwoDimensionalList{TInRow,TOutRow,TInItem,TOutItem}.this"/> メソッドの検証処理。
        /// </summary>
        /// <param name="rowIndex">行インデックス</param>
        /// <param name="columnIndex">列インデックス</param>
        public void GetItem(int rowIndex, int columnIndex);

        /// <summary>
        ///     <see cref="IWritableTwoDimensionalList{TInRow,TOutRow,TInItem,TOutItem}.SetRow"/> メソッドの検証処理。
        /// </summary>
        /// <param name="rowIndex">更新開始行インデックス</param>
        /// <param name="rows">更新要素</param>
        public void SetRow(int rowIndex, params TRow[] rows);

        /// <summary>
        ///     <see cref="IWritableTwoDimensionalList{TInRow,TOutRow,TInItem,TOutItem}.SetColumn"/> メソッドの検証処理。
        /// </summary>
        /// <param name="columnIndex">更新列インデックス</param>
        /// <param name="items">更新要素</param>
        public void SetColumn(int columnIndex, params IEnumerable<TItem>[] items);

        /// <summary>
        ///     <see cref="ITwoDimensionalList{TInRow,TOutRow,TInItem,TOutItem}.this"/> メソッドの検証処理。
        /// </summary>
        /// <param name="rowIndex">更新開始行インデックス</param>
        /// <param name="columnIndex">更新列インデックス</param>
        /// <param name="item">更新要素</param>
        public void SetItem(int rowIndex, int columnIndex, TItem item);

        /// <summary>
        ///     <see cref="IRowSizeChangeableTwoDimensionalList{TInRow,IOutRow,TInItem,TOutItem}.InsertRow"/> メソッドの検証処理。
        /// </summary>
        /// <param name="rowIndex">行インデックス</param>
        /// <param name="items">挿入する要素</param>
        public void InsertRow(int rowIndex, params TRow[] items);

        /// <summary>
        ///     <see cref="IColumnSizeChangeableTwoDimensionalList{TOutRow,TInItem,TOutItem}.InsertColumn"/> メソッドの検証処理。
        /// </summary>
        /// <param name="columnIndex">列インデックス</param>
        /// <param name="items">挿入する要素</param>
        public void InsertColumn(int columnIndex, params IEnumerable<TItem>[] items);

        /// <summary>
        ///     <see cref="IRowSizeChangeableTwoDimensionalList{TInRow,IOutRow,TInItem,TOutItem}.OverwriteRow"/> メソッドの検証処理。
        /// </summary>
        /// <param name="rowIndex">行インデックス</param>
        /// <param name="items">上書き/追加リスト</param>
        public void OverwriteRow(int rowIndex, params TRow[] items);

        /// <summary>
        ///     <see cref="IColumnSizeChangeableTwoDimensionalList{TOutRow,TInItem,TOutItem}.OverwriteColumn"/> メソッドの検証処理。
        /// </summary>
        /// <param name="columnIndex">列インデックス</param>
        /// <param name="items">上書き/追加リスト</param>
        public void OverwriteColumn(int columnIndex, params IEnumerable<TItem>[] items);

        /// <summary>
        ///     <see cref="IWritableTwoDimensionalList{TInRow,TOutRow,TInItem,TOutItem}.MoveRow"/> メソッドの検証処理。
        /// </summary>
        /// <param name="oldRowIndex">移動する項目の行番号開始位置</param>
        /// <param name="newRowIndex">移動先の行番号開始位置</param>
        /// <param name="count">移動させる要素数</param>
        public void MoveRow(int oldRowIndex, int newRowIndex, int count);

        /// <summary>
        ///     <see cref="ITwoDimensionalList{TInRow,TOutRow,TInItem,TOutItem}.MoveColumn"/> メソッドの検証処理。
        /// </summary>
        /// <param name="oldColumnIndex">移動する項目の列番号開始位置</param>
        /// <param name="newColumnIndex">移動先の列番号開始位置</param>
        /// <param name="count">移動させる要素数</param>
        public void MoveColumn(int oldColumnIndex, int newColumnIndex, int count);

        /// <summary>
        ///     <see cref="ITwoDimensionalList{TInRow,TOutRow,TInItem,TOutItem}.RemoveRow"/> メソッドの検証処理。
        /// </summary>
        /// <param name="rowIndex">インデックス</param>
        /// <param name="count">削除する要素数</param>
        public void RemoveRow(int rowIndex, int count);

        /// <summary>
        ///     <see cref="ITwoDimensionalList{TInRow,TOutRow,TInItem,TOutItem}.RemoveColumn"/> メソッドの検証処理。
        /// </summary>
        /// <param name="columnIndex">インデックス</param>
        /// <param name="count">削除する要素数</param>
        public void RemoveColumn(int columnIndex, int count);

        /// <summary>
        ///     <see cref="ITwoDimensionalList{TInRow,TOutRow,TInItem,TOutItem}.AdjustLength"/> メソッドの検証処理。
        /// </summary>
        /// <param name="rowLength">調整する行数</param>
        /// <param name="columnLength">調整する列数</param>
        public void AdjustLength(int rowLength, int columnLength);

        /// <summary>
        ///     <see cref="ISizeChangeableTwoDimensionalList{TOutRow,TInRow,TInItem,TOutItem}.Reset"/> メソッドの検証処理。
        /// </summary>
        /// <param name="initItems">リストに詰め直す要素</param>
        public void Reset(IEnumerable<TRow> initItems);
    }
}
