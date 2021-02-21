// ========================================
// Project Name : WodiLib
// File Name    : VersionConfig.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;

namespace WodiLib.Sys.Cmn
{
    /// <summary>
    ///     WodiLib全体のバージョン設定
    /// </summary>
    public class VersionConfig : IVersionConfig
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     デフォルト設定キー名
        /// </summary>
        private static string DefaultKeyName => "default";

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Static Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     現在の設定キー名
        /// </summary>
        /// <remarks>
        ///     キー名の変更は <see cref="ChangeTargetKey"/> メソッドで行う。
        /// </remarks>
        public static string TargetKeyName { get; private set; } = default!;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     設定キー名
        /// </summary>
        public string KeyName { get; }

        /// <summary>
        ///     設定バージョン
        /// </summary>
        public WoditorVersion Version { get; private set; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Static Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     コンフィグコンテナ
        /// </summary>
        private static WodiLibContainer ConfigContainer { get; } = new();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Static Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     メインで使用する設定キーを変更する。
        /// </summary>
        /// <param name="keyName">[NotEmpty] 設定キー名</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="keyName"/> が <see langword="null"/> の場合。
        /// </exception>
        /// <exception cref="ArgumentException"><paramref name="keyName"/> が空文字の場合。</exception>
        public static void ChangeTargetKey(string keyName)
        {
            ThrowHelper.ValidateArgumentNotNull(keyName is null, nameof(keyName));
            ThrowHelper.ValidateArgumentNotEmpty(keyName.IsEmpty(), nameof(keyName));

            TargetKeyName = keyName;

            RegisterConfigInstanceIfNeeded(keyName);
        }

        /// <summary>
        ///     設定したウディタバージョンを取得する。
        /// </summary>
        /// <param name="keyName">
        ///     設定キー名。<br/>
        ///     <see langword="null"/> の場合 <see cref="TargetKeyName"/> の値を使用する。
        /// </param>
        /// <returns>ウディタバージョン</returns>
        public static WoditorVersion GetConfigWoditorVersion(string? keyName = null)
        {
            var config = GetConfig(keyName ?? TargetKeyName);
            return config.Version;
        }

        /// <summary>
        ///     指定されたキー名で管理される <see cref="VersionConfig"/> インスタンスに
        ///     ウディタバージョンをセットする。
        /// </summary>
        /// <param name="version">ウディタバージョン</param>
        /// <param name="keyName">
        ///     設定対象キー名。<br/>
        ///     <see langword="null"/> の場合 <see cref="TargetKeyName"/> の値を使用する。
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="version"/> が <see langword="null"/> の場合。
        /// </exception>
        public static void SetConfigWoditorVersion(WoditorVersion version, string? keyName = null)
        {
            ThrowHelper.ValidateArgumentNotNull(version is null, nameof(version));

            var config = GetConfig(keyName ?? TargetKeyName);
            config.Version = version;
        }

        /// <summary>
        ///     コンフィグに設定されたバージョンが指定されたバージョンより上かどうかを返す。
        /// </summary>
        /// <param name="version">比較バージョン</param>
        /// <param name="keyName">
        ///     設定対象キー名。<br/>
        ///     <see langword="null"/> の場合 <see cref="TargetKeyName"/> の値を使用する。
        /// </param>
        /// <returns>コンフィグ設定バージョン &gt; <paramref name="version"/> の場合 <see langword="true"/></returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="version"/> が <see langword="null"/> の場合。
        /// </exception>
        public static bool IsOverVersion(WoditorVersion version, string? keyName = null)
        {
            ThrowHelper.ValidateArgumentNotNull(version is null, nameof(version));

            var config = GetConfig(keyName ?? TargetKeyName);
            return config.Version > version;
        }

        /// <summary>
        ///     コンフィグに設定されたバージョンが指定されたバージョン以上かどうかを返す。
        /// </summary>
        /// <param name="version">比較バージョン</param>
        /// <param name="keyName">
        ///     設定対象キー名。<br/>
        ///     <see langword="null"/> の場合 <see cref="TargetKeyName"/> の値を使用する。
        /// </param>
        /// <returns>コンフィグ設定バージョン &gt;= <paramref name="version"/> の場合 <see langword="true"/></returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="version"/> が <see langword="null"/> の場合。
        /// </exception>
        public static bool IsGreaterVersion(WoditorVersion version, string? keyName = null)
        {
            ThrowHelper.ValidateArgumentNotNull(version is null, nameof(version));

            var config = GetConfig(keyName ?? TargetKeyName);
            return config.Version >= version;
        }

        /// <summary>
        ///     コンフィグに設定されたバージョンが指定されたバージョン以下かどうかを返す。
        /// </summary>
        /// <param name="version">比較バージョン</param>
        /// <param name="keyName">
        ///     設定対象キー名。<br/>
        ///     <see langword="null"/> の場合 <see cref="TargetKeyName"/> の値を使用する。
        /// </param>
        /// <returns>コンフィグ設定バージョン &lt;= <paramref name="version"/> の場合 <see langword="true"/></returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="version"/> が <see langword="null"/> の場合。
        /// </exception>
        public static bool IsSameVersion(WoditorVersion version, string? keyName = null)
        {
            ThrowHelper.ValidateArgumentNotNull(version is null, nameof(version));

            var config = GetConfig(keyName ?? TargetKeyName);
            return config.Version == version;
        }

        /// <summary>
        ///     コンフィグに設定されたバージョンが指定されたバージョン以下かどうかを返す。
        /// </summary>
        /// <param name="version">比較バージョン</param>
        /// <param name="keyName">
        ///     設定対象キー名。<br/>
        ///     <see langword="null"/> の場合 <see cref="TargetKeyName"/> の値を使用する。
        /// </param>
        /// <returns>コンフィグ設定バージョン &lt;= <paramref name="version"/> の場合 <see langword="true"/></returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="version"/> が <see langword="null"/> の場合。
        /// </exception>
        public static bool IsLessVersion(WoditorVersion version, string? keyName = null)
        {
            ThrowHelper.ValidateArgumentNotNull(version is null, nameof(version));

            var config = GetConfig(keyName ?? TargetKeyName);
            return config.Version <= version;
        }

        /// <summary>
        ///     コンフィグに設定されたバージョンが指定されたバージョン未満かどうかを返す。
        /// </summary>
        /// <param name="version">比較バージョン</param>
        /// <param name="keyName">
        ///     設定対象キー名。<br/>
        ///     <see langword="null"/> の場合 <see cref="TargetKeyName"/> の値を使用する。
        /// </param>
        /// <returns>コンフィグ設定バージョン &lt; <paramref name="version"/> の場合 <see langword="true"/></returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="version"/> が <see langword="null"/> の場合。
        /// </exception>
        public static bool IsUnderVersion(WoditorVersion version, string? keyName = null)
        {
            ThrowHelper.ValidateArgumentNotNull(version is null, nameof(version));

            var config = GetConfig(keyName ?? TargetKeyName);
            return config.Version < version;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Static Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     指定した設定キー名の設定インスタンスがコンテナに登録されていなければ登録する。
        /// </summary>
        /// <param name="keyName">設定キー名</param>
        private static void RegisterConfigInstanceIfNeeded(string keyName)
        {
            if (!ConfigContainer.HasCreateMethod<VersionConfig>(keyName))
            {
                ConfigContainer.Register(() => new VersionConfig(), WodiLibContainer.Lifetime.Container,
                    keyName);
            }
        }

        /// <summary>
        ///     設定キー名から設定インスタンスを取得する。
        /// </summary>
        /// <param name="keyName">設定キー名</param>
        /// <returns>設定インスタンス</returns>
        private static VersionConfig GetConfig(string keyName)
        {
            RegisterConfigInstanceIfNeeded(keyName);
            return ConfigContainer.Resolve<VersionConfig>(keyName);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        static VersionConfig()
        {
            ChangeTargetKey(DefaultKeyName);
        }

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        private VersionConfig()
        {
            KeyName = "";
            Version = WoditorVersion.Default;
        }
    }
}
