// ========================================
// Project Name : WodiLib
// File Name    : WodiLibConfig.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using WodiLib.Sys.Collections;

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
        ///     <see cref="IWodiLibConfig.DefaultNotifyBeforePropertyChangeEventType"/> プロパティを設定する。
        /// </summary>
        /// <param name="type">設定値</param>
        /// <param name="keyName">
        ///     設定対象キー名。<br/>
        ///     <see langword="null"/> の場合 <see cref="TargetKeyName"/> の値を使用する。
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="type"/> が <see langword="null"/> の場合。
        /// </exception>
        public static void SetDefaultNotifyBeforePropertyChangeEventType(
            NotifyPropertyChangeEventType type, string? keyName = null)
        {
            ThrowHelper.ValidateArgumentNotNull(type is null, nameof(type));

            var config = GetConfig(keyName ?? TargetKeyName);
            config.DefaultNotifyBeforePropertyChangeEventType = type;
        }

        /// <summary>
        ///     指定されたキー名で管理される <see cref="WodiLibConfig"/> インスタンスの
        ///     <see cref="IWodiLibConfig.DefaultNotifyBeforePropertyChangeEventType"/> プロパティを取得する。
        /// </summary>
        /// <param name="keyName">
        ///     設定対象キー名。<br/>
        ///     <see langword="null"/> の場合 <see cref="TargetKeyName"/> の値を使用する。
        /// </param>
        public static NotifyPropertyChangeEventType GetDefaultNotifyBeforePropertyChangeEventType(
            string? keyName = null)
        {
            var config = GetConfig(keyName ?? TargetKeyName);
            return config.DefaultNotifyBeforePropertyChangeEventType;
        }

        /// <summary>
        ///     指定されたキー名で管理される <see cref="WodiLibConfig"/> インスタンスの
        ///     <see cref="IWodiLibConfig.DefaultNotifyAfterPropertyChangeEventType"/> プロパティを設定する。
        /// </summary>
        /// <param name="type">設定値</param>
        /// <param name="keyName">
        ///     設定対象キー名。<br/>
        ///     <see langword="null"/> の場合 <see cref="TargetKeyName"/> の値を使用する。
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="type"/> が <see langword="null"/> の場合。
        /// </exception>
        public static void SetDefaultNotifyAfterPropertyChangeEventType(
            NotifyPropertyChangeEventType type, string? keyName = null)
        {
            ThrowHelper.ValidateArgumentNotNull(type is null, nameof(type));

            var config = GetConfig(keyName ?? TargetKeyName);
            config.DefaultNotifyAfterPropertyChangeEventType = type;
        }

        /// <summary>
        ///     指定されたキー名で管理される <see cref="WodiLibConfig"/> インスタンスの
        ///     <see cref="IWodiLibConfig.DefaultNotifyAfterPropertyChangeEventType"/> プロパティを取得する。
        /// </summary>
        /// <param name="keyName">
        ///     設定対象キー名。<br/>
        ///     <see langword="null"/> の場合 <see cref="TargetKeyName"/> の値を使用する。
        /// </param>
        public static NotifyPropertyChangeEventType GetDefaultNotifyAfterPropertyChangeEventType(
            string? keyName = null)
        {
            var config = GetConfig(keyName ?? TargetKeyName);
            return config.DefaultNotifyAfterPropertyChangeEventType;
        }

        /// <summary>
        ///     指定されたキー名で管理される <see cref="WodiLibConfig"/> インスタンスの
        ///     <see cref="IWodiLibConfig.DefaultNotifyBeforeCollectionChangeEventType"/> プロパティを設定する。
        /// </summary>
        /// <param name="type">設定値</param>
        /// <param name="keyName">
        ///     設定対象キー名。<br/>
        ///     <see langword="null"/> の場合 <see cref="TargetKeyName"/> の値を使用する。
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="type"/> が <see langword="null"/> の場合。
        /// </exception>
        public static void SetDefaultNotifyBeforeCollectionChangeFlag(
            NotifyCollectionChangeEventType type, string? keyName = null)
        {
            ThrowHelper.ValidateArgumentNotNull(type is null, nameof(type));

            var config = GetConfig(keyName ?? TargetKeyName);
            config.DefaultNotifyBeforeCollectionChangeEventType = type;
        }

        /// <summary>
        ///     指定されたキー名で管理される <see cref="WodiLibConfig"/> インスタンスの
        ///     <see cref="IWodiLibConfig.DefaultNotifyBeforeCollectionChangeEventType"/> プロパティを取得する。
        /// </summary>
        /// <param name="keyName">
        ///     設定対象キー名。<br/>
        ///     <see langword="null"/> の場合 <see cref="TargetKeyName"/> の値を使用する。
        /// </param>
        public static NotifyCollectionChangeEventType GetDefaultNotifyBeforeCollectionChangeEventType(
            string? keyName = null)
        {
            var config = GetConfig(keyName ?? TargetKeyName);
            return config.DefaultNotifyBeforeCollectionChangeEventType;
        }

        /// <summary>
        ///     指定されたキー名で管理される <see cref="WodiLibConfig"/> インスタンスの
        ///     <see cref="IWodiLibConfig.DefaultNotifyAfterCollectionChangeEventType"/> プロパティを設定する。
        /// </summary>
        /// <param name="type">設定値</param>
        /// <param name="keyName">
        ///     設定対象キー名。<br/>
        ///     <see langword="null"/> の場合 <see cref="TargetKeyName"/> の値を使用する。
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="type"/> が <see langword="null"/> の場合。
        /// </exception>
        public static void SetDefaultNotifyAfterCollectionChangeEventType(
            NotifyCollectionChangeEventType type, string? keyName = null)
        {
            var config = GetConfig(keyName ?? TargetKeyName);
            config.DefaultNotifyAfterCollectionChangeEventType = type;
        }

        /// <summary>
        ///     指定されたキー名で管理される <see cref="WodiLibConfig"/> インスタンスの
        ///     <see cref="IWodiLibConfig.DefaultNotifyAfterCollectionChangeEventType"/> プロパティを取得する。
        /// </summary>
        /// <param name="keyName">
        ///     設定対象キー名。<br/>
        ///     <see langword="null"/> の場合 <see cref="TargetKeyName"/> の値を使用する。
        /// </param>
        public static NotifyCollectionChangeEventType GetDefaultNotifyAfterCollectionChangeEventType(
            string? keyName = null)
        {
            var config = GetConfig(keyName ?? TargetKeyName);
            return config.DefaultNotifyAfterCollectionChangeEventType;
        }

        /// <summary>
        ///     指定されたキー名で管理される <see cref="WodiLibConfig"/> インスタンスの
        ///     <see cref="IWodiLibConfig.DefaultNotifyBeforeTwoDimensionalListChangeEventType"/> プロパティを設定する。
        /// </summary>
        /// <param name="type">設定値</param>
        /// <param name="keyName">
        ///     設定対象キー名。<br/>
        ///     <see langword="null"/> の場合 <see cref="TargetKeyName"/> の値を使用する。
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="type"/> が <see langword="null"/> の場合。
        /// </exception>
        public static void SetDefaultNotifyBeforeCollectionChangeFlag(
            NotifyTwoDimensionalListChangeEventType type, string? keyName = null)
        {
            ThrowHelper.ValidateArgumentNotNull(type is null, nameof(type));

            var config = GetConfig(keyName ?? TargetKeyName);
            config.DefaultNotifyBeforeTwoDimensionalListChangeEventType = type;
        }

        /// <summary>
        ///     指定されたキー名で管理される <see cref="WodiLibConfig"/> インスタンスの
        ///     <see cref="IWodiLibConfig.DefaultNotifyBeforeTwoDimensionalListChangeEventType"/> プロパティを取得する。
        /// </summary>
        /// <param name="keyName">
        ///     設定対象キー名。<br/>
        ///     <see langword="null"/> の場合 <see cref="TargetKeyName"/> の値を使用する。
        /// </param>
        public static NotifyTwoDimensionalListChangeEventType GetDefaultNotifyBeforeTwoDimensionalListChangeEventType(
            string? keyName = null)
        {
            var config = GetConfig(keyName ?? TargetKeyName);
            return config.DefaultNotifyBeforeTwoDimensionalListChangeEventType;
        }

        /// <summary>
        ///     指定されたキー名で管理される <see cref="WodiLibConfig"/> インスタンスの
        ///     <see cref="IWodiLibConfig.DefaultNotifyAfterTwoDimensionalListChangeEventType"/> プロパティを設定する。
        /// </summary>
        /// <param name="type">設定値</param>
        /// <param name="keyName">
        ///     設定対象キー名。<br/>
        ///     <see langword="null"/> の場合 <see cref="TargetKeyName"/> の値を使用する。
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="type"/> が <see langword="null"/> の場合。
        /// </exception>
        public static void SetDefaultNotifyAfterTwoDimensionalListChangeEventType(
            NotifyTwoDimensionalListChangeEventType type, string? keyName = null)
        {
            var config = GetConfig(keyName ?? TargetKeyName);
            config.DefaultNotifyAfterTwoDimensionalListChangeEventType = type;
        }

        /// <summary>
        ///     指定されたキー名で管理される <see cref="WodiLibConfig"/> インスタンスの
        ///     <see cref="IWodiLibConfig.DefaultNotifyAfterTwoDimensionalListChangeEventType"/> プロパティを取得する。
        /// </summary>
        /// <param name="keyName">
        ///     設定対象キー名。<br/>
        ///     <see langword="null"/> の場合 <see cref="TargetKeyName"/> の値を使用する。
        /// </param>
        public static NotifyTwoDimensionalListChangeEventType GetDefaultNotifyAfterTwoDimensionalListChangeEventType(
            string? keyName = null)
        {
            var config = GetConfig(keyName ?? TargetKeyName);
            return config.DefaultNotifyAfterTwoDimensionalListChangeEventType;
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
        public NotifyPropertyChangeEventType DefaultNotifyBeforePropertyChangeEventType
        {
            get => defaultNotifyBeforePropertyChangeEventType;
            set
            {
                ThrowHelper.ValidatePropertyNotNull(value is null, nameof(DefaultNotifyBeforePropertyChangeEventType));
                defaultNotifyBeforePropertyChangeEventType = value;
            }
        }


        /// <inheritdoc/>
        public NotifyPropertyChangeEventType DefaultNotifyAfterPropertyChangeEventType
        {
            get => defaultNotifyAfterPropertyChangeEventType;
            set
            {
                ThrowHelper.ValidatePropertyNotNull(value is null, nameof(DefaultNotifyAfterPropertyChangeEventType));
                defaultNotifyAfterPropertyChangeEventType = value;
            }
        }

        /// <inheritdoc/>
        public NotifyCollectionChangeEventType DefaultNotifyBeforeCollectionChangeEventType
        {
            get => defaultNotifyBeforeCollectionChangeEventType;
            set
            {
                ThrowHelper.ValidatePropertyNotNull(value is null,
                    nameof(DefaultNotifyBeforeCollectionChangeEventType));
                defaultNotifyBeforeCollectionChangeEventType = value;
            }
        }

        /// <inheritdoc/>
        public NotifyCollectionChangeEventType DefaultNotifyAfterCollectionChangeEventType
        {
            get => defaultNotifyAfterCollectionChangeEventType;
            set
            {
                ThrowHelper.ValidatePropertyNotNull(value is null, nameof(DefaultNotifyAfterCollectionChangeEventType));
                defaultNotifyAfterCollectionChangeEventType = value;
            }
        }

        /// <inheritdoc/>
        public NotifyTwoDimensionalListChangeEventType DefaultNotifyBeforeTwoDimensionalListChangeEventType
        {
            get => defaultNotifyBeforeTwoDimensionalListChangeEventType;
            set
            {
                ThrowHelper.ValidatePropertyNotNull(value is null,
                    nameof(DefaultNotifyBeforeTwoDimensionalListChangeEventType));
                defaultNotifyBeforeTwoDimensionalListChangeEventType = value;
            }
        }

        /// <inheritdoc/>
        public NotifyTwoDimensionalListChangeEventType DefaultNotifyAfterTwoDimensionalListChangeEventType
        {
            get => defaultNotifyAfterTwoDimensionalListChangeEventType;
            set
            {
                ThrowHelper.ValidatePropertyNotNull(value is null,
                    nameof(DefaultNotifyAfterTwoDimensionalListChangeEventType));
                defaultNotifyAfterTwoDimensionalListChangeEventType = value;
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Fields
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private NotifyPropertyChangeEventType defaultNotifyBeforePropertyChangeEventType;
        private NotifyPropertyChangeEventType defaultNotifyAfterPropertyChangeEventType;
        private NotifyCollectionChangeEventType defaultNotifyBeforeCollectionChangeEventType;
        private NotifyCollectionChangeEventType defaultNotifyAfterCollectionChangeEventType;
        private NotifyTwoDimensionalListChangeEventType defaultNotifyBeforeTwoDimensionalListChangeEventType;
        private NotifyTwoDimensionalListChangeEventType defaultNotifyAfterTwoDimensionalListChangeEventType;

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
            defaultNotifyBeforePropertyChangeEventType = NotifyPropertyChangeEventType.Disabled;
            defaultNotifyAfterPropertyChangeEventType = NotifyPropertyChangeEventType.Enabled;
            defaultNotifyBeforeCollectionChangeEventType = NotifyCollectionChangeEventType.None;
            defaultNotifyAfterCollectionChangeEventType = NotifyCollectionChangeEventType.Single;
            defaultNotifyBeforeTwoDimensionalListChangeEventType = NotifyTwoDimensionalListChangeEventType.None;
            defaultNotifyAfterTwoDimensionalListChangeEventType = NotifyTwoDimensionalListChangeEventType.Single;
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
