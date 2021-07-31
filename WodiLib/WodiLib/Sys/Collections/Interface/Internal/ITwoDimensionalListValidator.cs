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
        /// <param name="iniTs">初期要素</param>
        public void Constructor(T[][] iniTs);

        /// <summary>
        ///     <see cref="TwoDimensionalList{T}.GetRow"/> メソッドの検証処理。
        /// </summary>
        /// <param name="rowIndex">行インデックス</param>
        /// <param name="rowCount">行数</param>
        public void GetRow(int rowIndex, int rowCount);

        /// <summary>
        ///     <see cref="TwoDimensionalList{T}.GetColumn"/> メソッドの検証処理。
        /// </summary>
        /// <param name="columnIndex">列インデックス</param>
        /// <param name="columnCount">列数</param>
        public void GetColumn(int columnIndex, int columnCount);

        /// <summary>
        ///     <see cref="TwoDimensionalList{T}.GetItem"/> メソッドの検証処理。
        /// </summary>
        /// <param name="rowIndex">行インデックス</param>
        /// <param name="rowCount">行数</param>
        /// <param name="columnIndex">列インデックス</param>
        /// <param name="columnCount">列数</param>
        public void GetItem(int rowIndex, int rowCount, int columnIndex, int columnCount);

        /// <summary>
        ///     <see cref="TwoDimensionalList{T}.this"/> メソッドの検証処理。
        /// </summary>
        /// <param name="rowIndex">行インデックス</param>
        /// <param name="columnIndex">列インデックス</param>
        public void GetItem(int rowIndex, int columnIndex);

        /// <summary>
        ///     <see cref="TwoDimensionalList{T}.SetRow"/> メソッドの検証処理。
        /// </summary>
        /// <param name="rowIndex">更新開始行インデックス</param>
        /// <param name="rows">更新要素</param>
        public void SetRow(int rowIndex, params IEnumerable<T>[] rows);

        /// <summary>
        ///     <see cref="TwoDimensionalList{T}.SetColumn"/> メソッドの検証処理。
        /// </summary>
        /// <param name="columnIndex">更新列インデックス</param>
        /// <param name="items">更新要素</param>
        public void SetColumn(int columnIndex, params IEnumerable<T>[] items);

        /// <summary>
        ///     <see cref="TwoDimensionalList{T}.SetRow"/> メソッドの検証処理。
        /// </summary>
        /// <param name="rowIndex">更新開始行インデックス</param>
        /// <param name="columnIndex">更新列インデックス</param>
        /// <param name="item">更新要素</param>
        public void SetItem(int rowIndex, int columnIndex, T item);

        /// <summary>
        ///     <see cref="TwoDimensionalList{T}.InsertRow"/> メソッドの検証処理。
        /// </summary>
        /// <param name="rowIndex">行インデックス</param>
        /// <param name="items">挿入する要素</param>
        public void InsertRow(int rowIndex, params IEnumerable<T>[] items);

        /// <summary>
        ///     <see cref="TwoDimensionalList{T}.InsertColumn"/> メソッドの検証処理。
        /// </summary>
        /// <param name="columnIndex">列インデックス</param>
        /// <param name="items">挿入する要素</param>
        public void InsertColumn(int columnIndex, params IEnumerable<T>[] items);

        /// <summary>
        ///     <see cref="TwoDimensionalList{T}.OverwriteRow"/> メソッドの検証処理。
        /// </summary>
        /// <param name="rowIndex">行インデックス</param>
        /// <param name="items">上書き/追加リスト</param>
        public void OverwriteRow(int rowIndex, params IEnumerable<T>[] items);

        /// <summary>
        ///     <see cref="TwoDimensionalList{T}.OverwriteColumn"/> メソッドの検証処理。
        /// </summary>
        /// <param name="columnIndex">列インデックス</param>
        /// <param name="items">上書き/追加リスト</param>
        public void OverwriteColumn(int columnIndex, params IEnumerable<T>[] items);

        /// <summary>
        ///     <see cref="TwoDimensionalList{T}.MoveRow"/> メソッドの検証処理。
        /// </summary>
        /// <param name="oldRowIndex">
        ///     移動する項目の行番号開始位置
        /// </param>
        /// <param name="newRowIndex">
        ///     移動先の行番号開始位置
        /// </param>
        /// <param name="count">
        ///     移動させる要素数
        /// </param>
        public void MoveRow(int oldRowIndex, int newRowIndex, int count);

        /// <summary>
        ///     <see cref="TwoDimensionalList{T}.MoveColumn"/> メソッドの検証処理。
        /// </summary>
        /// <param name="oldColumnIndex">
        ///     移動する項目の列番号開始位置
        /// </param>
        /// <param name="newColumnIndex">
        ///     移動先の列番号開始位置
        /// </param>
        /// <param name="count">
        ///     移動させる要素数
        /// </param>
        public void MoveColumn(int oldColumnIndex, int newColumnIndex, int count);

        /// <summary>
        ///     <see cref="TwoDimensionalList{T}.RemoveRow"/> メソッドの検証処理。
        /// </summary>
        /// <param name="rowIndex">インデックス</param>
        /// <param name="count">削除する要素数</param>
        public void RemoveRow(int rowIndex, int count);

        /// <summary>
        ///     <see cref="TwoDimensionalList{T}.RemoveColumn"/> メソッドの検証処理。
        /// </summary>
        /// <param name="columnIndex">インデックス</param>
        /// <param name="count">削除する要素数</param>
        public void RemoveColumn(int columnIndex, int count);

        /// <summary>
        ///     <see cref="TwoDimensionalList{T}.AdjustLength"/> メソッドの検証処理。
        /// </summary>
        /// <param name="rowLength">
        ///     調整する行数
        /// </param>
        /// <param name="columnLength">
        ///     調整する列数
        /// </param>
        public void AdjustLength(int rowLength, int columnLength);

        /// <summary>
        ///     <see cref="TwoDimensionalList{T}.Reset(IEnumerable{IEnumerable{T}})"/> メソッドの検証処理。
        /// </summary>
        /// <param name="initItems">リストに詰め直す要素</param>
        public void Reset(IEnumerable<IEnumerable<T>> initItems);

        /// <summary>
        ///     自身と同じ検証能力を持つ <paramref name="target"/> 用の
        ///     <see cref="ITwoDimensionalListValidator{T}"/> インスタンスを作成する。
        /// </summary>
        /// <param name="target">検証対象</param>
        /// <returns>新規検証処理インスタンス</returns>
        public ITwoDimensionalListValidator<T> CreateAnotherFor(ITwoDimensionalList<T> target);
    }
}
