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
    internal interface ITwoDimensionalListValidator<T>
    {
        /// <summary>
        ///     コンストラクタの検証処理
        /// </summary>
        /// <param name="initItems">初期要素</param>
        public void Constructor(T[][] initItems);

        /// <summary>
        ///     CopyTo メソッドの検証処理
        /// </summary>
        /// <param name="array">コピー先の配列</param>
        /// <param name="index">コピー開始インデックス</param>
        public void CopyTo(IReadOnlyList<T>[] array, int index);

        /// <summary>
        ///     CopyTo メソッドの検証処理
        /// </summary>
        /// <param name="array">コピー先の配列</param>
        /// <param name="index">コピー開始インデックス</param>
        public void CopyTo(IEnumerable<T>[] array, int index);

        /// <summary>
        ///     CopyTo メソッドの検証処理
        /// </summary>
        /// <param name="array">コピー先の配列</param>
        /// <param name="index">コピー開始インデックス</param>
        /// <param name="direction">コピー方向</param>
        public void CopyTo(T[] array, int index, Direction direction);

        /// <summary>
        ///     CopyTo メソッドの検証処理
        /// </summary>
        /// <param name="array">コピー先の配列</param>
        /// <param name="row">コピー開始行番号</param>
        /// <param name="column">コピー開始列番号</param>
        public void CopyTo(T[,] array, int row, int column);

        /// <summary>
        ///     CopyTo メソッドの検証処理
        /// </summary>
        /// <param name="array">コピー先の配列</param>
        /// <param name="row">コピー開始行番号</param>
        /// <param name="column">コピー開始列番号</param>
        public void CopyTo(T[][] array, int row, int column);

        /// <summary>
        ///     Get, GetRange, GetRow, GetRowRange, GetColumn, GetColumnRange メソッドの検証処理
        /// </summary>
        /// <param name="row">行番号</param>
        /// <param name="rowCount">行数</param>
        /// <param name="column">列番号</param>
        /// <param name="columnCount">列数</param>
        /// <param name="direction">取得方向</param>
        public void Get(int row, int rowCount, int column, int columnCount, Direction direction);

        /// <summary>
        ///     Set, SetRange メソッドの検証処理
        /// </summary>
        /// <param name="row">更新開始行番号</param>
        /// <param name="column">更新開始列番号</param>
        /// <param name="items">更新要素</param>
        /// <param name="direction">更新方向</param>
        /// <param name="needFitItemsInnerSize"><paramref name="items"/>の内部要素数が自身の列または行と一致する必要性</param>
        public void Set(int row, int column, T[][] items, Direction direction, bool needFitItemsInnerSize);

        /// <summary>
        ///     AddRow, AddRowRange, AddColumn, AddColumnRange,
        ///     InsertRow, InsertRowRange, InsertColumn, InsertColumnRange メソッドの検証処理
        /// </summary>
        /// <param name="index">挿入インデックス</param>
        /// <param name="items">挿入要素</param>
        /// <param name="direction">挿入方向</param>
        public void Insert(int index, T[][] items, Direction direction);

        /// <summary>
        ///     OverwriteRow, OverwriteColumn メソッドの検証処理
        /// </summary>
        /// <param name="index">開始インデックス</param>
        /// <param name="items">上書き/追加リスト</param>
        /// <param name="direction">上書き/追加方向</param>
        public void Overwrite(int index, T[][] items, Direction direction);

        /// <summary>
        ///     MoveRow, MoveRowRange, MoveColumn, MoveColumnRange メソッドの検証処理
        /// </summary>
        /// <param name="oldIndex">移動前インデックス</param>
        /// <param name="newIndex">移動後インデックス</param>
        /// <param name="count">移動行数</param>
        /// <param name="direction">移動方向</param>
        public void Move(int oldIndex, int newIndex, int count, Direction direction);

        /// <summary>
        ///     RemoveRow, RemoveRowRange, RemoveColumn, RemoveColumnRange メソッドの検証処理
        /// </summary>
        /// <param name="index">削除開始インデックス</param>
        /// <param name="count">削除数</param>
        /// <param name="direction">削除方向</param>
        public void Remove(int index, int count, Direction direction);

        /// <summary>
        ///     AdjustLength, AdjustLengthIfShort, AdjustLengthIfLong,
        ///     AdjustRowLength, AdjustRowLengthIfShort, AdjustRowLengthIfLong,
        ///     AdjustColumnLength, AdjustColumnLengthIfShort, AdjustColumnLengthIfLong メソッドの検証処理
        /// </summary>
        /// <param name="rowLength">調整行数</param>
        /// <param name="columnLength">調整列数</param>
        public void AdjustLength(int rowLength, int columnLength);

        /// <summary>
        ///     Reset, Clear メソッドの検証処理
        /// </summary>
        /// <param name="items">初期化要素</param>
        public void Reset(T[][] items);

        /// <summary>
        ///     自身と同じ検証能力を持つ <paramref name="target"/> 用の
        ///     <see cref="ITwoDimensionalListValidator{T}"/> インスタンスを作成する。
        /// </summary>
        /// <param name="target">検証対象</param>
        /// <returns>新規検証処理インスタンス</returns>
        public ITwoDimensionalListValidator<T> CreateAnotherFor(IReadOnlyTwoDimensionalList<T> target);
    }
}
