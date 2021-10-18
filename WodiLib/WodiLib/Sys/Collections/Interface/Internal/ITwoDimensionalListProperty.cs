// ========================================
// Project Name : WodiLib
// File Name    : ITwoDimensionalListProperty.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.Sys.Collections
{
    /// <summary>
    /// 二次元リストプロパティ
    /// </summary>
    internal interface ITwoDimensionalListProperty
    {
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
    }
}
