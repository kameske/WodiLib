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
    public class VersionConfig : IVersionConfig,
        IContainerCreatable
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Static Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     現在の設定キー名
        /// </summary>
        /// <remarks>
        ///     キー名の変更は <see cref="ChangeTargetKey"/> メソッドで行う。
        /// </remarks>
        public static WodiLibContainerKeyName TargetKeyName => WodiLibContainer.TargetKeyName;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     設定キー名
        /// </summary>
        public WodiLibContainerKeyName KeyName { get; }

        /// <summary>
        ///     設定バージョン
        /// </summary>
        public WoditorVersion Version { get; private set; }

        /// <inheritdoc/>
        public WodiLibContainerKeyName? ContainerKeyName { get; set; }

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
        public static void ChangeTargetKey(WodiLibContainerKeyName keyName)
            => WodiLibContainer.ChangeTargetKey(keyName);

        /// <summary>
        ///     設定したウディタバージョンを取得する。
        /// </summary>
        /// <param name="keyName">
        ///     設定キー名。<br/>
        ///     <see langword="null"/> の場合 <see cref="TargetKeyName"/> の値を使用する。
        /// </param>
        /// <returns>ウディタバージョン</returns>
        public static WoditorVersion GetConfigWoditorVersion(WodiLibContainerKeyName? keyName = null)
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
        public static void SetConfigWoditorVersion(WoditorVersion version, WodiLibContainerKeyName? keyName = null)
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
        public static bool IsOverVersion(WoditorVersion version, WodiLibContainerKeyName? keyName = null)
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
        public static bool IsGreaterVersion(WoditorVersion version, WodiLibContainerKeyName? keyName = null)
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
        public static bool IsSameVersion(WoditorVersion version, WodiLibContainerKeyName? keyName = null)
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
        public static bool IsLessVersion(WoditorVersion version, WodiLibContainerKeyName? keyName = null)
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
        public static bool IsUnderVersion(WoditorVersion version, WodiLibContainerKeyName? keyName = null)
        {
            ThrowHelper.ValidateArgumentNotNull(version is null, nameof(version));

            var config = GetConfig(keyName ?? TargetKeyName);
            return config.Version < version;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Static Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     設定キー名から設定インスタンスを取得する。
        /// </summary>
        /// <param name="keyName">設定キー名</param>
        /// <returns>設定インスタンス</returns>
        private static VersionConfig GetConfig(WodiLibContainerKeyName keyName)
        {
            WodiLibContainer.RegisterIfNotHas(
                () => new VersionConfig(keyName),
                WodiLibContainer.Lifetime.Container,
                keyName
            );
            return WodiLibContainer.Resolve<VersionConfig>(keyName);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        private VersionConfig(WodiLibContainerKeyName keyName)
        {
            KeyName = keyName;
            Version = WoditorVersion.Default;
        }
    }
}
