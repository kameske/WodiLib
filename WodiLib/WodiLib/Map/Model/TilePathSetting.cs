// ========================================
// Project Name : WodiLib
// File Name    : TilePathSetting.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.ComponentModel;
using WodiLib.Sys;

namespace WodiLib.Map
{
    /// <summary>
    /// タイル通行許可設定
    /// </summary>
    [Serializable]
    public class TilePathSetting : ModelBase<TilePathSetting>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// タイル通行許可
        /// </summary>
        public TilePathPermission PathPermission => InnerSetting.PathPermission;

        /// <summary>
        /// タイル通行不可フラグ
        /// </summary>
        /// <exception cref="PropertyAccessException">タイル通行許可が"通行不可"以外の場合</exception>
        public TileImpassableFlags ImpassableFlags => InnerSetting.ImpassableFlags;

        /// <summary>
        /// [NotNull] タイル通行設定オプション
        /// </summary>
        /// <exception cref="PropertyNullException">nullを設定した場合</exception>
        public TilePathOption PathOption
        {
            get => InnerSetting.PathOption;
            set => InnerSetting.PathOption = value;
        }

        /// <summary>
        /// タイル通行方向設定
        /// </summary>
        /// <exception cref="PropertyAccessException">タイル通行許可が"通行不可"の場合</exception>
        public TileCannotPassingFlags CannotPassingFlags => InnerSetting.CannotPassingFlags;

        /// <summary>
        /// カウンター属性
        /// </summary>
        public bool IsCounter
        {
            get => InnerSetting.IsCounter;
            set => InnerSetting.IsCounter = value;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private ITilePathSetting innerSetting = new TilePathSettingAllow();

        private ITilePathSetting InnerSetting
        {
            get => innerSetting;
            set
            {
                innerSetting = value;
                NotifyPropertyChanged(nameof(PathPermission));
                NotifyPropertyChanged(nameof(ImpassableFlags));
                NotifyPropertyChanged(nameof(PathOption));
                NotifyPropertyChanged(nameof(CannotPassingFlags));
                NotifyPropertyChanged(nameof(IsCounter));
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     InnerNotifyChanged
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// タイル通行許可設定プロパティ変更通知
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="args">情報</param>
        private void OnInnerSettingPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            switch (args.PropertyName)
            {
                case nameof(ITilePathSetting.PathPermission):
                case nameof(ITilePathSetting.ImpassableFlags):
                case nameof(ITilePathSetting.PathOption):
                case nameof(ITilePathSetting.CannotPassingFlags):
                case nameof(ITilePathSetting.IsCounter):
                    NotifyPropertyChanged(args.PropertyName);
                    break;
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TilePathSetting()
        {
            InnerSetting.PropertyChanged += OnInnerSettingPropertyChanged;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="code">コード値</param>
        public TilePathSetting(int code)
        {
            InnerSetting = TilePathSettingFactory.Create(code);
            InnerSetting.PropertyChanged += OnInnerSettingPropertyChanged;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 通行許可設定を"許可"に変更する。
        /// </summary>
        /// <param name="cannotPassingFlags">[Nullable] 通行方向設定</param>
        public void ChangePathPermissionAllow(TileCannotPassingFlags cannotPassingFlags = null)
        {
            InnerSetting = TilePathSettingFactory.Create(
                TilePathPermission.Allow, InnerSetting,
                cannotPassingFlags: cannotPassingFlags);
        }

        /// <summary>
        /// 通行許可設定を"下レイヤーに依存"に変更する。
        /// </summary>
        /// <param name="cannotPassingFlags">[Nullable] 通行方向設定</param>
        public void ChangePathPermissionDependent(TileCannotPassingFlags cannotPassingFlags = null)
        {
            InnerSetting = TilePathSettingFactory.Create(
                TilePathPermission.Dependent, InnerSetting,
                cannotPassingFlags: cannotPassingFlags);
        }

        /// <summary>
        /// 通行許可設定を"通行不可"に変更する。
        /// </summary>
        public void ChangePathPermissionDeny()
        {
            InnerSetting = TilePathSettingFactory.Create(
                TilePathPermission.Deny, InnerSetting);
        }

        /// <summary>
        /// 通行許可設定を"部分的に通行不可"に変更する。
        /// </summary>
        /// <param name="impassableFlags">[Nullable] 通行許可設定</param>
        public void ChangePathPermissionPartialDeny(TileImpassableFlags impassableFlags = null)
        {
            InnerSetting = TilePathSettingFactory.Create(
                TilePathPermission.PartialDeny, InnerSetting,
                impassableFlags);
        }

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public override bool Equals(TilePathSetting other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return InnerSetting.Equals(other.InnerSetting);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Common
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// バイナリ変換する。
        /// </summary>
        /// <returns>バイナリデータ</returns>
        public byte[] ToBinary() => InnerSetting.ToBinary();
    }
}