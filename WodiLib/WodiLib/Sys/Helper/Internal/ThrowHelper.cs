// ========================================
// Project Name : WodiLib
// File Name    : ThrowHelper.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Diagnostics.CodeAnalysis;

namespace WodiLib.Sys
{
    /// <summary>
    /// 例外スロー用Helperクラス
    /// </summary>
    internal static class ThrowHelper
    {
        #region Validate Property

        #endregion

        #region Validate Argument

        /// <summary>
        /// 引数が <see langword="null"/> でないことを検証する際の例外処理
        /// </summary>
        /// <param name="isThrow">検証結果</param>
        /// <param name="itemName">検証項目名</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="isThrow"/> が <see langword="true"/> の場合
        /// </exception>
        public static void ValidateArgumentNotNull([DoesNotReturnIf(true)] bool isThrow, string itemName)
        {
            if (!isThrow) return;

            throw new ArgumentNullException(
                ErrorMessage.NotNull(itemName));
        }

        /// <summary>
        /// 引数が 空文字 でないことを検証する際の例外処理
        /// </summary>
        /// <param name="isThrow">検証結果</param>
        /// <param name="itemName">検証項目名</param>
        /// <exception cref="ArgumentException">
        ///     <paramref name="isThrow"/> が <see langword="true"/> の場合
        /// </exception>
        public static void ValidateValidateArgumentNotEmpty([DoesNotReturnIf(true)] bool isThrow, string itemName)
        {
            if (!isThrow) return;

            throw new ArgumentException(
                ErrorMessage.NotEmpty(itemName));
        }

        #endregion
    }
}
