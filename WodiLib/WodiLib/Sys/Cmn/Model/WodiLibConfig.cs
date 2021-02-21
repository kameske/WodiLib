// ========================================
// Project Name : WodiLib
// File Name    : WodiLibConfig.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;

namespace WodiLib.Sys.Cmn
{
    /// <summary>
    ///     WodiLib 全体の設定クラス
    /// </summary>
    public class WodiLibConfig : IWodiLibConfig
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

        /// <summary>
        ///     指定されたキー名で管理される <see cref="WodiLibConfig"/> インスタンスの
        ///     <see cref="IWodiLibConfig.DefaultNotifyBeforePropertyChangeFlag"/> プロパティを設定する。
        /// </summary>
        /// <param name="flag">設定値</param>
        /// <param name="keyName">
        ///     設定対象キー名。<br/>
        ///     <see langword="null"/> の場合 <see cref="TargetKeyName"/> の値を使用する。
        /// </param>
        public static void SetDefaultNotifyBeforePropertyChangeFlag(bool flag, string? keyName = null)
        {
            var config = GetConfig(keyName ?? TargetKeyName);
            config.DefaultNotifyBeforePropertyChangeFlag = flag;
        }

        /// <summary>
        ///     指定されたキー名で管理される <see cref="WodiLibConfig"/> インスタンスの
        ///     <see cref="IWodiLibConfig.DefaultNotifyBeforePropertyChangeFlag"/> プロパティを取得する。
        /// </summary>
        /// <param name="keyName">
        ///     設定対象キー名。<br/>
        ///     <see langword="null"/> の場合 <see cref="TargetKeyName"/> の値を使用する。
        /// </param>
        public static bool GetDefaultNotifyBeforePropertyChangeFlag(string? keyName = null)
        {
            var config = GetConfig(keyName ?? TargetKeyName);
            return config.DefaultNotifyBeforePropertyChangeFlag;
        }

        /// <summary>
        ///     指定されたキー名で管理される <see cref="WodiLibConfig"/> インスタンスの
        ///     <see cref="IWodiLibConfig.DefaultNotifyAfterPropertyChangeFlag"/> プロパティを設定する。
        /// </summary>
        /// <param name="flag">設定値</param>
        /// <param name="keyName">
        ///     設定対象キー名。<br/>
        ///     <see langword="null"/> の場合 <see cref="TargetKeyName"/> の値を使用する。
        /// </param>
        public static void SetDefaultNotifyAfterPropertyChangeFlag(bool flag, string? keyName = null)
        {
            var config = GetConfig(keyName ?? TargetKeyName);
            config.DefaultNotifyAfterPropertyChangeFlag = flag;
        }

        /// <summary>
        ///     指定されたキー名で管理される <see cref="WodiLibConfig"/> インスタンスの
        ///     <see cref="IWodiLibConfig.DefaultNotifyAfterPropertyChangeFlag"/> プロパティを取得する。
        /// </summary>
        /// <param name="keyName">
        ///     設定対象キー名。<br/>
        ///     <see langword="null"/> の場合 <see cref="TargetKeyName"/> の値を使用する。
        /// </param>
        public static bool GetDefaultNotifyAfterPropertyChangeFlag(string? keyName = null)
        {
            var config = GetConfig(keyName ?? TargetKeyName);
            return config.DefaultNotifyAfterPropertyChangeFlag;
        }

        /// <summary>
        ///     指定されたキー名で管理される <see cref="WodiLibConfig"/> インスタンスの
        ///     <see cref="IWodiLibConfig.DefaultNotifyBeforeCollectionChangeFlag"/> プロパティを設定する。
        /// </summary>
        /// <param name="flag">設定値</param>
        /// <param name="keyName">
        ///     設定対象キー名。<br/>
        ///     <see langword="null"/> の場合 <see cref="TargetKeyName"/> の値を使用する。
        /// </param>
        public static void SetDefaultNotifyBeforeCollectionChangeFlag(bool flag, string? keyName = null)
        {
            var config = GetConfig(keyName ?? TargetKeyName);
            config.DefaultNotifyBeforeCollectionChangeFlag = flag;
        }

        /// <summary>
        ///     指定されたキー名で管理される <see cref="WodiLibConfig"/> インスタンスの
        ///     <see cref="IWodiLibConfig.DefaultNotifyBeforeCollectionChangeFlag"/> プロパティを取得する。
        /// </summary>
        /// <param name="keyName">
        ///     設定対象キー名。<br/>
        ///     <see langword="null"/> の場合 <see cref="TargetKeyName"/> の値を使用する。
        /// </param>
        public static bool GetDefaultNotifyBeforeCollectionChangeFlag(string? keyName = null)
        {
            var config = GetConfig(keyName ?? TargetKeyName);
            return config.DefaultNotifyBeforeCollectionChangeFlag;
        }

        /// <summary>
        ///     指定されたキー名で管理される <see cref="WodiLibConfig"/> インスタンスの
        ///     <see cref="IWodiLibConfig.DefaultNotifyAfterCollectionChangeFlag"/> プロパティを設定する。
        /// </summary>
        /// <param name="flag">設定値</param>
        /// <param name="keyName">
        ///     設定対象キー名。<br/>
        ///     <see langword="null"/> の場合 <see cref="TargetKeyName"/> の値を使用する。
        /// </param>
        public static void SetDefaultNotifyAfterCollectionChangeFlag(bool flag, string? keyName = null)
        {
            var config = GetConfig(keyName ?? TargetKeyName);
            config.DefaultNotifyAfterCollectionChangeFlag = flag;
        }

        /// <summary>
        ///     指定されたキー名で管理される <see cref="WodiLibConfig"/> インスタンスの
        ///     <see cref="IWodiLibConfig.DefaultNotifyAfterCollectionChangeFlag"/> プロパティを取得する。
        /// </summary>
        /// <param name="keyName">
        ///     設定対象キー名。<br/>
        ///     <see langword="null"/> の場合 <see cref="TargetKeyName"/> の値を使用する。
        /// </param>
        public static bool GetDefaultNotifyAfterCollectionChangeFlag(string? keyName = null)
        {
            var config = GetConfig(keyName ?? TargetKeyName);
            return config.DefaultNotifyAfterCollectionChangeFlag;
        }

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

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc/>
        public string KeyName { get; }

        /// <inheritdoc/>
        public bool DefaultNotifyBeforePropertyChangeFlag { get; set; }

        /// <inheritdoc/>
        public bool DefaultNotifyAfterPropertyChangeFlag { get; set; }

        /// <inheritdoc/>
        public bool DefaultNotifyBeforeCollectionChangeFlag { get; set; }

        /// <inheritdoc/>
        public bool DefaultNotifyAfterCollectionChangeFlag { get; set; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        static WodiLibConfig()
        {
            ChangeTargetKey(DefaultKeyName);
        }

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        private WodiLibConfig()
        {
            KeyName = "";
            DefaultNotifyBeforePropertyChangeFlag = false;
            DefaultNotifyAfterPropertyChangeFlag = true;
            DefaultNotifyBeforeCollectionChangeFlag = false;
            DefaultNotifyAfterCollectionChangeFlag = true;
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
                ConfigContainer.Register(() => new WodiLibConfig(), WodiLibContainer.Lifetime.Container,
                    keyName);
            }
        }

        /// <summary>
        ///     設定キー名から設定インスタンスを取得する。
        /// </summary>
        /// <param name="keyName">設定キー名</param>
        /// <returns>設定インスタンス</returns>
        private static WodiLibConfig GetConfig(string keyName)
        {
            RegisterConfigInstanceIfNeeded(keyName);
            return ConfigContainer.Resolve<WodiLibConfig>(keyName);
        }
    }
}
