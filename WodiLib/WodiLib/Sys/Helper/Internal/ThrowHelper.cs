// ========================================
// Project Name : WodiLib
// File Name    : ThrowHelper.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

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

        #region Null

        /// <summary>
        /// 引数が <see langword="null"/> でないことを検証する際の例外処理
        /// </summary>
        /// <param name="isThrow">検証結果</param>
        /// <param name="itemName">検証項目名</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="isThrow"/> が <see langword="true"/> の場合。
        /// </exception>
        public static void ValidateArgumentNotNull([DoesNotReturnIf(true)] bool isThrow, string itemName)
        {
            if (!isThrow) return;

            throw new ArgumentNullException(
                ErrorMessage.NotNull(itemName));
        }

        /// <summary>
        /// 引数が 空文字 でないことを検証する際の例外処理。
        /// </summary>
        /// <param name="isThrow">検証結果</param>
        /// <param name="itemName">検証項目名</param>
        /// <exception cref="ArgumentException">
        ///     <paramref name="isThrow"/> が <see langword="true"/> の場合。
        /// </exception>
        public static void ValidateArgumentNotEmpty([DoesNotReturnIf(true)] bool isThrow, string itemName)
        {
            if (!isThrow) return;

            throw new ArgumentException(
                ErrorMessage.NotEmpty(itemName));
        }

        /// <summary>
        /// 列挙子に <see langword="null"/> が含まれないことを検証する際の例外処理。
        /// </summary>
        /// <param name="isThrow">検証結果</param>
        /// <param name="itemName">検証項目名</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="isThrow"/> が <see langword="true"/> の場合。
        /// </exception>
        public static void ValidateArgumentItemsHasNotNull([DoesNotReturnIf(true)] bool isThrow, string itemName)
        {
            if (!isThrow) return;

            throw new ArgumentNullException(
                ErrorMessage.NotNullInList(itemName));
        }

        #endregion

        #region Value Compare

        /// <summary>
        /// 値が指定値以上であることの検証時の例外処理。
        /// </summary>
        /// <param name="isThrow">検証結果</param>
        /// <param name="itemName">検証項目名</param>
        /// <param name="limit">下限値</param>
        /// <param name="itemValue">検証対象値</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="isThrow"/> が <see langword="true"/> の場合。
        /// </exception>
        public static void ValidateArgumentValueGreaterOrEqual([DoesNotReturnIf(true)] bool isThrow,
            string itemName, IntOrStr limit, int itemValue)
        {
            if (!isThrow) return;

            throw new ArgumentOutOfRangeException(
                ErrorMessage.GreaterOrEqual(itemName, limit, itemValue));
        }

        /// <summary>
        /// 値の範囲検証処理時の例外処理。
        /// </summary>
        /// <param name="isThrow">検証結果</param>
        /// <param name="itemName">検証項目名</param>
        /// <param name="target">検証対象値</param>
        /// <param name="min">最小値</param>
        /// <param name="max">最大値</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="isThrow"/> が <see langword="true"/> の場合。
        /// </exception>
        public static void ValidateArgumentValueRange([DoesNotReturnIf(true)] bool isThrow,
            string itemName, int target, IntOrStr min, IntOrStr max)
        {
            if (!isThrow) return;

            throw new ArgumentOutOfRangeException(
                ErrorMessage.OutOfRange(itemName, min, max, target));
        }

        /// <summary>
        /// 同値検証時の例外処理。
        /// </summary>
        /// <param name="isThrow">検証結果</param>
        /// <param name="itemName">検証項目名</param>
        /// <param name="otherName">比較対象名</param>
        /// <exception cref="ArgumentException">
        ///     <paramref name="isThrow"/> が <see langword="true"/> の場合。
        /// </exception>
        public static void ValidateArgumentNotEqual([DoesNotReturnIf(true)] bool isThrow,
            string itemName, string otherName)
        {
            if (!isThrow) return;

            throw new ArgumentException(
                ErrorMessage.NotEqual(itemName, otherName));
        }

        /// <summary>
        /// 文字列に改行が含まれないことの検証時の例外処理。
        /// </summary>
        /// <param name="isThrow">検証結果</param>
        /// <param name="itemName">検証項目名</param>
        /// <param name="value">検証値</param>
        /// <exception cref="ArgumentNewLineException">
        ///     <paramref name="isThrow"/> が <see langword="true"/> の場合。
        /// </exception>
        public static void ValidateArgumentNotNewLine([DoesNotReturnIf(true)] bool isThrow,
            string itemName, string value)
        {
            if (!isThrow) return;

            throw new ArgumentNewLineException(
                ErrorMessage.NotNewLine(itemName, value));
        }

        /// <summary>
        /// 文字列に改行が含まれないことの検証時の例外処理。
        /// </summary>
        /// <param name="isThrow">検証結果</param>
        /// <param name="value">検証値</param>
        /// <param name="regex">正規表現</param>
        /// <exception cref="ArgumentNewLineException">
        ///     <paramref name="isThrow"/> が <see langword="true"/> の場合。
        /// </exception>
        public static void ValidateArgumentNotRegex([DoesNotReturnIf(true)] bool isThrow,
            string value, Regex regex)
        {
            if (!isThrow) return;

            throw new ArgumentNewLineException(
                ErrorMessage.StringNotMatchRegex(value, regex));
        }

        /// <summary>
        /// サイズが指定以下であることの検証時の例外処理
        /// </summary>
        /// <param name="isThrow">検証結果</param>
        /// <param name="maxSize">最大サイズ</param>
        /// <exception cref="ArgumentException">
        ///     <paramref name="isThrow"/> が <see langword="true"/> の場合。
        /// </exception>
        public static void ValidateOverDataSize([DoesNotReturnIf(true)] bool isThrow,
            int maxSize)
        {
            if (!isThrow) return;

            throw new ArgumentException(
                ErrorMessage.OverDataSize(maxSize));
        }

        #endregion

        #region Validate Argument in List

        /// <summary>
        /// リストの範囲取得メソッドでインデックスと取得数の相関チェック時の例外処理。
        /// </summary>
        /// <param name="isThrow">検証結果</param>
        /// <param name="indexArgName">インデックス引数名</param>
        /// <param name="countArgName">取得数引数名</param>
        /// <exception cref="ArgumentException">
        ///     <paramref name="isThrow"/> が <see langword="true"/> の場合。
        /// </exception>
        public static void ValidateListRange([DoesNotReturnIf(true)] bool isThrow,
            string indexArgName, string countArgName)
        {
            if (!isThrow) return;

            throw new ArgumentException(
                $"{indexArgName}および{countArgName}が有効な範囲を示していません。");
        }

        /// <summary>
        /// 要素数が0でないことを検証する際の例外処理。
        /// </summary>
        /// <param name="isThrow">検証結果</param>
        /// <param name="itemName">検証項目名</param>
        /// <exception cref="InvalidOperationException">
        ///     <paramref name="isThrow"/> が <see langword="true"/> の場合。
        /// </exception>
        public static void ValidateListItemCountNotZero([DoesNotReturnIf(true)] bool isThrow, string itemName)
        {
            if (!isThrow) return;

            throw new InvalidOperationException(
                ErrorMessage.NotExecute($"{itemName}の要素が0個のため"));
        }

        /// <summary>
        /// リスト要素数が超過しないことを検証する際の例外処理。
        /// </summary>
        /// <param name="isThrow">検証結果</param>
        /// <param name="itemName">検証項目名</param>
        /// <param name="limit">要素上限数</param>
        /// <exception cref="InvalidOperationException">
        ///     <paramref name="isThrow"/> が <see langword="true"/> の場合。
        /// </exception>
        public static void ValidateListMaxItemCount([DoesNotReturnIf(true)] bool isThrow,
            string itemName, int limit)
        {
            if (!isThrow) return;

            throw new InvalidOperationException(
                ErrorMessage.OverListLength(limit, itemName));
        }

        /// <summary>
        /// リスト要素数が不足しないことを検証する際の例外処理。
        /// </summary>
        /// <param name="isThrow">検証結果</param>
        /// <param name="itemName">検証項目名</param>
        /// <param name="limit">要素下限数</param>
        /// <exception cref="InvalidOperationException">
        ///     <paramref name="isThrow"/> が <see langword="true"/> の場合。
        /// </exception>
        public static void ValidateListMinItemCount([DoesNotReturnIf(true)] bool isThrow,
            string itemName, int limit)
        {
            if (!isThrow) return;

            throw new InvalidOperationException(
                ErrorMessage.UnderListLength(limit, itemName));
        }

        #endregion

        #region Validate Argument in Tow Dimensional List

        /// <summary>
        /// 二重リストの全行の要素数が一致することを検証する際の例外処理。
        /// </summary>
        /// <param name="isThrow">検証結果</param>
        /// <param name="rowNum">エラー行数</param>
        /// <exception cref="ArgumentException">
        ///     <paramref name="isThrow"/> が <see langword="true"/> の場合。
        /// </exception>
        public static void ValidateTwoDimListInnerItemLength([DoesNotReturnIf(true)] bool isThrow,
            int rowNum)
        {
            if (!isThrow) return;

            throw new ArgumentException(
                $"{rowNum}行目の要素数が基準要素数と異なります。");
        }

        #endregion

        #region Suitable

        /// <summary>
        /// 引数不適切な場合の例外処理
        /// </summary>
        /// <param name="isThrow">検証結果</param>
        /// <param name="itemName">検証項目名</param>
        /// <param name="item">エラーメッセージ表示オブジェクト</param>
        /// <exception cref="ArgumentException">
        ///     <paramref name="isThrow"/> が <see langword="true"/> の場合。
        /// </exception>
        public static void ValidateArgumentUnsuitable([DoesNotReturnIf(true)] bool isThrow,
            string itemName, object item)
        {
            if (!isThrow) return;

            throw new ArgumentException(
                ErrorMessage.Unsuitable(itemName, item));
        }

        #endregion

        #region Not Execute

        /// <summary>
        /// 引数を理由に処理できない場合の例外処理。
        /// </summary>
        /// <param name="isThrow">検証結果</param>
        /// <param name="message">エラーメッセージ</param>
        /// <exception cref="ArgumentException">
        ///     <paramref name="isThrow"/> が <see langword="true"/> の場合。
        /// </exception>
        public static void ValidateArgumentNotExecute([DoesNotReturnIf(true)] bool isThrow,
            Func<string> message)
        {
            if (!isThrow) return;

            throw new ArgumentException(
                ErrorMessage.NotExecute(message()));
        }

        #endregion

        #endregion

        #region Invalid Operation

        /// <summary>
        /// 検証エラー時に <see cref="InvalidOperationException"/> を発生させる。
        /// </summary>
        /// <param name="isThrow">検証結果</param>
        /// <param name="message">エラーメッセージ</param>
        /// <exception cref="InvalidOperationException">
        ///     <paramref name="isThrow"/> が <see langword="true"/> の場合。
        /// </exception>
        public static void InvalidOperationIf([DoesNotReturnIf(true)] bool isThrow,
            Func<string> message)
        {
            if (!isThrow) return;

            throw new InvalidOperationException(
                ErrorMessage.NotExecute(message()));
        }

        /// <summary>
        /// 検証エラー時に <see cref="InvalidOperationException"/> を発生させる。
        /// </summary>
        /// <param name="isThrow">検証結果</param>
        /// <exception cref="InvalidCastException">
        ///     <paramref name="isThrow"/> が <see langword="true"/> の場合。
        /// </exception>
        public static void InvalidCastIf([DoesNotReturnIf(true)] bool isThrow)
        {
            if (!isThrow) return;

            throw new InvalidCastException();
        }

        #endregion

        #region NullPointer

        /// <summary>
        /// <see langword="null"/> 検証時に <see langword="NullReferenceException"/> を発生させる。
        /// </summary>
        /// <param name="isThrow">検証結果</param>
        /// <exception cref="NullReferenceException">
        ///     <paramref name="isThrow"/> が <see langword="true"/> の場合。
        /// </exception>
        public static void ValidateNotNull([DoesNotReturnIf(true)] bool isThrow)
        {
            if (!isThrow) return;

            throw new NullReferenceException();
        }

        #endregion
    }
}
