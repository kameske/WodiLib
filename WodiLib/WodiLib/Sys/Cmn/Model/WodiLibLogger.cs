// ========================================
// Project Name : WodiLib
// File Name    : WodiLibLogger.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;

namespace WodiLib.Sys.Cmn
{
    /// <summary>
    ///     WodiLib内で使用するロガークラス
    /// </summary>
    public class WodiLibLogger : IContainerCreatable
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     デフォルト設定キー名
        /// </summary>
        private static WodiLibContainerKeyName DefaultKeyName => WodiLibContainer.DefaultKeyName;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Static Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     現在の設定キー名
        /// </summary>
        public static WodiLibContainerKeyName TargetKeyName => WodiLibContainer.TargetKeyName;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc/>
        public WodiLibContainerKeyName? ContainerKeyName { get; set; }

        /// <summary>
        ///     ログハンドラ
        /// </summary>
        public WodiLibLogHandler LogHandler { get; private set; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Static Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     メインで使用する設定キーを変更する。
        /// </summary>
        /// <param name="keyName">設定キー名</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="keyName"/> が <see langword="null"/> の場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="keyName"/> が空文字の場合。
        /// </exception>
        public static void ChangeTargetKey(WodiLibContainerKeyName keyName)
            => WodiLibContainer.ChangeTargetKey(keyName);

        /// <summary>
        ///     設定キー名からインスタンスを取得する。
        /// </summary>
        /// <param name="keyName">
        ///     設定キー名<br/>
        ///     <see langword="null"/> の場合、<see cref="TargetKeyName"/> を使用する。
        /// </param>
        /// <returns>設定インスタンス</returns>
        public static WodiLibLogger GetInstance(WodiLibContainerKeyName? keyName = null)
        {
            var innerKeyName = keyName ?? TargetKeyName;
            RegisterInstanceIfNeeded(innerKeyName);
            return WodiLibContainer.Resolve<WodiLibLogger>(innerKeyName);
        }

        /// <summary>
        ///     ログハンドラを設定する。
        /// </summary>
        /// <param name="logHandler">ログ出力ハンドラ</param>
        /// <param name="keyName">
        ///     設定キー名<br/>
        ///     <see langword="null"/> の場合、<see cref="TargetKeyName"/> に指定したキー名の設定に対して処理を行う。
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="logHandler"/> が <see langword="null"/> の場合。
        /// </exception>
        public static void SetLogHandler(WodiLibLogHandler logHandler, WodiLibContainerKeyName? keyName = null)
        {
            ThrowHelper.ValidateArgumentNotNull(logHandler is null, nameof(logHandler));

            var innerKeyName = keyName ?? TargetKeyName;
            var instance = GetInstance(innerKeyName);
            instance.LogHandler = logHandler;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Static Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     指定した設定キー名の設定インスタンスがコンテナに登録されていなければ登録する。
        /// </summary>
        /// <param name="keyName">設定キー名</param>
        private static void RegisterInstanceIfNeeded(WodiLibContainerKeyName keyName)
        {
            if (!WodiLibContainer.HasCreateMethod<WodiLibLogger>(keyName))
            {
                WodiLibContainer.Register(
                    () => new WodiLibLogger(WodiLibLogHandler.Default),
                    WodiLibContainer.Lifetime.Container,
                    keyName
                );
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        static WodiLibLogger()
        {
            ChangeTargetKey(DefaultKeyName);
        }

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="logHandler">ログハンドラ</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="logHandler"/> が <see langword="null"/> の場合。
        /// </exception>
        public WodiLibLogger(WodiLibLogHandler logHandler)
        {
            ThrowHelper.ValidateArgumentNotNull(logHandler is null, nameof(logHandler));

            LogHandler = logHandler;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     エラーメッセージを処理する。
        /// </summary>
        /// <param name="message">メッセージ</param>
        public void Error(string? message)
        {
            LogHandler.DoError(message);
        }

        /// <summary>
        ///     警告メッセージを処理する。
        /// </summary>
        /// <param name="message">メッセージ</param>
        public void Warning(string? message)
        {
            LogHandler.DoWarning(message);
        }

        /// <summary>
        ///     情報メッセージを処理する。
        /// </summary>
        /// <param name="message">メッセージ</param>
        public void Info(string? message)
        {
            LogHandler.DoInfo(message);
        }

        /// <summary>
        ///     デバッグメッセージを処理する。
        /// </summary>
        /// <param name="message">メッセージ</param>
        public void Debug(string? message)
        {
            LogHandler.DoDebug(message);
        }
    }
}
