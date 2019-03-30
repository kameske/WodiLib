// ========================================
// Project Name : WodiLib
// File Name    : IntExtension.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using WodiLib.Map;

namespace WodiLib.Event.EventCommand
{
    /// <summary>
    /// int拡張クラス
    /// </summary>
    internal static class IntExtension
    {
        /// <summary>
        /// マップイベントIDかどうかを返す
        /// </summary>
        /// <param name="src">対象</param>
        /// <returns>マップイベントIDの場合、true（「主人公」を示す場合もtrue）</returns>
        public static bool IsMapEventId(this int src)
        {
            try
            {
                var _ = (MapEventId) src;
                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                return false;
            }
        }
    }
}