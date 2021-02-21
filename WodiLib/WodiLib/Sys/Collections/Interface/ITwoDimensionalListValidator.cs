// ========================================
// Project Name : WodiLib
// File Name    : ITwoDimensionalListValidator.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.Sys
{
    /// <summary>
    /// <see cref="ITwoDimensionalList{T}"/> 検証者インタフェース
    /// </summary>
    public interface ITwoDimensionalListValidator<in T>
    {
        /// <summary>
        /// コンストラクタの検証処理
        /// </summary>
        /// <param name="initItems">初期要素</param>
        void Constructor(T[][] initItems);

        /// <summary>
        /// Get, GetRange メソッドの検証処理
        /// </summary>
        /// <param name="row">行番号</param>
        /// <param name="rowCount">行数</param>
        /// <param name="column">列番号</param>
        /// <param name="columnCount">列数</param>
        void Get(int row, int rowCount, int column, int columnCount);

        /// <summary>
        /// GetRow, GetRowRange メソッドの検証処理
        /// </summary>
        /// <param name="row">行番号</param>
        /// <param name="count">行数</param>
        void GetRow(int row, int count);

        /// <summary>
        /// GetColumn, GetColumnRange メソッドの検証処理
        /// </summary>
        /// <param name="column">列番号</param>
        /// <param name="count">列数</param>
        void GetColumn(int column, int count);

        /// <summary>
        /// Set, SetRange メソッドの検証処理
        /// </summary>
        /// <param name="row">更新開始行番号</param>
        /// <param name="column">更新開始列番号</param>
        /// <param name="items">更新要素</param>
        void Set(int row, int column, T[][] items);

        /// <summary>
        /// AddDataValues, AddRowRange, InsertRow, InsertRowRange メソッドの検証処理
        /// </summary>
        /// <param name="row">挿入行番号</param>
        /// <param name="items">挿入要素</param>
        void InsertRow(int row, T[][] items);

        /// <summary>
        /// AddColumn, AddColumnRange, InsertColumn, InsertColumnRange メソッドの検証処理
        /// </summary>
        /// <param name="column">挿入列番号</param>
        /// <param name="items">挿入要素</param>
        void InsertColumn(int column, T[][] items);

        /// <summary>
        /// OverwriteRow メソッドの検証処理
        /// </summary>
        /// <param name="row">開始行番号</param>
        /// <param name="items">上書き/追加リスト</param>
        void OverwriteRow(int row, T[][] items);

        /// <summary>
        /// OverwriteColumn メソッドの検証処理
        /// </summary>
        /// <param name="column">開始列番号</param>
        /// <param name="items">上書き/追加リスト</param>
        void OverwriteColumn(int column, T[][] items);

        /// <summary>
        /// MoveRow, MoveRowRange メソッドの検証処理
        /// </summary>
        /// <param name="oldRow">移動前行番号</param>
        /// <param name="newRow">移動後行番号</param>
        /// <param name="count">移動行数</param>
        void MoveRow(int oldRow, int newRow, int count);

        /// <summary>
        /// MoveColumn, MoveColumnRange メソッドの検証処理
        /// </summary>
        /// <param name="oldColumn">移動前列番号</param>
        /// <param name="newColumn">移動後列番号</param>
        /// <param name="count">移動列数</param>
        void MoveColumn(int oldColumn, int newColumn, int count);

        /// <summary>
        /// RemoveRow, RemoveRowRange メソッドの検証処理
        /// </summary>
        /// <param name="row">削除開始行番号</param>
        /// <param name="count">削除行数</param>
        void RemoveRow(int row, int count);

        /// <summary>
        /// RemoveColumn, RemoveColumnRange メソッドの検証処理
        /// </summary>
        /// <param name="column">削除開始列番号</param>
        /// <param name="count">削除列数</param>
        void RemoveColumn(int column, int count);

        /// <summary>
        /// AdjustLength メソッドの検証処理
        /// </summary>
        /// <param name="rowLength">調整行数</param>
        /// <param name="columnLength">調整列数</param>
        void AdjustLength(int rowLength, int columnLength);

        /// <summary>
        /// AdjustLengthIfShort メソッドの検証処理
        /// </summary>
        /// <param name="rowLength">調整行数</param>
        /// <param name="columnLength">調整列数</param>
        void AdjustLengthIfShort(int rowLength, int columnLength);

        /// <summary>
        /// AdjustLengthIfLong メソッドの検証処理
        /// </summary>
        /// <param name="rowLength">調整行数</param>
        /// <param name="columnLength">調整列数</param>
        void AdjustLengthIfLong(int rowLength, int columnLength);

        /// <summary>
        /// AdjustRowLength メソッドの検証処理
        /// </summary>
        /// <param name="length">調整要素数</param>
        void AdjustRowLength(int length);

        /// <summary>
        /// AdjustColumnLength メソッドの検証処理
        /// </summary>
        /// <param name="length">調整要素数</param>
        void AdjustColumnLength(int length);

        /// <summary>
        /// AdjustRowLengthIfShort メソッドの検証処理
        /// </summary>
        /// <param name="length">調整要素数</param>
        void AdjustRowLengthIfShort(int length);

        /// <summary>
        /// AdjustColumnLengthIfShort メソッドの検証処理
        /// </summary>
        /// <param name="length">調整要素数</param>
        void AdjustColumnLengthIfShort(int length);

        /// <summary>
        /// AdjustRowLengthIfLong メソッドの検証処理
        /// </summary>
        /// <param name="length">調整要素数</param>
        void AdjustRowLengthIfLong(int length);

        /// <summary>
        /// AdjustColumnLengthIfLong メソッドの検証処理
        /// </summary>
        /// <param name="length">調整要素数</param>
        void AdjustColumnLengthIfLong(int length);

        /// <summary>
        /// Reset, Clear メソッドの検証処理
        /// </summary>
        /// <param name="items">初期化要素</param>
        void Reset(T[][] items);
    }
}
