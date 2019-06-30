// ========================================
// Project Name : WodiLib
// File Name    : TilePathSettingFactory.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;

namespace WodiLib.Map
{
    /// <summary>
    /// TilePathSettingクラスFactory
    /// </summary>
    internal static class TilePathSettingFactory
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Static Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コード値から通行許可設定インスタンスを生成する。
        /// </summary>
        /// <param name="code">コード値</param>
        /// <returns>通行許可設定インスタンス</returns>
        public static ITilePathSetting Create(int code)
        {
            var permission = TilePathPermission.FromCode(code);
            if (permission == TilePathPermission.Allow)
            {
                return new TilePathSettingAllow(code);
            }

            if (permission == TilePathPermission.Dependent)
            {
                return new TilePathSettingDependent(code);
            }

            if (permission == TilePathPermission.Deny)
            {
                return new TilePathSettingDeny(code);
            }

            if (permission == TilePathPermission.PartialDeny)
            {
                return new TilePathSettingPartialDeny(code);
            }

            throw new InvalidOperationException();
        }

        /// <summary>
        /// 通行設定を変更したインスタンスを生成する。
        /// </summary>
        /// <param name="pathPermission">[NotNull] 通行設定</param>
        /// <param name="src">[NotNull] 元データ</param>
        /// <param name="impassableFlags">[Nullable] 通行許可設定</param>
        /// <param name="cannotPassingFlags">[Nullable] 通行方向設定</param>
        /// <returns>変更後のインスタンス</returns>
        public static ITilePathSetting Create(TilePathPermission pathPermission, ITilePathSetting src,
            TileImpassableFlags impassableFlags = null, TileCannotPassingFlags cannotPassingFlags = null)
        {
            return new TilePathSettingFactoryChangePathPermissionHelper(pathPermission, src,
                impassableFlags, cannotPassingFlags).Execute();
        }
    }
}