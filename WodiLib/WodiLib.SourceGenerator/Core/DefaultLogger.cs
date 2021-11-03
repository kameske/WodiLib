// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : DefaultLogger.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Linq;
using System.Text;
using WodiLib.SourceGenerator.Core.Extensions;

namespace WodiLib.SourceGenerator.Core
{
    /// <summary>
    ///     デフォルトロガークラス
    /// </summary>
    internal class DefaultLogger : ILogger
    {
        /// <summary>ログ出力フラグ</summary>
        /* 解析途中のログが欲しいときは true にする (csproj の設定変更も必要) */
        private bool IsOutputLog => false;

        /// <summary>
        ///     ログ文字列タンク
        /// </summary>
        private StringBuilder Logs { get; } = new();

        /// <inheritdoc/>
        public bool HasLog => Logs.Length > 0;

        /// <inheritdoc/>
        public void Append(string message)
            => Logs.Append(message);

        /// <inheritdoc/>
        public void AppendLine(string message)
            => Append($"/* [{GetHashCode()}] */ {message}{Environment.NewLine}");

        /// <inheritdoc/>
        public void AppendLine(params string[] messages)
        {
            foreach (var message in messages)
            {
                AppendLine(message);
            }
        }

        /// <inheritdoc/>
        public string ToOutputText()
        {
            return IsOutputLog
                ? $"// {string.Join("\n// ", Logs.ToString().Lines(false))}"
                : "";
        }

        /// <inheritdoc/>
        public void AppendLine(ILogger? other)
        {
            if (other is null) return;
            AppendLine(other.ToOutputText().Lines(false).ToArray());
        }
    }
}
