// ========================================
// Project Name : WodiLib
// File Name    : Validator.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.Sys
{
    /// <summary>
    /// バリデーションクラス
    /// </summary>
    internal static class Validator
    {
        /// <summary>
        /// 範囲チェックを行う。
        /// </summary>
        /// <param name="target">チェック対象</param>
        /// <param name="min">最小値</param>
        /// <param name="max">最大値</param>
        /// <returns>min &lt;= target &lt;= max の場合、true</returns>
        public static bool Range(int target, int min, int max)
        {
            return min <= target && target <= max;
        }

    }
}