// ========================================
// Project Name : WodiLib
// File Name    : ArgumentNewLineException.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;

namespace WodiLib.Sys
{
    /// <summary>
    /// 文字列に改行コードを含む場合の例外
    /// </summary>
    public class ArgumentNewLineException : ArgumentException
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message">エラーメッセージ</param>
        public ArgumentNewLineException(string message) : base(message) { }
    }
}