// ========================================
// Project Name : WodiLib
// File Name    : AnimateSpeed.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Linq;
using WodiLib.Sys;

namespace WodiLib.Event
{
    /// <summary>
    ///     アニメ頻度
    /// </summary>
    public record AnimateSpeed : TypeSafeEnum<AnimateSpeed>
    {
        /// <summary>0:毎フレーム</summary>
        public static readonly AnimateSpeed Frame;

        /// <summary>1:超短間隔</summary>
        public static readonly AnimateSpeed Shortest;

        /// <summary>2:短間隔</summary>
        public static readonly AnimateSpeed Short;

        /// <summary>3:中間隔く</summary>
        public static readonly AnimateSpeed Middle;

        /// <summary>4:大間隔</summary>
        public static readonly AnimateSpeed Long;

        /// <summary>5:超大間隔</summary>
        public static readonly AnimateSpeed Longer;

        /// <summary>6:頻度遅</summary>
        public static readonly AnimateSpeed Longest;

        /// <summary>値</summary>
        public byte Code { get; }

        /// <summary>マップイベント移動ルートに対する使用可能フラグ</summary>
        public bool CanSetForMapEventMoveRoute { get; }

        static AnimateSpeed()
        {
            Frame = new AnimateSpeed("Frame", 0x00, true);
            Shortest = new AnimateSpeed("Shortest", 0x01, true);
            Short = new AnimateSpeed("Short", 0x02, true);
            Middle = new AnimateSpeed("Middle", 0x03, true);
            Long = new AnimateSpeed("Long", 0x04, true);
            Longer = new AnimateSpeed("Longer", 0x05, true);
            Longest = new AnimateSpeed("Longest", 0x06, false);
        }

        private AnimateSpeed(string id, byte code, bool canSetForMapEventMoveRoute) : base(id)
        {
            Code = code;
            CanSetForMapEventMoveRoute = canSetForMapEventMoveRoute;
        }

        /// <summary>
        ///     バイト値からインスタンスを取得する。
        /// </summary>
        /// <param name="code">バイト値</param>
        /// <returns>インスタンス</returns>
        public static AnimateSpeed FromByte(byte code)
        {
            return AllItems.First(x => x.Code == code);
        }

        /// <inheritdoc/>
        public override string ToString()
            => base.ToString();
    }
}
