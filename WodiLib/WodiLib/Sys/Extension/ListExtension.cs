// ========================================
// Project Name : WodiLib
// File Name    : ListExtension.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;

namespace WodiLib.Sys
{
    /// <summary>
    /// リストの拡張クラス
    /// </summary>
    internal static class ListExtension
    {
        /// <summary>
        /// リストの長さを調整する。
        /// </summary>
        /// <param name="target">対象</param>
        /// <param name="length">[Range(1, int.MaxValue)] 調整する長さ</param>
        /// <param name="defaultValue">要素数を増やす場合にデフォルトで設定する値</param>
        public static void AdjustLength<T>(this List<T> target, int length, T defaultValue = default(T))
        {
            var count = target.Count;

            if (count == length) return;

            if (count < length)
            {
                // 長さが足りないので追加
                while (target.Count < length)
                {
                    target.Add(defaultValue);
                }

                return;
            }

            // 長すぎるので除去
            var removeLength = count - length;
            var removeStartIndex = removeLength - 1;
            target.RemoveRange(removeStartIndex, removeLength);
        }
    }
}