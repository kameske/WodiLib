// ========================================
// Project Name : WodiLib
// File Name    : TilePathSettingAllow.cs
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
    /// タイル通行許可設定・許可
    /// </summary>
    [Serializable]
    internal class TilePathSettingAllow : TilePathSettingBase, IEquatable<TilePathSettingAllow>
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
            if (cannotPassingFlags is null)
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
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public bool Equals(TilePathSettingAllow other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (ReferenceEquals(null, other)) return false;
            return CannotPassingFlags.Equals(other.CannotPassingFlags);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Override Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc/>
        protected override bool ChildEquals(ITilePathSetting other)
        {
            if (other is null) return false;
            if (!(other is TilePathSettingAllow casted)) return false;
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
            result += CannotPassingFlags.ToCode();
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
        protected TilePathSettingAllow(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}