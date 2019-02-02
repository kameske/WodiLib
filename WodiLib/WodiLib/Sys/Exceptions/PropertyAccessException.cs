// ========================================
// Project Name : WodiLib
// File Name    : PropertyAccessException.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.Sys
{
    /// <inheritdoc />
    /// <summary>
    /// プロパティアクセス禁止例外
    /// </summary>
    public class PropertyAccessException : PropertyException
    {
        /// <inheritdoc />
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PropertyAccessException()
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message">エラーメッセージ</param>
        public PropertyAccessException(string message) : base(message)
        {
        }
    }
}