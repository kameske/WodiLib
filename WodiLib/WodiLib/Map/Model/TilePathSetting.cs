// ========================================
// Project Name : WodiLib
// File Name    : TilePathSetting.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Sys;

namespace WodiLib.Map
{
    /// <summary>
    /// タイル通行許可設定
    /// </summary>
    public class TilePathSetting
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// タイル通行許可
        /// </summary>
        public TilePathPermission PathPermission => innerSetting.PathPermission;

        /// <summary>
        /// タイル通行不可フラグ
        /// </summary>
        /// <exception cref="PropertyAccessException">タイル通行許可が"通行不可"以外の場合</exception>
        public TileImpassableFlags ImpassableFlags => innerSetting.ImpassableFlags;

        /// <summary>
        /// [NotNull] タイル通行設定オプション
        /// </summary>
        /// <exception cref="PropertyNullException">nullを設定した場合</exception>
        public TilePathOption PathOption
        {
            get => innerSetting.PathOption;
            set => innerSetting.PathOption = value;
        }

        /// <summary>
        /// タイル通行方向設定
        /// </summary>
        /// <exception cref="PropertyAccessException">タイル通行許可が"通行不可"の場合</exception>
        public TileCannotPassingFlags CannotPassingFlags => innerSetting.CannotPassingFlags;

        /// <summary>
        /// カウンター属性
        /// </summary>
        public bool IsCounter
        {
            get => innerSetting.IsCounter;
            set => innerSetting.IsCounter = value;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private ITilePathSetting innerSetting = new TilePathSettingAllow();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TilePathSetting()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="code">コード値</param>
        public TilePathSetting(int code)
        {
            innerSetting = TilePathSettingFactory.Create(code);
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
            innerSetting = TilePathSettingFactory.Create(
                TilePathPermission.Allow, innerSetting,
                cannotPassingFlags: cannotPassingFlags);
        }

        /// <summary>
        /// 通行許可設定を"下レイヤーに依存"に変更する。
        /// </summary>
        /// <param name="cannotPassingFlags">[Nullable] 通行方向設定</param>
        public void ChangePathPermissionDependent(TileCannotPassingFlags cannotPassingFlags = null)
        {
            innerSetting = TilePathSettingFactory.Create(
                TilePathPermission.Dependent, innerSetting,
                cannotPassingFlags: cannotPassingFlags);
        }

        /// <summary>
        /// 通行許可設定を"通行不可"に変更する。
        /// </summary>
        public void ChangePathPermissionDeny()
        {
            innerSetting = TilePathSettingFactory.Create(
                TilePathPermission.Deny, innerSetting);
        }

        /// <summary>
        /// 通行許可設定を"部分的に通行不可"に変更する。
        /// </summary>
        /// <param name="impassableFlags">[Nullable] 通行許可設定</param>
        public void ChangePathPermissionPartialDeny(TileImpassableFlags impassableFlags = null)
        {
            innerSetting = TilePathSettingFactory.Create(
                TilePathPermission.PartialDeny, innerSetting,
                impassableFlags);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Common
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// バイナリ変換する。
        /// </summary>
        /// <returns>バイナリデータ</returns>
        public byte[] ToBinary() => innerSetting.ToBinary();
    }
}