// ========================================
// Project Name : WodiLib
// File Name    : TilePathSettingPartialDeny.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using WodiLib.Sys;

namespace WodiLib.Map
{
    /// <summary>
    /// タイル通行許可設定・部分的に拒否
    /// </summary>
    [Serializable]
    internal class TilePathSettingPartialDeny : TilePathSettingBase, IEquatable<TilePathSettingPartialDeny>
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
            if (impassableFlags is null)
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
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public bool Equals(TilePathSettingPartialDeny other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return ImpassableFlags.Equals(other.ImpassableFlags);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Override Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc/>
        protected override bool ChildEquals(ITilePathSetting other)
        {
            if (other is null) return false;
            if (!(other is TilePathSettingPartialDeny casted)) return false;
            return Equals(casted);
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

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Serializable
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="info">デシリアライズ情報</param>
        /// <param name="context">コンテキスト</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected TilePathSettingPartialDeny(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}