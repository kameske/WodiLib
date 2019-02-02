// ========================================
// Project Name : WodiLib
// File Name    : AnimateSpeed.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Sys;

namespace WodiLib.Event
{
    /// <summary>
    /// アニメ頻度
    /// </summary>
    public class AnimateSpeed : TypeSafeEnum<AnimateSpeed>
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
        public static readonly AnimateSpeed Longest;

        /// <summary>値</summary>
        public byte Code { get; }

        static AnimateSpeed()
        {
            Frame = new AnimateSpeed("Frame", 0x00);
            Shortest = new AnimateSpeed("Shortest", 0x01);
            Short = new AnimateSpeed("Short", 0x02);
            Middle = new AnimateSpeed("Middle", 0x03);
            Long = new AnimateSpeed("Long", 0x04);
            Longest = new AnimateSpeed("Longest", 0x05);
        }

        private AnimateSpeed(string id, byte code) : base(id)
        {
            Code = code;
        }

        /// <summary>
        /// バイト値からインスタンスを取得する。
        /// </summary>
        /// <param name="code">バイト値</param>
        /// <returns>インスタンス</returns>
        public static AnimateSpeed FromByte(byte code)
        {
            return _FindFirst(x => x.Code == code);
        }
    }
}