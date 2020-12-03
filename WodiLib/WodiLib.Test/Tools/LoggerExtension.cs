// ========================================
// Project Name : WodiLib
// File Name    : LoggerExtension.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Text.Json;
using Commons;

namespace WodiLib.Test.Tools
{
    /// <summary>
    /// Commons.Logger 拡張クラス
    /// </summary>
    internal static class LoggerExtension
    {
        /// <summary>
        /// 任意の値をJSON形式でDebug出力する
        /// </summary>
        /// <param name="logger">ロガー</param>
        /// <param name="target">出力対象</param>
        /// <param name="targetName">出力対象名称</param>
        public static void DebugObjectToJson(this Logger logger, object target, string targetName = null)
        {
            var jsonText = JsonSerializer.Serialize(target, new JsonSerializerOptions
            {
                WriteIndented = true,
            });
            logger.Debug($"{(targetName != null ? $"\"{targetName}\":" : "")}{jsonText}");
        }
    }
}
