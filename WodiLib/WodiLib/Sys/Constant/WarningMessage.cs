// ========================================
// Project Name : WodiLib
// File Name    : WarningMessage.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Text.RegularExpressions;

namespace WodiLib.Sys
{
    /// <summary>
    /// 警告メッセージ
    /// </summary>
    internal static class WarningMessage
    {
        /// <summary>
        /// 範囲警告メッセージ
        /// </summary>
        /// <param name="itemName">警告項目名</param>
        /// <param name="min">最小値</param>
        /// <param name="max">最大値</param>
        /// <param name="setValue">設定値</param>
        /// <returns></returns>
        public static string OutOfRange(string itemName, IntOrStr min, IntOrStr max, int setValue)
        {
            return $"{itemName}は{min.ToValueString()}～{max.ToValueString()}の範囲外で設定されているため、" +
                   $"ウディタ上での動作が保証されません。(設定値：{setValue})";
        }

        /// <summary>
        /// いずれの型にもキャスト不可能な場合の警告メッセージ
        /// </summary>
        /// <param name="itemName">エラー項目名</param>
        /// <param name="setValue">設定値</param>
        /// <returns></returns>
        public static string CannotSetValue(string itemName, object setValue)
        {
            return $"{itemName}にウディタ上では設定できない値が設定されました。" +
                   $"ウディタ上での動作は保証されません。(設定値：{setValue})";
        }

        /// <summary>
        /// 不適切なファイル名の場合の警告メッセージ
        /// </summary>
        /// <param name="path">対象ファイルパス</param>
        /// <param name="regex">適切な正規表現</param>
        /// <returns>メッセージ</returns>
        public static string UnsuitableFileName(string path, Regex regex)
        {
            return $"ファイル名\"{path}\"は正規表現\"{regex}\"に一致しません。";
        }
    }
}