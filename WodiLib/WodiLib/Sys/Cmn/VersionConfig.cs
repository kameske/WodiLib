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
    /// WodiLib全体のバージョン設定
    /// </summary>
    [Serializable]
    public class VersionConfig
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
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
        public static string TargetKeyName { get; private set; }


        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 設定キー名
        /// </summary>
        public string KeyName { get; }

        /// <summary>
        /// 設定バージョン
        /// </summary>
        public WoditorVersion Version { get; private set; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Static Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// メインで使用する設定キーを変更する。
        /// </summary>
        /// <param name="keyName">[NotNullOrEmpty] 設定キー名</param>
        /// <exception cref="ArgumentNullException">keyNameがnullの場合</exception>
        /// <exception cref="ArgumentException">keyNameが空文字の場合</exception>
        public static void ChangeTargetKey(string keyName)
        {
            if (keyName is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(keyName)));
            if (keyName.IsEmpty())
                throw new ArgumentException(
                    ErrorMessage.NotEmpty(nameof(keyName)));

            TargetKeyName = keyName;

            RegisterConfigInstanceIfNeeded(keyName);
        }

        /// <summary>
        /// 設定したウディタバージョンを取得する。
        /// </summary>
        /// <param name="keyName">[Nullable] 設定キー名</param>
        /// <returns>ウディタバージョン</returns>
        public static WoditorVersion GetConfigWoditorVersion(string keyName = null)
        {
            var config = GetConfig(keyName ?? TargetKeyName);
            return config.Version;
        }

        /// <summary>
        /// ウディタバージョンをセットする。
        /// <para>keyNameがnullの場合、TargetKeyNameに指定したキー名の設定に対して処理を行う。</para>
        /// </summary>
        /// <param name="version">[NotNull] ウディタバージョン</param>
        /// <param name="keyName">[Nullable] 設定キー名</param>
        /// <exception cref="ArgumentNullException">versionがnullの場合</exception>
        public static void SetConfigWoditorVersion(WoditorVersion version, string keyName = null)
        {
            if (version is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(version)));
            var config = GetConfig(keyName ?? TargetKeyName);
            config.Version = version;
        }

        /// <summary>
        /// コンフィグに設定されたバージョンが指定されたバージョンより上かどうかを返す。
        /// <para>keyNameがnullの場合、TargetKeyNameに指定したキー名の設定に対して処理を行う。</para>
        /// </summary>
        /// <param name="version">[NotNull] 比較バージョン</param>
        /// <param name="keyName">[Nullable] 設定キー名</param>
        /// <returns>コンフィグ設定バージョン &gt; version の場合true</returns>
        /// <exception cref="ArgumentNullException">versionがnullの場合</exception>
        public static bool IsOverVersion(WoditorVersion version, string keyName = null)
        {
            if (version is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(version)));

            var config = GetConfig(keyName ?? TargetKeyName);
            return config.Version > version;
        }

        /// <summary>
        /// コンフィグに設定されたバージョンが指定されたバージョン以上かどうかを返す。
        /// <para>keyNameがnullの場合、TargetKeyNameに指定したキー名の設定に対して処理を行う。</para>
        /// </summary>
        /// <param name="version">[NotNull] 比較バージョン</param>
        /// <param name="keyName">[Nullable] 設定キー名</param>
        /// <returns>コンフィグ設定バージョン &gt;= version の場合true</returns>
        /// <exception cref="ArgumentNullException">versionがnullの場合</exception>
        public static bool IsGreaterVersion(WoditorVersion version, string keyName = null)
        {
            if (version is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(version)));

            var config = GetConfig(keyName ?? TargetKeyName);
            return config.Version >= version;
        }

        /// <summary>
        /// コンフィグに設定されたバージョンが指定されたバージョン以下かどうかを返す。
        /// <para>keyNameがnullの場合、TargetKeyNameに指定したキー名の設定に対して処理を行う。</para>
        /// </summary>
        /// <param name="version">[NotNull] 比較バージョン</param>
        /// <param name="keyName">[Nullable] 設定キー名</param>
        /// <returns>コンフィグ設定バージョン &lt;= version の場合true</returns>
        /// <exception cref="ArgumentNullException">versionがnullの場合</exception>
        public static bool IsSameVersion(WoditorVersion version, string keyName = null)
        {
            if (version is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(version)));

            var config = GetConfig(keyName ?? TargetKeyName);
            return config.Version == version;
        }

        /// <summary>
        /// コンフィグに設定されたバージョンが指定されたバージョン以下かどうかを返す。
        /// <para>keyNameがnullの場合、TargetKeyNameに指定したキー名の設定に対して処理を行う。</para>
        /// </summary>
        /// <param name="version">[NotNull] 比較バージョン</param>
        /// <param name="keyName">[Nullable] 設定キー名</param>
        /// <returns>コンフィグ設定バージョン &lt;= version の場合true</returns>
        /// <exception cref="ArgumentNullException">versionがnullの場合</exception>
        public static bool IsLessVersion(WoditorVersion version, string keyName = null)
        {
            if (version is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(version)));

            var config = GetConfig(keyName ?? TargetKeyName);
            return config.Version <= version;
        }

        /// <summary>
        /// コンフィグに設定されたバージョンが指定されたバージョン未満かどうかを返す。
        /// <para>keyNameがnullの場合、TargetKeyNameに指定したキー名の設定に対して処理を行う。</para>
        /// </summary>
        /// <param name="version">[NotNull] 比較バージョン</param>
        /// <param name="keyName">[Nullable] 設定キー名</param>
        /// <returns>コンフィグ設定バージョン &lt; version の場合true</returns>
        /// <exception cref="ArgumentNullException">versionがnullの場合</exception>
        public static bool IsUnderVersion(WoditorVersion version, string keyName = null)
        {
            if (version is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(version)));

            var config = GetConfig(keyName ?? TargetKeyName);
            return config.Version < version;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Static Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 指定した設定キー名の設定インスタンスがコンテナに登録されていなければ登録する。
        /// </summary>
        /// <param name="keyName">[NotNull] 設定キー名</param>
        private static void RegisterConfigInstanceIfNeeded(string keyName)
        {
            if (!WodiLibContainer.HasCreateMethod<VersionConfig>(keyName))
            {
                WodiLibContainer.Register(() => new VersionConfig(), WodiLibContainer.Lifetime.Container,
                    keyName);
            }
        }

        /// <summary>
        /// 設定キー名から設定インスタンスを取得する。
        /// </summary>
        /// <param name="keyName">設定キー名</param>
        /// <returns>設定インスタンス</returns>
        private static VersionConfig GetConfig(string keyName)
        {
            RegisterConfigInstanceIfNeeded(keyName);
            return WodiLibContainer.Resolve<VersionConfig>(keyName);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        static VersionConfig()
        {
            ChangeTargetKey(DefaultKeyName);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        private VersionConfig()
        {
            KeyName = "";
            Version = WoditorVersion.Default;
        }
    }
}