// ========================================
// Project Name : WodiLib
// File Name    : PropertyEmptyException.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.Sys
{
    /// <inheritdoc />
    /// <summary>
    /// プロパティが空の場合の例外
    /// </summary>
    public class PropertyEmptyException : PropertyException
    {
        /// <inheritdoc />
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PropertyEmptyException()
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message">エラーメッセージ</param>
        public PropertyEmptyException(string message) : base(message)
        {
        }
    }
}