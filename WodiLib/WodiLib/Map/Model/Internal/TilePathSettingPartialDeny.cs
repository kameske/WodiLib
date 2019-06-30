// ========================================
// Project Name : WodiLib
// File Name    : TilePathSettingPartialDeny.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using WodiLib.Sys;

namespace WodiLib.Map
{
    /// <summary>
    /// タイル通行許可設定・部分的に拒否
    /// </summary>
    internal class TilePathSettingPartialDeny : TilePathSettingBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// [NotNull] タイル通行許可
        /// </summary>
        public override TilePathPermission PathPermission => TilePathPermission.PartialDeny;

        /// <summary>
        /// [NotNull] タイル通行不可フラグ
        /// </summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        /// <exception cref="PropertyAccessException">タイル通行許可が"通行不可"以外の場合</exception>
        public override TileImpassableFlags ImpassableFlags { get; } = new TileImpassableFlags();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TilePathSettingPartialDeny()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="impassableFlags">[NotNull] 通行設定フラグ</param>
        /// <exception cref="ArgumentNullException">cannotPattingFlagsがnullの場合</exception>
        public TilePathSettingPartialDeny(TileImpassableFlags impassableFlags)
        {
            if (impassableFlags == null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(impassableFlags)));

            ImpassableFlags = impassableFlags;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="code">コード値</param>
        public TilePathSettingPartialDeny(int code) : base(code)
        {
            ImpassableFlags = new TileImpassableFlags(code);
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
            result += ImpassableFlags.ToCode();
            result += PathOption.Code;
            result += IsCounter ? CounterOnCode : 0;
            return result.ToWoditorIntBytes();
        }
    }
}