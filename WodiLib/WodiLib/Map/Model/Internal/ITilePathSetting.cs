// ========================================
// Project Name : WodiLib
// File Name    : ITilePathSetting.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Sys;

namespace WodiLib.Map
{
    /// <summary>
    /// タイル通行許可設定インタフェース
    /// </summary>
    internal interface ITilePathSetting : IModelBase<ITilePathSetting>
    {
        /// <summary>
        /// タイル通行許可
        /// </summary>
        TilePathPermission PathPermission { get; }

        /// <summary>
        /// タイル通行不可フラグ
        /// </summary>
        /// <exception cref="PropertyAccessException">タイル通行許可が"通行不可"以外の場合</exception>
        TileImpassableFlags ImpassableFlags { get; }

        /// <summary>
        /// [NotNull] タイル通行設定オプション
        /// </summary>
        /// <exception cref="PropertyNullException">nullを設定した場合</exception>
        TilePathOption PathOption { get; set; }

        /// <summary>
        /// タイル通行方向設定
        /// </summary>
        /// <exception cref="PropertyAccessException">タイル通行許可が"通行不可"の場合</exception>
        TileCannotPassingFlags CannotPassingFlags { get; }

        /// <summary>
        /// カウンター属性
        /// </summary>
        bool IsCounter { get; set; }

        /// <summary>
        /// バイナリ変換する。
        /// </summary>
        /// <returns>バイナリデータ</returns>
        byte[] ToBinary();
    }
}