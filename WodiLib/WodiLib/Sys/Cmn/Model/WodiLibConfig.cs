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
    public class WodiLibConfig : IWodiLibConfig, IContainerCreatable
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
        public static WodiLibContainerKeyName TargetKeyName => WodiLibContainer.TargetKeyName;

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
            NotifyPropertyChangeEventType type, WodiLibContainerKeyName? keyName = null)
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
            WodiLibContainerKeyName? keyName = null)
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
            NotifyPropertyChangeEventType type, WodiLibContainerKeyName? keyName = null)
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
            WodiLibContainerKeyName? keyName = null)
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
            NotifyCollectionChangeEventType type, WodiLibContainerKeyName? keyName = null)
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
            WodiLibContainerKeyName? keyName = null)
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
            NotifyCollectionChangeEventType type, WodiLibContainerKeyName? keyName = null)
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
            WodiLibContainerKeyName? keyName = null)
        {
            var config = GetConfig(keyName ?? TargetKeyName);
            return config.DefaultNotifyAfterCollectionChangeEventType;
        }

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
        public static void ChangeTargetKey(WodiLibContainerKeyName keyName)
            => WodiLibContainer.ChangeTargetKey(keyName);

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Static Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     指定した設定キー名の設定インスタンスがコンテナに登録されていなければ登録する。
        /// </summary>
        /// <param name="keyName">設定キー名</param>
        private static void RegisterConfigInstanceIfNeeded(WodiLibContainerKeyName keyName)
        {
            if (!WodiLibContainer.HasCreateMethod<WodiLibConfig>(keyName))
            {
                WodiLibContainer.Register(() => new WodiLibConfig(keyName), WodiLibContainer.Lifetime.Container,
                    keyName);
            }
        }

        /// <summary>
        ///     設定キー名から設定インスタンスを取得する。
        /// </summary>
        /// <param name="keyName">設定キー名</param>
        /// <returns>設定インスタンス</returns>
        private static WodiLibConfig GetConfig(WodiLibContainerKeyName keyName)
        {
            RegisterConfigInstanceIfNeeded(keyName);
            return WodiLibContainer.Resolve<WodiLibConfig>(keyName);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc/>
        public WodiLibContainerKeyName KeyName { get; }

        /// <inheritdoc/>
        public WodiLibContainerKeyName? ContainerKeyName { get; set; }

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

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Fields
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private NotifyPropertyChangeEventType defaultNotifyBeforePropertyChangeEventType;
        private NotifyPropertyChangeEventType defaultNotifyAfterPropertyChangeEventType;
        private NotifyCollectionChangeEventType defaultNotifyBeforeCollectionChangeEventType;
        private NotifyCollectionChangeEventType defaultNotifyAfterCollectionChangeEventType;

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
        private WodiLibConfig(WodiLibContainerKeyName keyName)
        {
            KeyName = keyName;
            defaultNotifyBeforePropertyChangeEventType = NotifyPropertyChangeEventType.Disabled;
            defaultNotifyAfterPropertyChangeEventType = NotifyPropertyChangeEventType.Enabled;
            defaultNotifyBeforeCollectionChangeEventType = NotifyCollectionChangeEventType.None;
            defaultNotifyAfterCollectionChangeEventType = NotifyCollectionChangeEventType.Single;
        }
    }
}
