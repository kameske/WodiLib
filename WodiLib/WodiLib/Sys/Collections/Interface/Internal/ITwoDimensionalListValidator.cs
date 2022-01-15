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
    internal interface ITwoDimensionalListValidator<TRow, TItem>
    {
        /// <summary>
        ///     コンストラクタの検証処理
        /// </summary>
        /// <param name="initItems">初期要素</param>
        public void Constructor(NamedValue<TRow[]> initItems);

        /// <summary>
        ///     <see cref="IReadableTwoDimensionalList{TOutRow,TOutItem}.GetRow"/> メソッドの検証処理。
        /// </summary>
        /// <param name="rowIndex">行インデックス</param>
        /// <param name="rowCount">行数</param>
        public void GetRow(NamedValue<int> rowIndex, NamedValue<int> rowCount);

        /// <summary>
        ///     <see cref="IReadableTwoDimensionalList{TOutRow,TOutItem}.GetColumn"/> メソッドの検証処理。
        /// </summary>
        /// <param name="columnIndex">列インデックス</param>
        /// <param name="columnCount">列数</param>
        public void GetColumn(NamedValue<int> columnIndex, NamedValue<int> columnCount);

        /// <summary>
        ///     <see cref="IReadableTwoDimensionalList{TOutRow,TOutItem}.GetItem"/> メソッドの検証処理。
        /// </summary>
        /// <param name="rowIndex">行インデックス</param>
        /// <param name="rowCount">行数</param>
        /// <param name="columnIndex">列インデックス</param>
        /// <param name="columnCount">列数</param>
        public void GetItem(
            NamedValue<int> rowIndex,
            NamedValue<int> rowCount,
            NamedValue<int> columnIndex,
            NamedValue<int> columnCount
        );

        /// <summary>
        ///     <see cref="ITwoDimensionalList{TInRow,TOutRow,TInItem,TOutItem}.this"/> メソッドの検証処理。
        /// </summary>
        /// <param name="rowIndex">行インデックス</param>
        /// <param name="columnIndex">列インデックス</param>
        public void GetItem(NamedValue<int> rowIndex, NamedValue<int> columnIndex);

        /// <summary>
        ///     <see cref="IWritableTwoDimensionalList{TInRow,TOutRow,TInItem,TOutItem}.SetRow"/> メソッドの検証処理。
        /// </summary>
        /// <param name="rowIndex">更新開始行インデックス</param>
        /// <param name="targetParamName">更新要素パラメータ名</param>
        /// <param name="rows">更新要素</param>
        public void SetRow(NamedValue<int> rowIndex, string targetParamName, params TRow[] rows);

        /// <summary>
        ///     <see cref="IWritableTwoDimensionalList{TInRow,TOutRow,TInItem,TOutItem}.SetColumn"/> メソッドの検証処理。
        /// </summary>
        /// <param name="columnIndex">更新列インデックス</param>
        /// <param name="targetParamName">更新要素パラメータ名</param>
        /// <param name="items">更新要素</param>
        public void SetColumn(NamedValue<int> columnIndex, string targetParamName, params IEnumerable<TItem>[] items);

        /// <summary>
        ///     <see cref="ITwoDimensionalList{TInRow,TOutRow,TInItem,TOutItem}.this"/> メソッドの検証処理。
        /// </summary>
        /// <param name="rowIndex">更新開始行インデックス</param>
        /// <param name="columnIndex">更新列インデックス</param>
        /// <param name="item">更新要素</param>
        public void SetItem(NamedValue<int> rowIndex, NamedValue<int> columnIndex, NamedValue<TItem> item);

        /// <summary>
        ///     <see cref="IRowSizeChangeableTwoDimensionalList{TInRow,IOutRow,TInItem}.InsertRow"/> メソッドの検証処理。
        /// </summary>
        /// <param name="rowIndex">行インデックス</param>
        /// <param name="targetParamName">挿入要素パラメータ名</param>
        /// <param name="items">挿入する要素</param>
        public void InsertRow(NamedValue<int> rowIndex, string targetParamName, params TRow[] items);

        /// <summary>
        ///     <see cref="IColumnSizeChangeableTwoDimensionalList{TOutRow,TInItem}.InsertColumn"/> メソッドの検証処理。
        /// </summary>
        /// <param name="columnIndex">列インデックス</param>
        /// <param name="targetParamName">挿入要素パラメータ名</param>
        /// <param name="items">挿入する要素</param>
        public void InsertColumn(
            NamedValue<int> columnIndex,
            string targetParamName,
            params IEnumerable<TItem>[] items
        );

        /// <summary>
        ///     <see cref="IRowSizeChangeableTwoDimensionalList{TInRow,IOutRow,TInItem}.OverwriteRow"/> メソッドの検証処理。
        /// </summary>
        /// <param name="rowIndex">行インデックス</param>
        /// <param name="targetParamName">上書き/追加要素パラメータ名</param>
        /// <param name="items">上書き/追加リスト</param>
        public void OverwriteRow(NamedValue<int> rowIndex, string targetParamName, params TRow[] items);

        /// <summary>
        ///     <see cref="IColumnSizeChangeableTwoDimensionalList{TOutRow,TInItem}.OverwriteColumn"/> メソッドの検証処理。
        /// </summary>
        /// <param name="columnIndex">列インデックス</param>
        /// <param name="targetParamName">上書き/追加要素パラメータ名</param>
        /// <param name="items">上書き/追加リスト</param>
        public void OverwriteColumn(
            NamedValue<int> columnIndex,
            string targetParamName,
            params IEnumerable<TItem>[] items
        );

        /// <summary>
        ///     <see cref="IWritableTwoDimensionalList{TInRow,TOutRow,TInItem,TOutItem}.MoveRow"/> メソッドの検証処理。
        /// </summary>
        /// <param name="oldRowIndex">移動する項目の行番号開始位置</param>
        /// <param name="newRowIndex">移動先の行番号開始位置</param>
        /// <param name="count">移動させる要素数</param>
        public void MoveRow(NamedValue<int> oldRowIndex, NamedValue<int> newRowIndex, NamedValue<int> count);

        /// <summary>
        ///     <see cref="ITwoDimensionalList{TInRow,TOutRow,TInItem,TOutItem}.MoveColumn"/> メソッドの検証処理。
        /// </summary>
        /// <param name="oldColumnIndex">移動する項目の列番号開始位置</param>
        /// <param name="newColumnIndex">移動先の列番号開始位置</param>
        /// <param name="count">移動させる要素数</param>
        public void MoveColumn(NamedValue<int> oldColumnIndex, NamedValue<int> newColumnIndex, NamedValue<int> count);

        /// <summary>
        ///     <see cref="ITwoDimensionalList{TInRow,TOutRow,TInItem,TOutItem}.RemoveRow"/> メソッドの検証処理。
        /// </summary>
        /// <param name="rowIndex">インデックス</param>
        /// <param name="count">削除する要素数</param>
        public void RemoveRow(NamedValue<int> rowIndex, NamedValue<int> count);

        /// <summary>
        ///     <see cref="ITwoDimensionalList{TInRow,TOutRow,TInItem,TOutItem}.RemoveColumn"/> メソッドの検証処理。
        /// </summary>
        /// <param name="columnIndex">インデックス</param>
        /// <param name="count">削除する要素数</param>
        public void RemoveColumn(NamedValue<int> columnIndex, NamedValue<int> count);

        /// <summary>
        ///     <see cref="ITwoDimensionalList{TInRow,TOutRow,TInItem,TOutItem}.AdjustLength"/> メソッドの検証処理。
        /// </summary>
        /// <param name="rowLength">調整する行数</param>
        /// <param name="columnLength">調整する列数</param>
        public void AdjustLength(NamedValue<int> rowLength, NamedValue<int> columnLength);

        /// <summary>
        ///     <see cref="ISizeChangeableTwoDimensionalList{TOutRow,TInRow,TInItem}.Reset"/> メソッドの検証処理。
        /// </summary>
        /// <param name="initItems">リストに詰め直す要素</param>
        public void Reset(NamedValue<IEnumerable<TRow>> initItems);
    }
}
