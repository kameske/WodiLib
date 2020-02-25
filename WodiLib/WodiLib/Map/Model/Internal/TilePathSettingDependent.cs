// ========================================
// Project Name : WodiLib
// File Name    : TilePathSettingDependent.cs
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
    /// タイル通行許可設定・下レイヤーに依存
    /// </summary>
    [Serializable]
    internal class TilePathSettingDependent : TilePathSettingBase, IEquatable<TilePathSettingDependent>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// [NotNull] タイル通行許可
        /// </summary>
        public override TilePathPermission PathPermission => TilePathPermission.Dependent;

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
        public TilePathSettingDependent()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="cannotPassingFlags">[NotNull] 通行設定フラグ</param>
        /// <exception cref="ArgumentNullException">cannotPattingFlagsがnullの場合</exception>
        public TilePathSettingDependent(TileCannotPassingFlags cannotPassingFlags)
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
        public TilePathSettingDependent(int code) : base(code)
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
        public bool Equals(TilePathSettingDependent other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return CannotPassingFlags.Equals(other.CannotPassingFlags);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Override Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc/>
        protected override bool ChildEquals(ITilePathSetting other)
        {
            if (other is null) return false;
            if (!(other is TilePathSettingDependent casted)) return false;
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
        protected TilePathSettingDependent(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}