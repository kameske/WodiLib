// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : ILogger.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.SourceGenerator.Core
{
    /// <summary>
    ///     WodiLib.SourceGenerator 内で使用するロガーインタフェース
    /// </summary>
    internal interface ILogger
    {
        /// <summary>
        ///     ログ有無
        /// </summary>
        /// <value>ログが存在する場合<see langword="true"/></value>
        public bool HasLog { get; }

        /// <summary>
        ///     新たなログを結合する。
        /// </summary>
        /// <param name="message">ログメッセージ</param>
        public void Append(string message);

        /// <summary>
        ///     新たなログを結合し、改行する。
        /// </summary>
        /// <param name="message">ログメッセージ</param>
        public void AppendLine(string message);

        /// <summary>
        ///     複数行のログを結合し、改行する。
        /// </summary>
        /// <param name="messages">ログメッセージ</param>
        public void AppendLine(params string[] messages);

        /// <summary>
        ///     他の <see cref="ILogger"/> の内容を結合する。
        /// </summary>
        /// <param name="other">結合対象</param>
        public void AppendLine(ILogger? other);

        /// <summary>
        ///     ログ出力文字列に変換する。
        /// </summary>
        /// <returns>ログ出力文字列</returns>
        public string ToOutputText();
    }
}
