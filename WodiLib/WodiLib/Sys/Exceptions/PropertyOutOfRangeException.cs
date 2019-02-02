// ========================================
// Project Name : WodiLib
// File Name    : PropertyOutOfRangeException.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.Sys
{
    /// <inheritdoc />
    /// <summary>
    ///     プロパティに許容範囲外の値が渡されたときの例外
    /// </summary>
    public class PropertyOutOfRangeException : PropertyException
    {
        /// <inheritdoc />
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PropertyOutOfRangeException()
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message">エラーメッセージ</param>
        public PropertyOutOfRangeException(string message) : base(message)
        {
        }
    }
}