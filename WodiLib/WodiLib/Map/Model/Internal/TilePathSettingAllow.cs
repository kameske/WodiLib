// ========================================
// Project Name : WodiLib
// File Name    : TilePathSettingAllow.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using WodiLib.Sys;

namespace WodiLib.Map
{
    /// <summary>
    /// タイル通行許可設定・許可
    /// </summary>
    internal class TilePathSettingAllow : TilePathSettingBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// [NotNull] タイル通行許可
        /// </summary>
        public override TilePathPermission PathPermission => TilePathPermission.Allow;

        /// <summary>
        /// [NotNull] タイル通行方向設定
        /// </summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        /// <exception cref="PropertyAccessException">タイル通行許可が"通行不可"以外の場合</exception>
        public override TileCannotPassingFlags CannotPassingFlags { get; } = new TileCannotPassingFlags();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TilePathSettingAllow()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="cannotPassingFlags">[NotNull] 通行設定フラグ</param>
        /// <exception cref="ArgumentNullException">cannotPattingFlagsがnullの場合</exception>
        public TilePathSettingAllow(TileCannotPassingFlags cannotPassingFlags)
        {
            if (cannotPassingFlags == null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(cannotPassingFlags)));

            CannotPassingFlags = cannotPassingFlags;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="code">コード値</param>
        public TilePathSettingAllow(int code) : base(code)
        {
            CannotPassingFlags = new TileCannotPassingFlags(code);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Common
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// バイナリ変換する。
        /// </summary>
        /// <returns>バイナリデータ</returns>
        public override byte[] ToBinary()
        {
            var result = 0;
            result += PathPermission.Code;
            result += CannotPassingFlags.ToCode();
            result += PathOption.Code;
            result += IsCounter ? CounterOnCode : 0;
            return result.ToWoditorIntBytes();
        }
    }
}