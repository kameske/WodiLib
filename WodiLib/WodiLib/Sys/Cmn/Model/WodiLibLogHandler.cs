// ========================================
// Project Name : WodiLib
// File Name    : WodiLibLogHandler.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;

namespace WodiLib.Sys.Cmn
{
    /// <summary>
    ///     ログ出力処理ハンドラクラス
    /// </summary>
    public class WodiLibLogHandler
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Static Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>デフォルトハンドラ</summary>
        public static readonly WodiLibLogHandler Default;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     private Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>エラーメッセージ処理</summary>
        private Action<string?>? ErrorAction { get; }

        /// <summary>警告メッセージ処理</summary>
        private Action<string?>? WarningAction { get; }

        /// <summary>情報メッセージ処理</summary>
        private Action<string?>? InfoAction { get; }

        /// <summary>デバッグメッセージ処理</summary>
        private Action<string?>? DebugAction { get; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        static WodiLibLogHandler()
        {
            Default = new WodiLibLogHandler(
                Console.WriteLine,
                Console.WriteLine,
                Console.WriteLine
            );
        }

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="errorAction">エラーメッセージ処理</param>
        /// <param name="warningAction">警告メッセージ処理</param>
        /// <param name="infoAction">情報メッセージ処理</param>
        /// <param name="debugAction">デバッグメッセージ処理</param>
        public WodiLibLogHandler(Action<string?>? errorAction = null, Action<string?>? warningAction = null,
            Action<string?>? infoAction = null, Action<string?>? debugAction = null)
        {
            ErrorAction = errorAction;
            WarningAction = warningAction;
            InfoAction = infoAction;
            DebugAction = debugAction;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     エラーメッセージを処理する。
        /// </summary>
        /// <param name="message">メッセージ</param>
        internal void DoError(string? message)
            => ErrorAction?.Invoke(message);

        /// <summary>
        ///     警告メッセージを処理する。
        /// </summary>
        /// <param name="message">メッセージ</param>
        internal void DoWarning(string? message)
            => WarningAction?.Invoke(message);

        /// <summary>
        ///     情報メッセージを処理する。
        /// </summary>
        /// <param name="message">メッセージ</param>
        internal void DoInfo(string? message)
            => InfoAction?.Invoke(message);

        /// <summary>
        ///     デバッグメッセージを処理する。
        /// </summary>
        /// <param name="message">メッセージ</param>
        internal void DoDebug(string? message)
            => DebugAction?.Invoke(message);
    }
}
