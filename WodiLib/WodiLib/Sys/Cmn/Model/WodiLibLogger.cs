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
    /// WodiLib内で使用するロガークラス
    /// </summary>
    public class WodiLibLogger
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// デフォルト設定キー名
        /// </summary>
        private static string DefaultKeyName => "default";

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Static Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 現在の設定キー名
        /// </summary>
        public static string TargetKeyName { get; private set; } = "";

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Static Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンフィグコンテナ
        /// </summary>
        private static WodiLibContainer ConfigContainer { get; } = new();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// ログハンドラ
        /// </summary>
        public WodiLibLogHandler LogHandler { get; private set; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Static Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// メインで使用する設定キーを変更する。
        /// </summary>
        /// <param name="keyName">設定キー名</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="keyName"/> が <see langword="null"/> の場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="keyName"/> が空文字の場合。
        /// </exception>
        public static void ChangeTargetKey(string keyName)
        {
            ThrowHelper.ValidateArgumentNotNull(keyName is null, nameof(keyName));
            ThrowHelper.ValidateArgumentNotEmpty(keyName.IsEmpty(), nameof(keyName));

            TargetKeyName = keyName;

            RegisterInstanceIfNeeded(keyName);
        }

        /// <summary>
        /// 設定キー名からインスタンスを取得する。
        /// </summary>
        /// <param name="keyName">
        ///     設定キー名<br/>
        ///     <see langword="null"/> の場合、<see cref="TargetKeyName"/> を使用する。
        /// </param>
        /// <returns>設定インスタンス</returns>
        public static WodiLibLogger GetInstance(string? keyName = null)
        {
            var innerKeyName = keyName ?? TargetKeyName;
            RegisterInstanceIfNeeded(innerKeyName);
            return ConfigContainer.Resolve<WodiLibLogger>(innerKeyName);
        }

        /// <summary>
        /// ログハンドラを設定する。
        /// </summary>
        /// <param name="logHandler">ログ出力ハンドラ</param>
        /// <param name="keyName">
        ///     設定キー名<br/>
        ///     <see langword="null"/> の場合、<see cref="TargetKeyName"/> に指定したキー名の設定に対して処理を行う。
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="logHandler"/> が <see langword="null"/> の場合。
        /// </exception>
        public static void SetLogHandler(WodiLibLogHandler logHandler, string? keyName = null)
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
        /// 指定した設定キー名の設定インスタンスがコンテナに登録されていなければ登録する。
        /// </summary>
        /// <param name="keyName">設定キー名</param>
        private static void RegisterInstanceIfNeeded(string keyName)
        {
            if (!ConfigContainer.HasCreateMethod<WodiLibLogger>(keyName))
            {
                ConfigContainer.Register(() => new WodiLibLogger(WodiLibLogHandler.Default),
                    WodiLibContainer.Lifetime.Container, keyName);
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
        /// コンストラクタ
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
        /// エラーメッセージを処理する。
        /// </summary>
        /// <param name="message">メッセージ</param>
        public void Error(string? message)
        {
            LogHandler.DoError(message);
        }

        /// <summary>
        /// 警告メッセージを処理する。
        /// </summary>
        /// <param name="message">メッセージ</param>
        public void Warning(string? message)
        {
            LogHandler.DoWarning(message);
        }

        /// <summary>
        /// 情報メッセージを処理する。
        /// </summary>
        /// <param name="message">メッセージ</param>
        public void Info(string? message)
        {
            LogHandler.DoInfo(message);
        }

        /// <summary>
        /// デバッグメッセージを処理する。
        /// </summary>
        /// <param name="message">メッセージ</param>
        public void Debug(string? message)
        {
            LogHandler.DoDebug(message);
        }
    }
}
