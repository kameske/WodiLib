// ========================================
// Project Name : WodiLib
// File Name    : PropertyNullException.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.Sys
{
    /// <inheritdoc />
    /// <summary>
    ///     Nullを許容していないプロパティにNullが渡されたときの例外
    /// </summary>
    public class PropertyNullException : PropertyException
    {
        /// <inheritdoc />
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PropertyNullException()
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message">エラーメッセージ</param>
        public PropertyNullException(string message) : base(message)
        {
        }
    }
}