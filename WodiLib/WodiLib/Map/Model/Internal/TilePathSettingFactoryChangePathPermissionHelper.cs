// ========================================
// Project Name : WodiLib
// File Name    : TilePathSettingFactoryChangePathPermissionHelper.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;

namespace WodiLib.Map
{
    /// <summary>
    /// タイル設定インスタンス設定変更処理ヘルパークラス
    /// </summary>
    internal class TilePathSettingFactoryChangePathPermissionHelper
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private readonly TilePathPermission pathPermission;
        private readonly ITilePathSetting src;
        private readonly TileImpassableFlags impassableFlags;
        private readonly TileCannotPassingFlags cannotPassingFlags;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        public TilePathSettingFactoryChangePathPermissionHelper(TilePathPermission pathPermission,
            ITilePathSetting src, TileImpassableFlags impassableFlags, TileCannotPassingFlags cannotPassingFlags)
        {
            this.pathPermission = pathPermission;
            this.src = src;
            this.impassableFlags = impassableFlags;
            this.cannotPassingFlags = cannotPassingFlags;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 処理を実行して通行設定インスタンスを生成する。
        /// </summary>
        /// <returns>通行設定インスタンス</returns>
        public ITilePathSetting Execute()
        {
            if (pathPermission.GroupCode != src.PathPermission.GroupCode)
            {
                return Create_ToDifference(pathPermission, src);
            }

            switch (pathPermission.GroupCode)
            {
                case TilePathPermission.InnerFlagGroup.Impassible:
                    return Create_ImpassableToImpassable(pathPermission, src, impassableFlags);

                case TilePathPermission.InnerFlagGroup.CannotPassing:
                    return Create_CannotPassingToCannotPassing(pathPermission, src, cannotPassingFlags);

                case TilePathPermission.InnerFlagGroup.Not:
                    return Create_Not();

                default:
                    throw new InvalidOperationException();
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Static Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private static ITilePathSetting Create_CannotPassingToCannotPassing(TilePathPermission pathPermission,
            ITilePathSetting src, TileCannotPassingFlags cannotPassingFlags)
        {
            if (pathPermission == TilePathPermission.Allow)
            {
                return new TilePathSettingAllow(cannotPassingFlags ?? src.CannotPassingFlags)
                {
                    PathOption = src.PathOption,
                    IsCounter = src.IsCounter
                };
            }

            if (pathPermission == TilePathPermission.Dependent)
            {
                return new TilePathSettingDependent(cannotPassingFlags ?? src.CannotPassingFlags)
                {
                    PathOption = src.PathOption,
                    IsCounter = src.IsCounter
                };
            }

            throw new InvalidOperationException();
        }

        private static ITilePathSetting Create_ImpassableToImpassable(TilePathPermission pathPermission,
            ITilePathSetting src, TileImpassableFlags impassableFlags)
        {
            if (pathPermission == TilePathPermission.PartialDeny)
            {
                return new TilePathSettingPartialDeny(impassableFlags ?? src.ImpassableFlags)
                {
                    PathOption = src.PathOption,
                    IsCounter = src.IsCounter
                };
            }

            throw new InvalidOperationException();
        }

        private static ITilePathSetting Create_Not()
        {
            return new TilePathSettingDeny();
        }

        private static ITilePathSetting Create_ToDifference(TilePathPermission pathPermission,
            ITilePathSetting src)
        {
            if (pathPermission == TilePathPermission.Allow)
            {
                return new TilePathSettingAllow
                {
                    PathOption = src.PathOption,
                    IsCounter = src.IsCounter
                };
            }

            if (pathPermission == TilePathPermission.Dependent)
            {
                return new TilePathSettingDependent
                {
                    PathOption = src.PathOption,
                    IsCounter = src.IsCounter
                };
            }

            if (pathPermission == TilePathPermission.Deny)
            {
                return new TilePathSettingDeny
                {
                    PathOption = src.PathOption,
                    IsCounter = src.IsCounter
                };
            }

            if (pathPermission == TilePathPermission.PartialDeny)
            {
                return new TilePathSettingPartialDeny
                {
                    PathOption = src.PathOption,
                    IsCounter = src.IsCounter
                };
            }

            throw new InvalidOperationException();
        }
    }
}