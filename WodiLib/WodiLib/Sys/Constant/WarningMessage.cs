// ========================================
// Project Name : WodiLib
// File Name    : WarningMessage.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

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


    }
}