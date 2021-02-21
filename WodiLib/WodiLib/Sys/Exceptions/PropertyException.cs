// ========================================
// Project Name : WodiLib
// File Name    : PropertyException.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;

namespace WodiLib.Sys
{
    /// <inheritdoc/>
    /// <summary>
    ///     プロパティの例外
    /// </summary>
    public class PropertyException : Exception
    {
        /// <inheritdoc/>
        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public PropertyException()
        {
        }

        /// <inheritdoc/>
        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="message">エラーメッセージ</param>
        public PropertyException(string message) : base(message)
        {
        }
    }
}
